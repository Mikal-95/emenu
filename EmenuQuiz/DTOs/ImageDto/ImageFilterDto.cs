using System;
using EmenuQuiz.Models;
using Microsoft.Extensions.Hosting;

#nullable disable
namespace EmenuQuiz.DTOs.ImageDto
{
	public class ImageFilterDto
	{
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }



        public bool IsEmpty()
        {
            return StartDate == null
                && EndDate == null
                && string.IsNullOrWhiteSpace(Name);
        }

        public bool HasDateSearch()
        {
            return (StartDate != null && StartDate != DateTime.MinValue)
                || (EndDate != null && EndDate != DateTime.MinValue);
        }

        public bool HasStringSearch()
        {
            return !string.IsNullOrWhiteSpace(Name);
        }
    }
}

