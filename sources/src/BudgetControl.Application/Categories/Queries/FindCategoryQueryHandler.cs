using BudgetControl.Domain.Categories;
using Microsoft.Extensions.Logging;

namespace BudgetControl.Application.Categories.Queries;

public class FindCategoryQueryHandler(
    ICategoryRepository categoryRepository,
    ILogger<FindCategoryQuery> logger) : IQueryHandler<FindCategoryQuery, CategoryResponse>
{
    public async Task<Result<CategoryResponse>> Handle(FindCategoryQuery request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category == null)
        {
            logger.LogWarning("Category with id {Id} was not found", request.CategoryId);
            return Result<CategoryResponse>.Failures([Error.NotFound]);
        }

        var categoryDto = new CategoryResponse(
            category.Title.Value,
            category.Description.Value,
            category.Parent?.Id.Value);

        return Result.Success(categoryDto);
    }
}
