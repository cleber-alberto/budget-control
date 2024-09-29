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
        group.MapGet("/{categoryId:guid}/subcategories", GetAllSubcategories()).WithName("Get all subcategories by category id");
        group.MapGet("/{categoryId:guid}/subcategories/{id:guid}", FindSubcategory()).WithName("Find subcategory by id");
        group.MapPost("/{categoryId:guid}/subcategories", CreateSubcategory()).WithName("Create subcategory");
        group.MapPut("/{categoryId:guid}/subcategories/{id:guid}", UpdateSubcategory()).WithName("Update subcategory");
        group.MapDelete("/{categoryId:guid}/subcategories/{id:guid}", DeleteSubcategory()).WithName("Delete subcategory");

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
                    : Problem("Categories not found", string.Join(", ", result.Errors.Select(e => e.Message)), StatusCodes.Status404NotFound);
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
                    : Problem("Category not found", string.Join(", ", result.Errors.Select(e => e.Message)), StatusCodes.Status404NotFound);
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
                    : Problem("Failed to create category", string.Join(", ", result.Errors.Select(e => e.Message)), StatusCodes.Status400BadRequest);
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
                        ? Problem("Category not found", string.Join(", ", result.Errors.Select(e => e.Message)), StatusCodes.Status404NotFound)
                        : Problem("Failed to update category", string.Join(", ", result.Errors.Select(e => e.Message)), StatusCodes.Status400BadRequest);
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
                        ? Problem("Category not found", string.Join(", ", result.Errors.Select(e => e.Message)), StatusCodes.Status404NotFound)
                        : Problem("Failed to delete category", string.Join(", ", result.Errors.Select(e => e.Message)), StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                return Problem(ex);
            }
        };

    private static Func<Guid, ISender, CancellationToken, Task<IResult>> GetAllSubcategories() =>
        async (Guid categoryId, ISender sender, CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await sender.Send(new GetAllSubcategoriesQuery(categoryId), cancellationToken);

                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : Problem("Subcategories not found", string.Join(", ", result.Errors.Select(e => e.Message)), StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                return Problem(ex);
            }
        };

    private static Func<Guid, Guid, ISender, CancellationToken, Task<IResult>> FindSubcategory() =>
        async (Guid categoryId, Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await sender.Send(new FindSubcategoryQuery(id, categoryId), cancellationToken);

                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : Problem("Subcategory not found", string.Join(", ", result.Errors.Select(e => e.Message)), StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                return Problem(ex);
            }
        };

    private static Func<Guid, CreateSubcategoryCommand, ISender, CancellationToken, Task<IResult>> CreateSubcategory() =>
        async (Guid categoryId, CreateSubcategoryCommand createSubcategoryCommand, ISender sender, CancellationToken cancellationToken) =>
        {
            try
            {
                createSubcategoryCommand.SetCategoryId(categoryId);
                var result = await sender.Send(createSubcategoryCommand, cancellationToken);
                var uri = new Uri($"/categories/{categoryId}/subcategories/{result.Value}", UriKind.Relative);

                return result.IsSuccess
                    ? Results.Created(uri, new { id = result.Value })
                    : Problem("Failed to create subcategory", string.Join(", ", result.Errors.Select(e => e.Message)), StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                return Problem(ex);
            }
        };

    private static Func<Guid, Guid, UpdateSubcategoryCommand, ISender, CancellationToken, Task<IResult>> UpdateSubcategory() =>
        async (Guid categoryId, Guid id, UpdateSubcategoryCommand updateSubcategoryCommand, ISender sender, CancellationToken cancellationToken) =>
        {
            try
            {
                updateSubcategoryCommand.SetId(id);
                updateSubcategoryCommand.SetCategoryId(categoryId);
                var result = await sender.Send(updateSubcategoryCommand, cancellationToken);

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

    private static Func<Guid, Guid, ISender, CancellationToken, Task<IResult>> DeleteSubcategory() =>
        async (Guid categoryId, Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            try
            {
                var deleteSubcategoryCommand = new DeleteSubcategoryCommand(id);
                deleteSubcategoryCommand.SetCategoryId(categoryId);
                var result = await sender.Send(deleteSubcategoryCommand, cancellationToken);

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

    private static IResult Problem(string message, string detail, int statusCode)
    {
        return Results.Problem(new ProblemDetails()
        {
            Title = message,
            Detail = detail,
            Status = statusCode,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
        });
    }

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
