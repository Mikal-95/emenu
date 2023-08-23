using System;
using System.ComponentModel.DataAnnotations;
using EmenuQuiz.Models;


#nullable disable
namespace EmenuQuiz.DTOs.CategoryDto
{
	public class CategoryPostDto
	{
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }


    
}

