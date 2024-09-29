using BudgetControl.Domain.Enumerations;
using BudgetControl.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BudgetControl.Infrastructure.Persistence.Conventers;

public static class ValueObjectConverter
{
    public static ValueConverter<Title, string> TitleConverter => new(
        v => v.Value,
        v => Title.Create(v).Value
    );

    public static ValueConverter<Description, string> DescriptionConverter => new(
        v => v.Value,
        v => Description.Create(v).Value
    );

    public static ValueConverter<Money, decimal> MoneyConverter => new(
        v => v.Value,
        v => Money.Create(v, Currency.CAD).Value
    );
}
