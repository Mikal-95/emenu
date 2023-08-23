using System;

#nullable disable
namespace EmenuQuiz.Models
{
	public partial class Image
	{

		public int Id { get; set; }
		public string Name { get; set; }
		public string PublicId { get; set; }
		public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
		public string ImageUrl { get; set; }
		public long ImageSize { get; set; }
		public int? ProductId { get; set; }

		public Product Product { get; set; }
    }
}

