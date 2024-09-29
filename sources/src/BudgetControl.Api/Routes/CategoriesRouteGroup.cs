using System.ComponentModel.DataAnnotations;
using BudgetControl.Application.Categories.Commands;
using BudgetControl.Application.Categories.Queries;
using BudgetControl.Common.Primitives.Results;
using BudgetControl.Domain.Categories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BudgetControl.Api.Routes;

public static class CategoriesRouteGroup
{

    public static RouteGroupBuilder MapCategoriesRoutes(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetAll()).WithName("Get all categories");
        group.MapGet("/{id:guid}", Find()).WithName("Find category by id");
        group.MapPost("/", Create()).WithName("Create category");
        group.MapPut("/{id:guid}", Update()).WithName("Update category");
        // group.MapPatch("/{id:guid}", (Guid id) => new { Id = 1, Name = "Category 1" }).WithName("Patch category");
        group.MapDelete("/{id:guid}", Delete()).WithName("Delete category");

        return group;
    }

    private static Func<ISender, CancellationToken, Task<IResult>> GetAll()
    {
        return async (ISender sender, CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await sender.Send(new GetAllCategoriesQuery(), cancellationToken);

                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : Results.NotFound();
            }
            catch (Exception ex)
            {
                return Problem(ex);
            }
        };
    }

    private static Func<Guid, ISender, CancellationToken, Task<IResult>> Find()
    {
        return async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await sender.Send(new FindCategoryQuery(new CategoryId(id)), cancellationToken);

                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : Results.NotFound();
            }
            catch (Exception ex)
            {
                return Problem(ex);
            }
        };
    }

    private static Func<CreateCategoryCommand, ISender, CancellationToken, Task<IResult>> Create() =>
        async (CreateCategoryCommand createCategoryCommand, ISender sender, CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await sender.Send(createCategoryCommand, cancellationToken);
                var uri = new Uri($"/categories/{result.Value}", UriKind.Relative);

                return result.IsSuccess
                    ? Results.Created(uri, new { id = result.Value })
                    : Results.BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                return Problem(ex);
            }
        };

    private static Func<Guid, UpdateCategoryCommand, ISender, CancellationToken, Task<IResult>> Update() =>
        async (Guid id, UpdateCategoryCommand updateCategoryCommand, ISender sender, CancellationToken cancellationToken) =>
        {
            try
            {
                updateCategoryCommand.SetId(id);
                var result = await sender.Send(updateCategoryCommand, cancellationToken);

                return result.IsSuccess
                    ? Results.NoContent()
                    : result.Errors.Contains(Error.NotFound())
                        ? Results.NotFound(result.Errors)
                        : Results.BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                return Problem(ex);
            }
        };

    private static Func<Guid, ISender, CancellationToken, Task<IResult>> Delete() =>
        async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await sender.Send(new DeleteCategoryCommand(id), cancellationToken);

                return result.IsSuccess
                    ? Results.NoContent()
                    : result.Errors.Contains(Error.NotFound())
                        ? Results.NotFound(result.Errors)
                        : Results.BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                return Problem(ex);
            }
        };

    private static IResult Problem(Exception ex)
    {
        var error = Error.FromException(ex);
        var message = error.Message;
        var statusCode = StatusCodes.Status500InternalServerError;

        if (ex is ValidationException)
        {
            statusCode = StatusCodes.Status400BadRequest;
            message = "Validation error";
        }

        if (ex is DbUpdateException)
        {
            statusCode = StatusCodes.Status400BadRequest;
            message = "Database error";
        }

        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
        {
            message = ex.Message + Environment.NewLine + ex.StackTrace;
        }

        // Log error
        Console.WriteLine(message);

        return Results.Problem(new ProblemDetails
        {
            Title = error.Message,
            Detail = message,
            Status = statusCode,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
        });
    }
}
