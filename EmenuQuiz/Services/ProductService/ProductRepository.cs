using System;
using System.Transactions;
using EmenuQuiz.DTOs;
using EmenuQuiz.DTOs.CategoryDto;
using EmenuQuiz.DTOs.ImageDto;
using EmenuQuiz.DTOs.Product;
using EmenuQuiz.DTOs.ProductDto;
using EmenuQuiz.IRepository.IProductRepository;
using EmenuQuiz.Models;
using EmenuQuiz.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EmenuQuiz.Services.ProductService
{
	public class ProductRepository : IProductRepository
	{
        private readonly MySqlDbContext _MySqlDbContext;

        public ProductRepository(MySqlDbContext mySqlDbContext)
        {
            _MySqlDbContext = mySqlDbContext;
        }


        public async Task<APIResult> AddProduct(ProductPostDto productPostDto)
        {
            APIResult result = new();

            try
            {

                Product newProduct = new Product().GetProduct(productPostDto);
                _MySqlDbContext.Add(newProduct);
                await _MySqlDbContext.SaveChangesAsync();

                foreach(ImagePostDto imagePostDto in productPostDto.Images)
                {
                    var image = await _MySqlDbContext.Images.Where(i=>i.Id==imagePostDto.Id).FirstOrDefaultAsync();

                    if(image == null)
                    {
                        return result.FailMe(-1, "One of the added image is not exist.");
                    }

                    image.ProductId = newProduct.Id;

                    if(imagePostDto.Name != null)
                    {
                        image.Name = imagePostDto.Name;
                    }

                    _MySqlDbContext.Images.Update(image);
                    await _MySqlDbContext.SaveChangesAsync();
                }

                return result.SuccessMe(newProduct.Id, "Create success", false, APIResult.RESPONSE_CODE.OK, productPostDto);
            }
            catch
            {
                return result.FailMe(-1, "Failed to add product");
            }
        }

        public async Task<APIResult> DeleteProduct(int productId)
        {
            APIResult result = new();

            try
            {
                var singleProduct = await _MySqlDbContext.Products.Where(c => c.Id == productId).FirstOrDefaultAsync();

                if (singleProduct == null)
                {
                    return result.FailMe(-1, "Product not found");
                }

                _MySqlDbContext.Products.Remove(singleProduct);
                await _MySqlDbContext.SaveChangesAsync();

                return result.SuccessMe(singleProduct.Id, "Success", false, APIResult.RESPONSE_CODE.CREATED);
            }
            catch
            {
                return result.FailMe(-1, "Failed to remove product");
            }
        }

        public async Task<ListCount> GetProducts(ProductFilterDto productFilterDto, int pageSize, int pageIndex, string? lang)
        {
            bool serverSearch = productFilterDto != null && productFilterDto.HasDateSearch();
            bool localSearch = productFilterDto != null && productFilterDto.HasStringSearch();

            var products = await _MySqlDbContext.Products
                .OrderByDescending(p=>p.Id)
                .Where(ca => !serverSearch || (productFilterDto.StartDate == null
                || (ca.CreatedAt > productFilterDto.StartDate.Value
                && ca.CreatedAt < productFilterDto.EndDate.Value.AddDays(1).Date)))
                .Select(p => new ProductGetDto

                {
                Id = p.Id,
                Name = p.productTranslations.Where(pt=>lang==null||pt.Lang.Equals(lang)).Select(pt => new ProductTranslationGetDto
                {
                    Id = pt.Id,
                    Lang = pt.Lang,
                    Value = pt.Value
                }).ToList(),

                Description = p.Description,
                InvNumber = p.InvNumber,
                Price = p.Price,
                Cost = p.Cost,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                Categories = p.productCategories.Select(pc=>new ProductCategoryDto
                {
                    Id = pc.Id,
                    Name = pc.Category.Name
                }).ToList(),
                Images = p.images.Select(im=>new ImageGetDto
                {
                    Id=im.Id,
                    Name = im.Name,
                    CreatedAt = im.CreatedAt,
                    UpdatedAt = im.UpdatedAt,
                    ImageSize = im.ImageSize,

                }).ToList()

            }).ToListAsync();


            if (products != null && products.Any())
            {
                if (localSearch)
                {
                    products = products.Where(e =>
                    (string.IsNullOrWhiteSpace(productFilterDto.Name)
                    || e.Name.Select(n => n.Value.ToLower()).Contains(productFilterDto.Name.ToLower())
                    )
                    &&
                    (string.IsNullOrWhiteSpace(productFilterDto.Description)
                    || e.Description.ToLower().Contains(productFilterDto.Description.ToLower()))
                    &&
                    (string.IsNullOrWhiteSpace(productFilterDto.InvNumber)
                    || e.InvNumber.Contains(productFilterDto.InvNumber))
                    &&
                    (string.IsNullOrWhiteSpace(productFilterDto.Category)
                    || e.Categories.Select(c=>c.Name.ToLower()).Contains(productFilterDto.Category.ToLower()))
                    &&
                    (productFilterDto.Price == null
                    || e.Price == productFilterDto.Price)
                    && (productFilterDto.Cost == null
                    || e.Cost == productFilterDto.Cost)).ToList();
                }
            }

            return new ListCount
            {
                Count = products.Count(),
                Items = products.Skip((pageIndex - 1) * pageSize).Take(pageSize)
            };
        }

        public async Task<APIResult> UpdateProduct(int productId, ProductPostDto productPostDto)
        {
            APIResult result = new();

            try
            {
                DateTime currentDate = DateTime.Now;

                var singleProduct = await _MySqlDbContext.Products
                    .Include(p=>p.productTranslations)
                    .Include(w=>w.productCategories).Where(c => c.Id == productId).FirstOrDefaultAsync();

                if (singleProduct == null)
                {
                    return result.FailMe(-1, "Product not found");
                }
                 
                singleProduct.Description = productPostDto.Description;
                singleProduct.InvNumber = productPostDto.InvNumber;
                singleProduct.Price = productPostDto.Price;
                singleProduct.Cost = productPostDto.Cost;
                singleProduct.UpdatedAt = currentDate;

                await EditProductCategoryAsync(singleProduct, productPostDto.Categories);

                await EditProductTranslationAsync(singleProduct, productPostDto.Name);

                foreach (ImagePostDto imagePostDto in productPostDto.Images)
                {
                    var image = await _MySqlDbContext.Images.Where(i => i.Id == imagePostDto.Id).FirstOrDefaultAsync();

                    if (image == null)
                    {
                        return result.FailMe(-1, "One of the added image is not exist.");
                    }

                    image.ProductId = singleProduct.Id;

                    if (imagePostDto.Name != null)
                    {
                        image.Name = imagePostDto.Name;
                    }

                    _MySqlDbContext.Images.Update(image);
                    await _MySqlDbContext.SaveChangesAsync();
                }



                _MySqlDbContext.Update(singleProduct);
                await _MySqlDbContext.SaveChangesAsync();

                return result.SuccessMe(singleProduct.Id, "Update success", false, APIResult.RESPONSE_CODE.OK, productPostDto);
            }
            catch
            {
                return result.FailMe(-1, "Failed to update product");
            }
        }

        public async Task EditProductCategoryAsync(Product? product, ICollection<int> productCategory)
        {
            using TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            if (product == null)
            {
                throw new InvalidOperationException("Product is not exist");
            }

            var oldgCategories = product.productCategories;
            await RemoveFromCategoriesAsync(oldgCategories);

            List<int> categories = new List<int>();
            categories = await _MySqlDbContext.Categories.Where(x => productCategory.Contains(x.Id)).Select(x => x.Id).ToListAsync();

            if (categories != null)
            {
                foreach (var categoryId in categories)
                {
                    product.productCategories.Add(new ProductCategory { CategoryId = categoryId });
                }
            }

            await _MySqlDbContext.SaveChangesAsync();

            scope.Complete();
        }






        public async Task EditProductTranslationAsync(Product? product, ICollection<ProductTranslationPostDto> productTranslation)
        {
            using TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            if (product == null)
            {
                throw new InvalidOperationException("Product is not exist");
            }

            var oldTranslations = product.productTranslations;
            await RemoveFromTranslationsAsync(oldTranslations);

 
                foreach (ProductTranslationPostDto productTranslationPostDto in productTranslation)
                {
                    product.productTranslations.Add(new ProductTranslation { Lang = productTranslationPostDto.Lang, Value = productTranslationPostDto.Value });
                }


            await _MySqlDbContext.SaveChangesAsync();

            scope.Complete();
        }




        // Remove Categories
        private async Task RemoveFromCategoriesAsync(ICollection<ProductCategory> oldgCategories)
        {
            foreach (var category in oldgCategories)
            {
                _MySqlDbContext.ProductCategories.Remove(category);
            }

            await _MySqlDbContext.SaveChangesAsync();
        }

        // Remove Translations
        private async Task RemoveFromTranslationsAsync(ICollection<ProductTranslation> oldTranslations)
        {
            foreach (var translation in oldTranslations)
            {
                _MySqlDbContext.ProductTranslations.Remove(translation);
            }

            await _MySqlDbContext.SaveChangesAsync();
        }

        public async Task<APIResult> GetProductById(int productId)
        {
            APIResult result = new();

            try
            {
                var singleProduct = await _MySqlDbContext.Products.Where(c => c.Id == productId).Select(p => new ProductGetDto
                {
                    Id = p.Id,
                    Name = p.productTranslations.Select(pt => new ProductTranslationGetDto
                    {
                        Id = pt.Id,
                        Lang = pt.Lang,
                        Value = pt.Value
                    }).ToList(),

                    Description = p.Description,
                    InvNumber = p.InvNumber,
                    Price = p.Price,
                    Cost = p.Cost,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    Categories = p.productCategories.Select(pc => new ProductCategoryDto
                    {
                        Id = pc.Id,
                        Name = pc.Category.Name
                    }).ToList(),
                    Images = p.images.Select(im => new ImageGetDto
                    {
                        Id = im.Id,
                        Name = im.Name,
                        CreatedAt = im.CreatedAt,
                        UpdatedAt = im.UpdatedAt,
                        ImageSize = im.ImageSize,

                    }).ToList()

                }).FirstOrDefaultAsync();

                if (singleProduct == null)
                {
                    return result.FailMe(-1, "Product not found");
                }

                return result.SuccessMe(1, "Success", false, APIResult.RESPONSE_CODE.OK, singleProduct);
            }
            catch
            {
                return result.FailMe(-1, "Failed to get product");
            }
        }
    }
}

