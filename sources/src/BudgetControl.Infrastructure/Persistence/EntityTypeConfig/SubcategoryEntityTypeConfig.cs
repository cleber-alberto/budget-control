using System;
using BudgetControl.Domain.Categories;
using BudgetControl.Infrastructure.Persistence.Conventers;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BudgetControl.Infrastructure.Persistence.EntityTypeConfig;

public class SubcategoryEntityTypeConfig
    : IEntityTypeConfiguration<Subcategory>
{
    public void Configure(EntityTypeBuilder<Subcategory> builder)
    {
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
            .HasMaxLength(64)
            .IsRequired();
        builder.Property(x => x.Description)
            .HasConversion(ValueObjectConverter.DescriptionConverter)
            .HasMaxLength(1024)
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

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
