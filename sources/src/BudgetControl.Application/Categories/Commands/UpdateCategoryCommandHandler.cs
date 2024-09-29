using BudgetControl.Domain.Categories;
using Microsoft.Extensions.Logging;

namespace BudgetControl.Application.Categories.Commands;

public class UpdateCategoryCommandHandler(
    IUnitOfWork unitOfWork,
    ICategoryRepository categoryRepository,
    ILogger<UpdateCategoryCommandHandler> logger) : ICommandHandler<UpdateCategoryCommand>
{
    public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(new CategoryId(request.Id), cancellationToken);
        if (category == null)
        {
            logger.LogWarning("Category with id {Id} was not found", request.Id);
            return Result.Failures([Error.NotFound(nameof(Category))]);
        }

        var resultCategory = category.Update(
            new CategoryId(request.Id),
            request.Title,
            request.Description,
            request.Type);

        if (resultCategory.IsFailure)
        {
            logger.LogError("Failed to update category: {Errors}", resultCategory.Errors);
            return Result.Failures(resultCategory.Errors);
        }

        categoryRepository.Update(category);

        var resultDatabase = await unitOfWork.CommitAsync(cancellationToken);
        if (resultDatabase.IsFailure)
        {
            logger.LogCritical("Failed to commit changes: {Errors}", resultDatabase.Errors);
            return Result.Failures(resultDatabase.Errors);
        }

        logger.LogInformation("Category {CategoryId} updated", category.Id);
        return Result.Success(true);
    }
}
