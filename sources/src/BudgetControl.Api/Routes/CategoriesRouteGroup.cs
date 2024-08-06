using System.ComponentModel.DataAnnotations;
using BudgetControl.Application.Categories.Commands;
using BudgetControl.Common.Primitives.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;

namespace BudgetControl.Api.Routes;

public static class CategoriesRouteGroup
{
    public static RouteGroupBuilder MapCategoriesRoutes(this RouteGroupBuilder group)
    {
        // group.MapGet("/", async context =>
        // {
        //     await context.Response.WriteAsJsonAsync(new[]
        //     {
        //         new { Id = 1, Name = "Category 1" },
        //         new { Id = 2, Name = "Category 2" },
        //         new { Id = 3, Name = "Category 3" }
        //     });
        // }).WithName("GetCategories");

        group.MapGet("/", () => new { Id = 1, Name = "Category 1" }).WithName("Get all categories");
        group.MapGet("/{id:guid}", (Guid id) => new { Id = 1, Name = "Category 1" }).WithName("Get category by id");
        group.MapPost("/", Create()).WithName("Create category");
        group.MapPut("/{id:guid}", (Guid id) => new { Id = 1, Name = "Category 1" }).WithName("Update category");
        group.MapPatch("/{id:guid}", (Guid id) => new { Id = 1, Name = "Category 1" }).WithName("Patch category");
        group.MapDelete("/{id:guid}", (Guid id) => new { Id = 1, Name = "Category 1" }).WithName("Delete category");

        return group;
    }

    private static Func<CreateCategoryCommand, ISender, CancellationToken, Task<IResult>> Create() =>
        async (CreateCategoryCommand createProductCommand, ISender sender, CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await sender.Send(createProductCommand, cancellationToken);

                return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                return Problem(ex);
            }
        };


    private static IResult Problem(Exception ex)
    {
        var error = Error.FromException(ex);
        var statusCode = StatusCodes.Status500InternalServerError;

        // Get the http status code from exception
        if (ex is ValidationException)
        {
            statusCode = StatusCodes.Status400BadRequest;
        }

        return Results.Problem(new ProblemDetails
        {
            Title = error.Message,
            Detail = ex.Message,
            Status = statusCode,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
        });
    }
}
