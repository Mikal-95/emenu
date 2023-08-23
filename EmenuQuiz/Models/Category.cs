using System;
using EmenuQuiz.DTOs.CategoryDto;


#nullable disable
namespace EmenuQuiz.Models
{
	public partial class Category
	{
		public Category()
		{
			productCategories = new HashSet<ProductCategory>();
		}

		public int Id { get; set; }
        public string Name { get; set; }
		public string Description { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }


		public ICollection<ProductCategory> productCategories { get; set; }


        public Category GetCategory(CategoryPostDto categoryPostDto)
        {
            DateTime currentDate = DateTime.Now;

            Category category = new Category();
            category.Name = categoryPostDto.Name;
            category.Description = categoryPostDto.Description;
            category.CreatedAt = currentDate;
            category.UpdatedAt = currentDate;
                   
            return category;
        }

    }
}

