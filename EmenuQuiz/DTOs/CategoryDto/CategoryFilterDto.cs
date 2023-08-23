using System;

#nullable disable
namespace EmenuQuiz.DTOs.CategoryDto
{
	public class CategoryFilterDto
	{
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool IsEmpty()
        {
            return StartDate == null
                && EndDate == null
                && string.IsNullOrWhiteSpace(Name)
                && string.IsNullOrWhiteSpace(Description);
        }

        public bool HasDateSearch()
        {
            return (StartDate != null && StartDate != DateTime.MinValue)
                || (EndDate != null && EndDate != DateTime.MinValue);
        }

        public bool HasStringSearch()
        {
            return !string.IsNullOrWhiteSpace(Name)
                || !string.IsNullOrWhiteSpace(Description);
        }
    }
}

