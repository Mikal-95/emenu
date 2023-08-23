using EmenuQuiz.Models;
using Microsoft.EntityFrameworkCore;

using System;



namespace EmenuQuiz.Models
{
    public partial class MySqlDbContext : DbContext
    {
        private readonly IConfiguration _IConfiguration;
        public MySqlDbContext(IConfiguration iConfiguration)
        {
            _IConfiguration = iConfiguration;
        }

        public MySqlDbContext(DbContextOptions<DbContext> options, IConfiguration iConfiguration)
            : base(options)
        {
            _IConfiguration = iConfiguration;
        }

        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!; 
        public virtual DbSet<Image> Images { get; set; } = null!;
        public virtual DbSet<ProductTranslation> ProductTranslations { get; set; } = null!;
        public virtual DbSet<ProductCategory> ProductCategories { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL("server=localhost;database=emenu;uid=root;pwd=MzhgSEk2T8XH8f6!;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(_IConfiguration["Schema"])
                .UseCollation("USING_NLS_COMP");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.Id)
                    .HasPrecision(10)
                    .HasColumnName("id");


                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");


                entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.Description)
                    .HasMaxLength(1000)
                    .HasColumnName("description");
            });


            modelBuilder.Entity<Image>(entity =>
            {
                entity.ToTable("Image");

                entity.Property(e => e.Id)
                    .HasPrecision(10)
                    .HasColumnName("id");


                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");


                entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.PublicId)
                    .HasMaxLength(255)
                    .HasColumnName("public_id");

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(1000)
                    .HasColumnName("image_url");

                entity.Property(e => e.ImageSize)
                    .HasPrecision(10)
                    .HasColumnName("image_size");

                entity.Property(e => e.ProductId)
                    .HasPrecision(10)
                    .HasColumnName("product_id");

                entity.HasOne(d => d.Product)
                   .WithMany(p => p.images)
                   .HasForeignKey(d => d.ProductId)
                   .HasConstraintName("Image_FK");
            });
             
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.Id)
                    .HasPrecision(10)
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.Description)
                    .HasMaxLength(1000)
                    .HasColumnName("description");

                entity.Property(e => e.InvNumber)
                .HasMaxLength(255)
                    .HasColumnName("inventory_number");

                entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.Property(e => e.Price)
                      .HasPrecision(10)
                      .HasColumnName("price");

                entity.Property(e => e.Cost)
                    .HasPrecision(10)
                    .HasColumnName("cost");
            });


            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.ToTable("ProductCategory");

                entity.Property(e => e.Id)
                    .HasPrecision(10)
                    .HasColumnName("id");

                entity.Property(e => e.ProductId)
                    .HasPrecision(10)
                    .HasColumnName("product_id");

                entity.Property(e => e.CategoryId)
                    .HasPrecision(10)
                    .HasColumnName("category_id");

                entity.HasOne(d => d.Product)
                   .WithMany(p => p.productCategories)
                   .HasForeignKey(d => d.ProductId)
                   .HasConstraintName("ProductCategory_FK");

                entity.HasOne(d => d.Category)
                   .WithMany(p => p.productCategories)
                   .HasForeignKey(d => d.CategoryId)
                   .HasConstraintName("ProductCategory_FK_1");
            });

            modelBuilder.Entity<ProductTranslation>(entity =>
            {
                entity.ToTable("ProductTranslation");

                entity.Property(e => e.Id)
                    .HasPrecision(10)
                    .HasColumnName("id");

                entity.Property(e => e.ProductId)
                    .HasPrecision(10)
                    .HasColumnName("product_id");

                entity.Property(e => e.Lang)
                    .HasMaxLength(10)
                    .HasColumnName("lang");


                entity.Property(e => e.Value)
                    .HasMaxLength(1000)
                    .HasColumnName("value");

                entity.HasOne(d => d.Product)
                   .WithMany(p => p.productTranslations)
                   .HasForeignKey(d => d.ProductId)
                   .HasConstraintName("ProductTranslation_FK");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
