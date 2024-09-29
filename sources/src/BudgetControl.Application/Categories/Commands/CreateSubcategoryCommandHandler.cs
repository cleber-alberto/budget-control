using System;
using BudgetControl.Domain.Categories;
using Microsoft.Extensions.Logging;

namespace BudgetControl.Application.Categories.Commands;

public class CreateSubcategoryCommandHandler(
    IUnitOfWork unitOfWork,
    ICategoryRepository categoryRepository,
    ILogger<CreateSubcategoryCommandHandler> logger) : ICommandHandler<CreateSubcategoryCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateSubcategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdWithReferencesAsync(new CategoryId(request.CategoryId), cancellationToken);

        if (category is null)
        {
            logger.LogError("Category {CategoryId} not found", request.CategoryId);
            return Result.Failures<Guid>([Error.NotFound("Category not found")]);
        }

        var subcategoryResult = Subcategory.Create(request.Title, request.Description, category);

        if (subcategoryResult.IsFailure)
        {
            logger.LogError("Failed to create subcategory: {Errors}", subcategoryResult.Errors);
            return Result.Failures<Guid>(subcategoryResult.Errors);
        }

        await categoryRepository.AddSubcategoryAsync(subcategoryResult.Value, cancellationToken);

        var resultDatabase = await unitOfWork.CommitAsync(cancellationToken);
        if (resultDatabase.IsFailure)
        {
            logger.LogCritical("Failed to commit changes: {Errors}", resultDatabase.Errors);
            return Result.Failures<Guid>(resultDatabase.Errors);
        }

        logger.LogInformation("Subcategory {SubcategoryId} created", subcategoryResult.Value.Id);
        return Result.Success(subcategoryResult.Value.Id.Value);
    }
}
