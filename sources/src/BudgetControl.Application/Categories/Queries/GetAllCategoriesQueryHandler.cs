using System;
using BudgetControl.Domain.Categories;
using Microsoft.Extensions.Logging;

namespace BudgetControl.Application.Categories.Queries;

public class GetAllCategoriesQueryHandler(
    ICategoryRepository categoryRepository,
    ILogger<GetAllCategoriesQueryHandler> logger) : IQueryHandler<GetAllCategoriesQuery, IEnumerable<CategoryResponse>>
{
    public async Task<Result<IEnumerable<CategoryResponse>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = new List<CategoryResponse>();
        if (categories == null)
        {
            logger.LogWarning("No categories were found");
            return Result<IEnumerable<CategoryResponse>>.Failures([Error.NotFound()]);
        }

        await foreach (var category in categoryRepository.GetAllAsync(cancellationToken))
        {
            categories.Add(new CategoryResponse(category.Title.Value, category.Description.Value, category.CategoryType.Name));
        }

        return Result.Success(categories.AsEnumerable());
    }
}
