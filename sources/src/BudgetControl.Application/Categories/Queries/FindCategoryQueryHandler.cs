using BudgetControl.Domain.Categories;
using Microsoft.Extensions.Logging;

namespace BudgetControl.Application.Categories.Queries;

public class FindCategoryQueryHandler(
    ICategoryRepository categoryRepository,
    ILogger<FindCategoryQuery> logger) : IQueryHandler<FindCategoryQuery, CategoryResponse>
{
    public async Task<Result<CategoryResponse>> Handle(FindCategoryQuery request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdWithReferencesAsync(request.CategoryId, cancellationToken);
        if (category == null)
        {
            logger.LogWarning("Category with id {Id} was not found", request.CategoryId);
            return Result<CategoryResponse>.Failures([Error.NotFound(nameof(Category))]);
        }

        var categoryDto = new CategoryResponse(
            category.Id.Value,
            category.Title.Value,
            category.Description.Value,
            category.Type.Name,
            category.Subcategories.Select(x => new SubcategoryResponse(x.Id.Value, x.Title.Value, x.Description.Value)).ToList());

        return Result.Success(categoryDto);
    }
}
