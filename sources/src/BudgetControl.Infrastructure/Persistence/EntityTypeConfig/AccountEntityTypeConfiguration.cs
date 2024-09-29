using BudgetControl.Domain.Accounts;
using BudgetControl.Infrastructure.Persistence.Conventers;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BudgetControl.Infrastructure.Persistence.EntityTypeConfig;

public class AccountEntityTypeConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Accounts");

        var idConverter = new ValueConverter<AccountId, Guid>(
            v => v.Value,
            v => new AccountId(v)
        );

        var accountTypeConverter = new ValueConverter<AccountType, string>(
            v => v.Id,
            v => AccountType.FromId<AccountType>(v).Value
        );

        builder.HasKey(a => a.Id);
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
            .HasConversion(accountTypeConverter)
            .HasMaxLength(128)
            .IsRequired();
        builder.Property(x => x.Balance)
            .HasConversion(ValueObjectConverter.MoneyConverter)
            .IsRequired();
        builder.Property(x => x.Limit)
            .HasConversion(ValueObjectConverter.MoneyConverter);

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
