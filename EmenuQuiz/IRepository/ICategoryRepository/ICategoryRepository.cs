using System;
using EmenuQuiz.DTOs;
using EmenuQuiz.DTOs.CategoryDto;
using EmenuQuiz.Utility;

namespace EmenuQuiz.IRepository.ICategoryRepository
{
	public interface ICategoryRepository
	{
		Task<APIResult> AddCategory(CategoryPostDto categoryPostDto);
        Task<ListCount> GetCategories(CategoryFilterDto categoryFilterDto, int pageSize, int pageIndex);
        Task<APIResult> UpdateCategory(int categoryId, CategoryPostDto categoryPostDto);
        Task<APIResult> DeleteCategory(int categoryId);
        Task<APIResult> GetCategoryById(int categoryId);
    }
}