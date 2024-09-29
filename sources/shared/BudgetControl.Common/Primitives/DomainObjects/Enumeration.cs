using System.Reflection;

namespace BudgetControl.Common.Primitives.DomainObjects;

public abstract class Enumeration<TId>(TId id, string name) : ValueObject
{
    public TId Id { get; } = id;
    public string Name { get; } = name;

    public override string ToString() => Name;

    public static IEnumerable<T> GetAll<T>() where T : Enumeration<TId>
    {
        var fields = typeof(T).GetFields(BindingFlags.Public |
                                         BindingFlags.Static |
                                         BindingFlags.DeclaredOnly);

        return fields.Select(f => f.GetValue(null)).Cast<T>();
    }

    public static Result<T> FromId<T>(TId id) where T : Enumeration<TId>
    {
        var matchingItem = Parse<T, TId>(id, "id", item => item.Id!.Equals(id));

        if (matchingItem.IsFailure)
        {
            return Result.Failures<T>(matchingItem.Errors);
        }

        return matchingItem.IsSuccess
            ? Result.Success(GetAll<T>().FirstOrDefault(item => item.Id!.Equals(id))!)
            : Result.Failures<T>(Error.InvalidEnumerationId(id, typeof(T).Name));
    }

    public static Result<T> FromDisplayName<T>(string name) where T : Enumeration<TId>
    {
        var matchingItem = Parse<T, string>(name, "name", item => item.Name == name);

        if (matchingItem.IsFailure)
        {
            return Result.Failures<T>(matchingItem.Errors);
        }

        return matchingItem.IsSuccess
            ? Result.Success(GetAll<T>().FirstOrDefault(item => item.Name == name)!)
            : Result.Failures<T>(Error.InvalidEnumerationName(name, typeof(T).Name));
    }

    private static Result Parse<T, K>(K value, string description, Func<T, bool> predicate) where T : Enumeration<TId>
    {
        var erros = new List<Error>();

        var matchingItem = GetAll<T>().FirstOrDefault(predicate);

        if (matchingItem == null)
        {
            erros.Add(Error.InvalidEnumerationValue(description, value, typeof(T).Name));
        }

        return erros.Count != 0
            ? Result.Failures<T>(erros)
            : Result.Success(true);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Id ?? throw new InvalidOperationException("Id cannot be null");
        yield return Name;
    }
}
