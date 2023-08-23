using System;
using System.Net;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using EmenuQuiz.DTOs;
using EmenuQuiz.DTOs.PhotoDto;
using EmenuQuiz.IRepository.IPhotoUploaderRepository;
using EmenuQuiz.Models;
using EmenuQuiz.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySqlX.XDevAPI;
using Org.BouncyCastle.Ocsp;

namespace EmenuQuiz.Services.PhotoUploaderService
{
	public class PhotoUploaderRepository : IPhotoUploaderRepository
	{
        private readonly IConfiguration _IConfiguration;

        private readonly CloudinarySetting _cLoudinarySetting;

        private readonly Cloudinary _cloudinary;

        private readonly MySqlDbContext _MySQLDbContext;

        private readonly HttpClient _client = new HttpClient();

        public PhotoUploaderRepository(IConfiguration configuration, MySqlDbContext mySqlDbContext)
		{
            _IConfiguration = configuration;

            _cLoudinarySetting = _IConfiguration.GetSection("CloudinarySetting").Get<CloudinarySetting>();

            Account account = new Account(_cLoudinarySetting.CloudName, _cLoudinarySetting.APIKey, _cLoudinarySetting.APISecret);

            _cloudinary = new Cloudinary(account);

            _MySQLDbContext = mySqlDbContext;
		}

        public async Task<APIResult> RemovePhoto(string publicId)
        {
            APIResult result = new APIResult();

            try
            {
                var deletionResult = new DeletionResult();

                var deletionParams = new DeletionParams(publicId);

                deletionResult = await  _cloudinary.DestroyAsync(deletionParams);
                
                if (!deletionResult.Result.Equals("ok"))
                {
                    return result.FailMe(-1, deletionResult.Result);
                }

                return result.SuccessMe(1, "success", false, APIResult.RESPONSE_CODE.OK);

            } catch
            {
                return result.FailMe(-1, "Failed to remove photo");
            }
        }

        public async Task<APIResult> UploadPhoto(IFormFile formFile)
        {
            APIResult result = new APIResult();

            DateTime currentDate = DateTime.Now;

            try
            {
                var uploadResult = new ImageUploadResult();

                if (formFile.Length > 0)
                {
                    using(var stream = formFile.OpenReadStream())
                    {
                        var uploadParams = new ImageUploadParams()
                        {
                            File = new FileDescription(formFile.Name, stream)
                        };

                        uploadResult = await _cloudinary.UploadAsync(uploadParams);
                            
                    };
                }

                var photoGetDto = new PhotoGetDto
                {
                    Url = uploadResult.Url.ToString(),
                    CreatedAt = currentDate,
                    PublicId = uploadResult.PublicId.ToString(),
                    ImageSize = uploadResult.Bytes,
                };

                return result.SuccessMe(1, "Upload success", false, APIResult.RESPONSE_CODE.OK, photoGetDto);
            } catch
            {
                return result.FailMe(-1, "Failed to upload photo");
            }
        }

        public async Task<KeyValuePair<byte[], string>> ViewPhoto(string publicId)
        {

                var singleImage = await _MySQLDbContext.Images.Where(im => im.PublicId.Equals(publicId)).FirstOrDefaultAsync();

                if(singleImage == null)
                {
                    throw new HttpStatusException("Image Not Found", "IMAGE_NOT_FOUND_ERR", HttpStatusCode.NotFound);
                }


                var keyVal = await DownloadFileAsync("GET", singleImage.ImageUrl);

                if(keyVal.Value == null)
                {
                throw new HttpStatusException("IMAGE Not Uploaded", "IMAGE_NOT_UPLOADED_ERR", HttpStatusCode.NotFound);
            }

                return keyVal;

        }

        public async Task<KeyValuePair<byte[], string>> DownloadFileAsync(string method, string endpoint)
        {

            HttpResponseMessage response = new HttpResponseMessage();

            switch (method)
            {
                case "GET":
                    response = await _client.GetAsync(
                                    endpoint);
                    break;

                case "DELETE":
                    response = await _client.DeleteAsync(
                                    endpoint);
                    break;
            }

            if (response.IsSuccessStatusCode)
            {
                HttpContent content = response.Content;

                var contentByte = await content.ReadAsByteArrayAsync();

                if(content==null || content.Headers.ContentType == null || string.IsNullOrEmpty(content.Headers.ContentType.ToString()))
                {
                    throw new HttpStatusException("File Not Found", "FILE_NOT_FOUND_ERR", HttpStatusCode.NotFound);
                }

                return new KeyValuePair<byte[], string>(contentByte, content.Headers.ContentType.ToString());

            }

            else
            {
                throw new HttpStatusException("File Not Found", "FILE_NOT_FOUND_ERR", HttpStatusCode.NotFound);
            }

        }
    }
}

