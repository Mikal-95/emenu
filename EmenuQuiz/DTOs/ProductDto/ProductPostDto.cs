using System;
using System.ComponentModel.DataAnnotations;
using EmenuQuiz.DTOs.ImageDto;


#nullable disable
namespace EmenuQuiz.DTOs.Product
{
	public class ProductPostDto
	{
        [Required]
        public ICollection<ProductTranslationPostDto> Name { get; set; }


        public string Description { get; set; }

        [Required]
        public string InvNumber { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public decimal Cost { get; set; }



        public ICollection<ImagePostDto> Images { get; set; }
        public ICollection<int> Categories { get; set; }
    }
}

