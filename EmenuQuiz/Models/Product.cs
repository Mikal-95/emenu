using System;
using EmenuQuiz.DTOs.CategoryDto;
using EmenuQuiz.DTOs.ImageDto;
using EmenuQuiz.DTOs.Product;

#nullable disable
namespace EmenuQuiz.Models
{
	public partial class Product
	{
		public Product()
		{
            productCategories = new HashSet<ProductCategory>();
            images = new HashSet<Image>();
            productTranslations = new HashSet<ProductTranslation>();
        }


		public int Id { get; set; }
        public string Description { get; set;}
        public string InvNumber { get; set; }
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<Image> images { get; set; }
        public ICollection<ProductCategory> productCategories { get; set; }
        public ICollection<ProductTranslation> productTranslations { get; set; }


        public Product GetProduct(ProductPostDto productPostDto)
        {
            DateTime currentDate = DateTime.Now;

            Product product = new Product();

            foreach(ProductTranslationPostDto productTranslationPostDto in productPostDto.Name)
            {
                product.productTranslations.Add(new ProductTranslation
                {Lang = productTranslationPostDto.Lang, Value = productTranslationPostDto.Value});
            }


            foreach (int categoryId in productPostDto.Categories)
            {
                product.productCategories.Add(new ProductCategory
                { CategoryId = categoryId });
            }

            product.Description = productPostDto.Description;
            product.InvNumber = productPostDto.InvNumber;
            product.Price = productPostDto.Price;
            product.Cost = productPostDto.Cost;
            product.CreatedAt = currentDate;
            product.UpdatedAt = currentDate;

            return product;
        }

    }
}

