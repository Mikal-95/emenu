using System.Net;
using EmenuQuiz.DTOs;
using EmenuQuiz.DTOs.ImageDto;
using EmenuQuiz.DTOs.PhotoDto;
using EmenuQuiz.IRepository.IImageRepository;
using EmenuQuiz.IRepository.IPhotoUploaderRepository;
using EmenuQuiz.Models;
using EmenuQuiz.Utility;
using Microsoft.EntityFrameworkCore;


namespace EmenuQuiz.Services.ImageService
{
	public class ImageRepository : IImageRepository
	{
        private readonly MySqlDbContext _MySqlDbContext;
        private readonly IPhotoUploaderRepository _IPhotoUploaderRepository;

        public ImageRepository(MySqlDbContext mySqlDbContext, IPhotoUploaderRepository photoUploaderRepository)
        {
            _MySqlDbContext = mySqlDbContext;
            _IPhotoUploaderRepository = photoUploaderRepository;
        }

        public async Task<ListCount> GetImages(ImageFilterDto imageFilterDto, int pageSize, int pageIndex)
        {
            bool serverSearch = imageFilterDto != null && imageFilterDto.HasDateSearch();
            bool localSearch = imageFilterDto != null && imageFilterDto.HasStringSearch();

            var images = await _MySqlDbContext.Images
                .Where(ca => !serverSearch || (imageFilterDto.StartDate == null
                || (ca.CreatedAt > imageFilterDto.StartDate.Value
                && ca.CreatedAt < imageFilterDto.EndDate.Value.AddDays(1).Date)))
                .Select(im => new ImageGetDto
            {
                Id = im.Id,
                Name = im.Name,
                CreatedAt = im.CreatedAt,
                UpdatedAt = im.UpdatedAt,
                ImageSize = im.ImageSize

            }).ToListAsync();

            if (images != null && images.Any())
            {
                if (localSearch)
                {
                    images = images.Where(e =>
                    (string.IsNullOrWhiteSpace(imageFilterDto.Name)
                    || e.Name.Contains(imageFilterDto.Name))).ToList();
                }
            }

            return new ListCount
            {
                Count = images.Count(),
                Items = images.Skip((pageIndex - 1) * pageSize).Take(pageSize)
            };
        }

        public async Task<APIResult> RemoveImage(int imageId)
        {
            APIResult result = new APIResult();

            var singleImage = await _MySqlDbContext.Images.Where(im => im.Id == imageId).FirstOrDefaultAsync();
            if(singleImage == null)
            {
                return result.FailMe(-1, "Image is not exist");
            }

            result =  await _IPhotoUploaderRepository.RemovePhoto(singleImage.PublicId);

            _MySqlDbContext.Remove(singleImage);
            await _MySqlDbContext.SaveChangesAsync();


            return result;

        }

        public async Task<APIResult> UploadImage(IFormFile formFile, int? productId)
        {

            APIResult result = new APIResult();

            try
            {
                var uploadedFile = await _IPhotoUploaderRepository.UploadPhoto(formFile);
                PhotoGetDto photoGetDto = uploadedFile.Result;

                Image newImage = new Image
                {
                    Name = photoGetDto.PublicId,
                    PublicId = photoGetDto.PublicId,
                    CreatedAt = photoGetDto.CreatedAt,
                    UpdatedAt = photoGetDto.CreatedAt,
                    ImageSize = photoGetDto.ImageSize,
                    ImageUrl = photoGetDto.Url,
                    ProductId = productId!=null? productId:null,
                };


                _MySqlDbContext.Images.Add(newImage);
                await _MySqlDbContext.SaveChangesAsync();


                return result.SuccessMe(newImage.Id, "Upload success", false, APIResult.RESPONSE_CODE.OK);
            }

            catch
            {
                return result.FailMe(-1, "Error uploading the image");
            }
        }

        public async Task<KeyValuePair<byte[], string>> ViewImage(int imageId)
        {
            var singleImage = await _MySqlDbContext.Images.Where(im => im.Id == imageId).FirstOrDefaultAsync();
            if (singleImage == null)
            {
                throw new HttpStatusException("File Not Found", "FILE_NOT_FOUND_ERR", HttpStatusCode.NotFound);
            }
            return await _IPhotoUploaderRepository.ViewPhoto(singleImage.PublicId);
        }
    }
}