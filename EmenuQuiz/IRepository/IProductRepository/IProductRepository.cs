using System;
using EmenuQuiz.DTOs;
using EmenuQuiz.DTOs.Product;
using EmenuQuiz.DTOs.ProductDto;
using EmenuQuiz.Utility;

#nullable disable
namespace EmenuQuiz.IRepository.IProductRepository
{
	public interface IProductRepository
	{

        Task<APIResult> AddProduct(ProductPostDto productPostDto);
        Task<ListCount> GetProducts(ProductFilterDto productFilterDto, int pageSize, int pageIndex, string? lang);
        Task<APIResult> UpdateProduct(int categoryId, ProductPostDto productPostDto);
        Task<APIResult> DeleteProduct(int categoryId);
        Task<APIResult> GetProductById(int productId);
    }
}