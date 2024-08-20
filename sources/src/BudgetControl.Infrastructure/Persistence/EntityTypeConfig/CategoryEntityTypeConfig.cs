using System.Security.Cryptography.X509Certificates;
using BudgetControl.Domain.Categories;
using BudgetControl.Domain.ValueObjects;
using BudgetControl.Infrastructure.Persistence.Conventers;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BudgetControl.Infrastructure.Persistence.EntityTypeConfig;

internal sealed class CategoryEntityTypeConfig : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {

        var idConverter = new ValueConverter<CategoryId, Guid>(
            v => v.Value,
            v => new CategoryId(v)
        );

        var categoryTypeConverter = new ValueConverter<CategoryType, int>(
            v => v.Id,
            v => CategoryType.FromValue<CategoryType>(v)
        );

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(idConverter)
            .ValueGeneratedOnAdd();
        builder.Property(x => x.Title)
            .HasConversion(ValueObjectConverter.TitleConverter)
            .HasMaxLength(64)
            .IsRequired();
        builder.Property(x => x.Description)
            .HasConversion(ValueObjectConverter.DescriptionConverter)
            .HasMaxLength(1024)
            .IsRequired();
        builder.Property(x => x.CategoryType)
            .HasConversion(categoryTypeConverter)
            .IsRequired();
        builder.Property(x => x.IsDeleted)
            .IsRequired();

        // builder.HasOne(x => x.Parent)
        //     .WithMany()
        //     .HasForeignKey("ParentId")
        //     .IsRequired(false)
        //     .OnDelete(DeleteBehavior.Restrict);

        // builder.Navigation(x => x.Parent)
        //     .UsePropertyAccessMode(PropertyAccessMode.Property);

        // builder.HasData(
        //     new Category
        //     {
        //         Id = new CategoryId(1),
        //         Name = "Income",
        //         Description = "Income category",
        //         Type = CategoryType.Income
        //     },
        //     new Category
        //     {
        //         Id = new CategoryId(2),
        //         Name = "Expense",
        //         Description = "Expense category",
        //         Type = CategoryType.Expense
        //     }
        // );

        builder.HasQueryFilter(x => !x.IsDeleted);

        builder.ToTable("Categories");
    }
}
