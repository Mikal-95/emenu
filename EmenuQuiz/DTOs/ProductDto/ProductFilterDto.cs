using System;
using EmenuQuiz.DTOs.Product;


#nullable disable
namespace EmenuQuiz.DTOs.ProductDto
{
	public class ProductFilterDto
	{
        public string Name { get; set; }
        public string Description { get; set; }
        public string InvNumber { get; set; }
        public string Category { get; set; }
        public decimal? Price { get; set; }
        public decimal? Cost { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool IsEmpty()
        {
            return StartDate == null
                && EndDate == null
                && string.IsNullOrWhiteSpace(Name)
                && string.IsNullOrWhiteSpace(Description)
                && string.IsNullOrWhiteSpace(InvNumber)
                && string.IsNullOrWhiteSpace(Category)
                && Price == null
                && Cost == null;
        }

        public bool HasDateSearch()
        {
            return (StartDate != null && StartDate != DateTime.MinValue)
                || (EndDate != null && EndDate != DateTime.MinValue);
        }

        public bool HasStringSearch()
        {
            return !string.IsNullOrWhiteSpace(Name)
                || !string.IsNullOrWhiteSpace(Description)
                || !string.IsNullOrWhiteSpace(InvNumber)
                || !string.IsNullOrWhiteSpace(Category)
                || Price != null
                || Cost != null;
        }
    }
}

