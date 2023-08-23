using System;

#nullable disable
namespace EmenuQuiz.DTOs.PhotoDto
{
	public class PhotoGetDto
	{
		public string Url { get; set; }
		public DateTime CreatedAt { get; set; }
		public string PublicId { get; set; }
		public long ImageSize { get; set; }
	}
}

