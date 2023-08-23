using System;
using EmenuQuiz.DTOs;
using EmenuQuiz.DTOs.ImageDto;
using EmenuQuiz.Utility;

namespace EmenuQuiz.IRepository.IImageRepository
{
	public interface IImageRepository
	{
        Task<APIResult> UploadImage(IFormFile formFile, int? productId);

        Task<KeyValuePair<byte[], string>> ViewImage(int imageId);

        Task<APIResult> RemoveImage(int imageId);

        Task<ListCount> GetImages(ImageFilterDto imageFilterDto, int pageSize, int pageIndex);
    }
}