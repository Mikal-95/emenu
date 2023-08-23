using System;
using EmenuQuiz.DTOs.ImageDto;
using EmenuQuiz.DTOs.Product;
using EmenuQuiz.IRepository.IImageRepository;
using EmenuQuiz.IRepository.IProductRepository;
using EmenuQuiz.Services.ProductService;
using Microsoft.AspNetCore.Mvc;

namespace EmenuQuiz.Controllers.ImageUploaderController
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ImageController : ControllerBase
    {

        private readonly IImageRepository _IImageRepository;

        public ImageController(IImageRepository imageRepository)
        {
            _IImageRepository = imageRepository;

        }

        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = 100_000_000_000)]
        [HttpPost()]
        public async Task<ActionResult> UploadImage(int? productId, IFormFile image)
        {
            var result = await _IImageRepository.UploadImage(image, productId);
            return Ok(result);
        }

        [HttpDelete()]
        public async Task<ActionResult> RemoveImage(int imageId)
        {
            var result = await _IImageRepository.RemoveImage(imageId);
            return Ok(result);
        }

        [HttpGet("ViewImage")]
        public async Task<FileContentResult> ViewImage(int imageId)
        {
            var result = await _IImageRepository.ViewImage(imageId);
            return File(result.Key, result.Value);
        }

        [HttpPost("AllImages")]
        public async Task<ActionResult> GetImages([FromBody] ImageFilterDto imageFilterDto, int pageSize = 10, int pageIndex = 1)
        {
            var result = await _IImageRepository.GetImages(imageFilterDto, pageSize, pageIndex);
            return Ok(result);
        }
    }
}
