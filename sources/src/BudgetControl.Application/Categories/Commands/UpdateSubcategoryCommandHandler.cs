using BudgetControl.Domain.Categories;
using Microsoft.Extensions.Logging;

namespace BudgetControl.Application.Categories.Commands;

public class UpdateSubcategoryCommandHandler(
    IUnitOfWork unitOfWork,
    ICategoryRepository categoryRepository,
    ILogger<UpdateSubcategoryCommandHandler> logger) : ICommandHandler<UpdateSubcategoryCommand>
{
    public async Task<Result> Handle(UpdateSubcategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdWithReferencesAsync(new CategoryId(request.CategoryId), cancellationToken);

        if (category is null)
        {
            logger.LogError("Category {CategoryId} not found", request.CategoryId);
            return Result.Failures([Error.NotFound(nameof(Category))]);
        }

        var subcategoryId = new SubcategoryId(request.Id);
        var subcategory = category.Subcategories.FirstOrDefault(x => x.Id.Equals(subcategoryId));
        if (subcategory is null)
        {
            logger.LogError("Subcategory {SubcategoryId} not found", request.Id);
            return Result.Failures([Error.NotFound(nameof(Subcategory))]);
        }

        var resultSubcategory = subcategory.Update(subcategoryId, request.Title, request.Description);
        if (resultSubcategory.IsFailure)
        {
            logger.LogError("Failed to update subcategory: {Errors}", resultSubcategory.Errors);
            return Result.Failures(resultSubcategory.Errors);
        }

        categoryRepository.UpdateSubcategory(subcategory);

        var resultDatabase = await unitOfWork.CommitAsync(cancellationToken);
        if (resultDatabase.IsFailure)
        {
            logger.LogCritical("Failed to commit changes: {Errors}", resultDatabase.Errors);
            return Result.Failures(resultDatabase.Errors);
        }

        logger.LogInformation("Subcategory {SubcategoryId} updated", subcategory.Id);
        return Result.Success(subcategory.Id.Value);
    }
}
