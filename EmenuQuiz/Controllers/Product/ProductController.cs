using System;
using EmenuQuiz.DTOs.Product;
using EmenuQuiz.DTOs.ProductDto;
using EmenuQuiz.IRepository.IProductRepository;
using Microsoft.AspNetCore.Mvc;

namespace EmenuQuiz.Controllers.Product
{
	[ApiController]
	[Route("api/v1/[controller]")]
	public class ProductController : ControllerBase
	{

        private readonly IProductRepository _IProductRepository;

        public ProductController(IProductRepository productRepository)
		{
            _IProductRepository = productRepository;

        }

        [HttpPost()]
        public async Task<ActionResult> AddProduct(ProductPostDto productPostDto)
        {
            var result = await _IProductRepository.AddProduct(productPostDto);
            return Ok(result);
        }

        [HttpPost("AllProducts")]
        public async Task<ActionResult> GetProducts([FromBody] ProductFilterDto productFilterDto, string? lang, int pageSize = 10, int pageIndex = 1)
        {
            var result = await _IProductRepository.GetProducts(productFilterDto, pageSize, pageIndex, lang);
            return Ok(result);
        }

        [HttpPut()]
        public async Task<ActionResult> UpdateProduct(int productId, ProductPostDto productPostDto)
        {
            var result = await _IProductRepository.UpdateProduct(productId, productPostDto);
            return Ok(result);
        }

        [HttpDelete()]
        public async Task<ActionResult> DeleteProduct(int productId)
        {
            var result = await _IProductRepository.DeleteProduct(productId);
            return Ok(result);
        }

        [HttpGet("GetById")]
        public async Task<ActionResult> GetProductById(int productId)
        {
            var result = await _IProductRepository.GetProductById(productId);
            return Ok(result);
        }
    }
}

