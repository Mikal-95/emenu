using System;
using EmenuQuiz.DTOs.CategoryDto;
using EmenuQuiz.IRepository.ICategoryRepository;
using Microsoft.AspNetCore.Mvc;

namespace EmenuQuiz.Controllers.Category
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CategoryController : ControllerBase
    {

        private readonly ICategoryRepository _ICategoryRepository;


		public CategoryController(ICategoryRepository iCategoryRepository)
		{
            _ICategoryRepository = iCategoryRepository;
		}

        [HttpPost()]
        public async Task<ActionResult> AddCategory(CategoryPostDto categoryPostDto)
        {
            var result = await _ICategoryRepository.AddCategory(categoryPostDto);
            return Ok(result);
        }

        [HttpPost("AllCategories")]
        public async Task<ActionResult> GetCategories([FromBody] CategoryFilterDto categoryFilterDto, int pageSize = 10, int pageIndex = 1)
        {
            var result = await _ICategoryRepository.GetCategories(categoryFilterDto, pageSize, pageIndex);
            return Ok(result);
        }

        [HttpPut()]
        public async Task<ActionResult> UpdateCategory(int categoryId, CategoryPostDto categoryPostDto)
        {
            var result = await _ICategoryRepository.UpdateCategory(categoryId, categoryPostDto);
            return Ok(result);
        }

        [HttpDelete()]
        public async Task<ActionResult> DeleteCategory(int categoryId)
        {
            var result = await _ICategoryRepository.DeleteCategory(categoryId);
            return Ok(result);
        }

        [HttpGet("GetById")]
        public async Task<ActionResult> GetCategoryById(int categoryId)
        {
            var result = await _ICategoryRepository.GetCategoryById(categoryId);
            return Ok(result);
        }
    }
}

