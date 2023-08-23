using System;

#nullable disable
namespace EmenuQuiz.DTOs.ImageDto
{
	public class ImageGetDto
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long ImageSize { get; set; }
    }
}