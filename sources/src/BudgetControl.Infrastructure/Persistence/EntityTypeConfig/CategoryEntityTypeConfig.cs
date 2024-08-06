using BudgetControl.Domain.Categories;

namespace BudgetControl.Infrastructure.Persistence.EntityTypeConfig;

internal sealed class CategoryEntityTypeConfig : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(500);
        builder.Property(x => x.Type).IsRequired();
        builder.HasOne<Category>().WithMany().HasForeignKey(x => x.Parent!.Id);

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

        builder.ToTable("Categories");
    }
}
