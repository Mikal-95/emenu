using System;
using EmenuQuiz.DTOs;
using EmenuQuiz.DTOs.CategoryDto;
using EmenuQuiz.IRepository.ICategoryRepository;
using EmenuQuiz.Models;
using EmenuQuiz.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmenuQuiz.Services.CategoryService
{
	public class CategoryRepository : ICategoryRepository
	{
        private readonly MySqlDbContext _MySqlDbContext;

		public CategoryRepository(MySqlDbContext mySqlDbContext)
		{
            _MySqlDbContext = mySqlDbContext;
		}

        public async Task<APIResult> AddCategory(CategoryPostDto categoryPostDto)
        {
            APIResult result = new();

            try
            {
                Category newCategory = new Category().GetCategory(categoryPostDto);
                _MySqlDbContext.Categories.Add(newCategory);
                await _MySqlDbContext.SaveChangesAsync();

                return result.SuccessMe(newCategory.Id, "Create success", false, APIResult.RESPONSE_CODE.OK, categoryPostDto);
            }
            catch
            {
                return result.FailMe(-1, "Failed to add category");
            }
        }

        public async Task<APIResult> DeleteCategory(int categoryId)
        {
            APIResult result = new();

            try
            {
                var singleCategory = await _MySqlDbContext.Categories.Where(c => c.Id == categoryId).FirstOrDefaultAsync();

                if(singleCategory == null)
                {
                    return result.FailMe(-1, "Category not found");
                }

                _MySqlDbContext.Categories.Remove(singleCategory);
                await _MySqlDbContext.SaveChangesAsync();

                return result.SuccessMe(singleCategory.Id, "Success", false, APIResult.RESPONSE_CODE.CREATED);
            }
            catch
            {
                return result.FailMe(-1, "Failed to remove category");
            }
        }

        public async Task<ListCount> GetCategories(CategoryFilterDto categoryFilterDto, int pageSize, int pageIndex)
        {
            bool serverSearch = categoryFilterDto != null && categoryFilterDto.HasDateSearch();
            bool localSearch = categoryFilterDto != null && categoryFilterDto.HasStringSearch();

            var categories = await _MySqlDbContext.Categories
                .Where(ca => !serverSearch || (categoryFilterDto.StartDate == null
                || (ca.CreatedAt > categoryFilterDto.StartDate.Value
                && ca.CreatedAt < categoryFilterDto.EndDate.Value.AddDays(1).Date))
            ).Select(c => new CategoryGetDto
            {
                Id= c.Id,
                Name = c.Name,
                Description = c.Description,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
            }).ToListAsync();


            if (categories != null && categories.Any())
            {
                if (localSearch)
                {
                    categories = categories.Where(e =>
                    (string.IsNullOrWhiteSpace(categoryFilterDto.Name)
                    || e.Name.ToLower().Contains(categoryFilterDto.Name.ToLower()))
                    &&
                    (string.IsNullOrWhiteSpace(categoryFilterDto.Description)
                    || e.Description.ToLower().Contains(categoryFilterDto.Description.ToLower()))).ToList();
                }
            }


            return new ListCount
            {
                Count = categories.Count(),
                Items = categories.Skip((pageIndex - 1) * pageSize).Take(pageSize)
            };
        }

        public async Task<APIResult> GetCategoryById(int categoryId)
        {
            APIResult result = new();

            try
            {
                var singleCategory = await _MySqlDbContext.Categories.Where(c => c.Id == categoryId).Select(ca=> new CategoryGetDto
                {
                    Id = ca.Id,
                    Name = ca.Name,
                    Description = ca.Description,
                    CreatedAt = ca.CreatedAt,
                    UpdatedAt = ca.UpdatedAt
                }).FirstOrDefaultAsync();

                if (singleCategory == null)
                {
                    return result.FailMe(-1, "Category not found");
                }

                return result.SuccessMe(1, "success", false, APIResult.RESPONSE_CODE.OK, singleCategory);
            }
            catch
            {
                return result.FailMe(-1, "Failed to get category");
            }
        }

        public async Task<APIResult> UpdateCategory(int categoryId, CategoryPostDto categoryPostDto)
        {
            APIResult result = new();

            DateTime currentDate = DateTime.Now;

            try
            {
                var singleCategory = await _MySqlDbContext.Categories.Where(c => c.Id == categoryId).FirstOrDefaultAsync();

                if (singleCategory == null)
                {
                    return result.FailMe(-1, "Category not found");
                }


                singleCategory.Name = categoryPostDto.Name;
                singleCategory.Description = categoryPostDto.Description;
                singleCategory.UpdatedAt = currentDate;

                _MySqlDbContext.Categories.Update(singleCategory);
                await _MySqlDbContext.SaveChangesAsync();

                return result.SuccessMe(singleCategory.Id, "Update success", false, APIResult.RESPONSE_CODE.OK, categoryPostDto);
            }
            catch
            {
                return result.FailMe(-1, "Failed to update category");
            }
        }
    }
}

