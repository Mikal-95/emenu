using EmenuQuiz.IRepository.ICategoryRepository;
using EmenuQuiz.IRepository.IImageRepository;
using EmenuQuiz.IRepository.IPhotoUploaderRepository;
using EmenuQuiz.IRepository.IProductRepository;
using EmenuQuiz.Models;
using EmenuQuiz.Services.CategoryService;
using EmenuQuiz.Services.ImageService;
using EmenuQuiz.Services.PhotoUploaderService;
using EmenuQuiz.Services.ProductService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IPhotoUploaderRepository, PhotoUploaderRepository>();


// Add services to the container.
builder.Services.AddMvc(option => option.EnableEndpointRouting = false)
    .SetCompatibilityVersion(CompatibilityVersion.Latest)
    .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);


var connection = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<MySqlDbContext>(options => options.UseMySQL(connection));

builder.Services.AddCors(opt =>
{
opt.AddPolicy("Open",
    builder =>

        builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader());
    
});

builder.Services.AddControllers();


builder.Services.AddOptions();
builder.Services.AddMemoryCache();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EmenuQuiz", Version = "v1" });
});


builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1.0", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "API", Version = "v1.0" });
    options.DocInclusionPredicate((docName, description) => true);
});


var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("Open");
app.UseHttpsRedirection();
app.UseCors();
app.MapControllers();
app.Run();