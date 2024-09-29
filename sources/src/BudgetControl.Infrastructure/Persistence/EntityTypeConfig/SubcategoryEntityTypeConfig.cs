using BudgetControl.Domain.Categories;
using BudgetControl.Infrastructure.Persistence.Conventers;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BudgetControl.Infrastructure.Persistence.EntityTypeConfig;

public class SubcategoryEntityTypeConfig : IEntityTypeConfiguration<Subcategory>
{
    public void Configure(EntityTypeBuilder<Subcategory> builder)
    {
        builder.ToTable("Subcategories");

        var idConverter = new ValueConverter<SubcategoryId, Guid>(
            v => v.Value,
            v => new SubcategoryId(v)
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
        builder.Property(x => x.IsDeleted)
            .IsRequired();

        // builder.HasOne(x => x.Category)
        //     .WithMany()
        //     .HasForeignKey("CategoryId")
        //     .IsRequired()
        //     .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Category)
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
