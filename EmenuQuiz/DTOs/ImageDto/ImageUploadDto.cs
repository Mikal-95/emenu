using System;
using Microsoft.AspNetCore.Mvc;


#nullable disable
namespace EmenuQuiz.DTOs.ImageDto
{
	public class ImageUploadDto
	{

        public IFormFile formFile { get; set; }


        public int? productId { get; set; }

    }
}