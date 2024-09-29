using System.Net;

namespace BudgetControl.Common.Primitives.Results;

public sealed record Error(string Code, string Message)
{
    public static Error FromHttpStatusCode(HttpStatusCode statusCode) => statusCode switch
    {
        HttpStatusCode.NotFound => NotFound(),
        HttpStatusCode.Unauthorized => Unauthorized,
        HttpStatusCode.Forbidden => Forbidden,
        HttpStatusCode.BadRequest => BadRequest,
        _ => Unknown
    };

    public static Error FromException(Exception exception) => exception switch
    {
        ArgumentException => InvalidArgument,
        InvalidOperationException => InvalidOperation,
        InvalidCastException => InvalidData,
        FormatException => InvalidFormat(),
        _ => Unknown
    };

    public static Error Combine(params Error[] errors) => new("Combined", string.Join(", ", errors.Select(e => e.Message)));

    public static Error None => new(string.Empty, string.Empty);
    public static Error Unknown => new("Unknown", "An unknown error occurred");
    public static Error NotFound() => new("NotFound", "The requested resource was not found");
    public static Error NotFound(string name) => new($"NotFound.{name}", $"The {name} was not found");
    public static Error Unauthorized => new("Unauthorized", "You are not authorized to perform this action");
    public static Error Forbidden => new("Forbidden", "You are not allowed to perform this action");
    public static Error BadRequest => new("BadRequest", "The request is invalid");
    public static Error InvalidOperation => new("InvalidOperation", "The operation is invalid");
    public static Error InvalidState => new("InvalidState", "The state is invalid");
    public static Error InvalidArgument => new("InvalidArgument", "The argument is invalid");
    public static Error InvalidFormat() => new("InvalidFormat", "The format is invalid");
    public static Error InvalidFormat(string name) => new($"InvalidFormat.{name}", $"The {name} has an invalid format");
    public static Error InvalidData => new("InvalidData", "The data is invalid");
    public static Error InvalidRequest => new("InvalidRequest", "The request is invalid");
    public static Error InvalidResponse => new("InvalidResponse", "The response is invalid");
    public static Error InvalidToken => new("InvalidToken", "The token is invalid");
    public static Error InvalidCredentials => new("InvalidCredentials", "The credentials are invalid");
    public static Error InvalidSignature => new("InvalidSignature", "The signature is invalid");
    public static Error LimitExceeded => new("LimitExceeded", "The limit has been exceeded");
    public static Error Required(string name) => new($"Required.{name}", $"The {name} is required");
    public static Error NullOrEmpty(string name) => new($"NullOrEmpty.{name}", $"The {name} should not be null or empty");
    public static Error NullOrWhiteSpace(string name) => new($"NullOrWhiteSpace.{name}", $"The {name} should not be null or white space");
    public static Error TooLong(string name, int maxLength) => new($"TooLong.{name}", $"The {name} is too long. Max length is {maxLength}");
    public static Error Duplicate(string property) => new($"Duplicate.{property}", $"The {property} is already in use");
    public static Error LessOrEqual(string property, decimal value) => new($"LessOrEqual.{property}", $"The {property} should be less or equal to {value}");
    public static Error MoreThan(string property, decimal value) => new($"MoreThan.{property}", $"The {property} should be more than {value}");
    internal static Error InvalidEnumerationValue<TValue>(string description, TValue value, string type) => new($"InvalidEnumerationValue.{description}", $"'{value}' is not a valid {description} for {type}");
    public static Error AlreadyExists(string property, string value) => new($"AlreadyExists.{property}", $"The {property} '{value}' already exists");
    public static Error DatabaseError(string message) => new("DatabaseError", message);

    internal static IEnumerable<Error> InvalidEnumerationName(string name, string enumeration)
    {
        yield return new($"InvalidEnumerationName.{name}", $"'{name}' is not a valid {enumeration}");
    }

    internal static IEnumerable<Error> InvalidEnumerationId<TId>(TId? id, string enumeration)
    {
        yield return new($"InvalidEnumerationId.{id}", $"'{id}' is not a valid {enumeration}");
    }
}
