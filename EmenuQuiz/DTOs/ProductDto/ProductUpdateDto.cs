using System;
using EmenuQuiz.DTOs.CategoryDto;
using EmenuQuiz.DTOs.ImageDto;


#nullable disable
namespace EmenuQuiz.DTOs.Product
{
	public class ProductUpdateDto
	{
        public ICollection<ProductTranslationPostDto> Name { get; set; }
        public string Description { get; set; }
        public string InvNumber { get; set; }
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public ICollection<ImagePostDto> Images { get; set; }
        public ICollection<int> Categories { get; set; }
    }
}