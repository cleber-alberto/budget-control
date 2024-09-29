using BudgetControl.Domain.Categories;
using Microsoft.Extensions.Logging;

namespace BudgetControl.Application.Categories.Queries;

public class FindSubcategoryQueryHandler(
    ICategoryRepository categoryRepository,
    ILogger<FindSubcategoryQueryHandler> logger) : IQueryHandler<FindSubcategoryQuery, SubcategoryResponse>
{
    public async Task<Result<SubcategoryResponse>> Handle(FindSubcategoryQuery request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdWithReferencesAsync(new CategoryId(request.CategoryId), cancellationToken);

        if (category is null)
        {
            logger.LogError("Category {CategoryId} not found", request.CategoryId);
            return Result.Failures<SubcategoryResponse>([Error.NotFound(nameof(Category))]);
        }

        var subcategoryId = new SubcategoryId(request.Id);
        var subcategory = category.Subcategories.FirstOrDefault(x => x.Id.Equals(subcategoryId));
        if (subcategory is null)
        {
            logger.LogError("Subcategory {SubcategoryId} not found", request.Id);
            return Result.Failures<SubcategoryResponse>([Error.NotFound(nameof(Subcategory))]);
        }

        return Result.Success(new SubcategoryResponse(
            subcategory.Id.Value,
            subcategory.Title.Value,
            subcategory.Description.Value
        ));
    }
}
