using System;
using BudgetControl.Domain.Categories;
using Microsoft.Extensions.Logging;

namespace BudgetControl.Application.Categories.Queries;

public class GetAllSubcategoriesQueryHandler(
    ICategoryRepository categoryRepository,
    ILogger<GetAllSubcategoriesQueryHandler> logger) : IQueryHandler<GetAllSubcategoriesQuery, IEnumerable<SubcategoryResponse>>
{
    public async Task<Result<IEnumerable<SubcategoryResponse>>> Handle(GetAllSubcategoriesQuery request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdWithReferencesAsync(new CategoryId(request.CategoryId), cancellationToken);

        if (category is null)
        {
            logger.LogError("Category {CategoryId} not found", request.CategoryId);
            return Result.Failures<IEnumerable<SubcategoryResponse>>([Error.NotFound(nameof(Category))]);
        }

        var subcategories = category.Subcategories.Select(x => new SubcategoryResponse(
            x.Id.Value,
            x.Title.Value,
            x.Description.Value
        ));

        return Result.Success(subcategories);
    }
}
