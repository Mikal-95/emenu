using System;

#nullable disable
namespace EmenuQuiz.Models
{
	public partial class ProductTranslation
	{
		public ProductTranslation()
		{
		}

		public int Id { get; set; }
		public string Lang { get; set; }
		public string Value { get; set; }
		public int ProductId { get; set; }
		public Product Product { get; set; }
	}
}

