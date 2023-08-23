using System;
using EmenuQuiz.DTOs.CategoryDto;
using EmenuQuiz.DTOs.ImageDto;


#nullable disable
namespace EmenuQuiz.DTOs.Product
{
	public class ProductGetDto
	{
        public int Id { get; set; }
        public ICollection<ProductTranslationGetDto> Name { get; set; }
        public string Description { get; set; }
        public string InvNumber { get; set; }
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


        public ICollection<ImageGetDto> Images { get; set; }
        public ICollection<ProductCategoryDto> Categories { get; set; }

    }
}

