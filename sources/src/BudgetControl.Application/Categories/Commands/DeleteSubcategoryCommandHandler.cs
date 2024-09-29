using BudgetControl.Domain.Categories;
using Microsoft.Extensions.Logging;

namespace BudgetControl.Application.Categories.Commands;

public class DeleteSubcategoryCommandHandler(
    IUnitOfWork unitOfWork,
    ICategoryRepository categoryRepository,
    ILogger<DeleteSubcategoryCommandHandler> logger) : ICommandHandler<DeleteSubcategoryCommand>
{
    public async Task<Result> Handle(DeleteSubcategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdWithReferencesAsync(new CategoryId(request.CategoryId), cancellationToken);

        if (category is null)
        {
            logger.LogError("Category {CategoryId} not found", request.CategoryId);
            return Result.Failures([Error.NotFound("Category not found")]);
        }

        var subcategoryId = new SubcategoryId(request.Id);
        var subcategory = category.Subcategories.FirstOrDefault(x => x.Id.Equals(subcategoryId));

        if (subcategory is null)
        {
            logger.LogError("Subcategory {SubcategoryId} not found", request.Id);
            return Result.Failures([Error.NotFound("Subcategory not found")]);
        }

        categoryRepository.DeleteSubcategory(subcategory);

        var resultDatabase = await unitOfWork.CommitAsync(cancellationToken);
        if (resultDatabase.IsFailure)
        {
            logger.LogCritical("Failed to commit changes: {Errors}", resultDatabase.Errors);
            return Result.Failures(resultDatabase.Errors);
        }

        logger.LogInformation("Subcategory {SubcategoryId} deleted", subcategory.Id);
        return Result.Success(true);
    }
}
