using System;
using EmenuQuiz.Utility;

namespace EmenuQuiz.IRepository.IPhotoUploaderRepository
{
	public interface IPhotoUploaderRepository
    {
        Task<APIResult> UploadPhoto(IFormFile formFile);

        Task<KeyValuePair<byte[], string>> ViewPhoto(string publicId);

        Task<APIResult> RemovePhoto(string publicId);
    }
}