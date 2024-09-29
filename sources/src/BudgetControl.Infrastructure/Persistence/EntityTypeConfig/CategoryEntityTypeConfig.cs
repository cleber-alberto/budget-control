using BudgetControl.Domain.Categories;
using BudgetControl.Infrastructure.Persistence.Conventers;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BudgetControl.Infrastructure.Persistence.EntityTypeConfig;

internal sealed class CategoryEntityTypeConfig : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        var idConverter = new ValueConverter<CategoryId, Guid>(
            v => v.Value,
            v => new CategoryId(v)
        );

        var categoryTypeConverter = new ValueConverter<CategoryType, string>(
            v => v.Id,
            v => CategoryType.FromId<CategoryType>(v).Value
        );

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(idConverter)
            .ValueGeneratedOnAdd();
        builder.Property(x => x.Title)
            .HasConversion(ValueObjectConverter.TitleConverter)
            .HasMaxLength(1024)
            .IsRequired();
        builder.Property(x => x.Description)
            .HasConversion(ValueObjectConverter.DescriptionConverter)
            .IsRequired();
        builder.Property(x => x.Type)
            .HasConversion(categoryTypeConverter)
            .HasMaxLength(128)
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

        builder.Navigation(x => x.Subcategories)
            .UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.Property(x => x.Created)
            .ValueGeneratedOnAdd()
            .HasDefaultValue(DateTimeOffset.UtcNow)
            .IsRequired();

        builder.Property(x => x.LastUpdated)
            .ValueGeneratedOnUpdateSometimes()
            .HasDefaultValue(DateTimeOffset.UtcNow);

        builder.Property(x => x.IsDeleted)
            .IsRequired();

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
