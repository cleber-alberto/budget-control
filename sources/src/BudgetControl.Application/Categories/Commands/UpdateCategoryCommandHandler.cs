using System;
using BudgetControl.Domain.Categories;
using Microsoft.Extensions.Logging;

namespace BudgetControl.Application.Categories.Commands;

public class UpdateCategoryCommandHandler(
    IUnitOfWork unitOfWork,
    ICategoryRepository categoryRepository,
    ILogger<UpdateCategoryCommand> logger) : ICommandHandler<UpdateCategoryCommand>
{
    public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(new CategoryId(request.Id), cancellationToken);
        if (category == null)
        {
            logger.LogWarning("Category with id {Id} was not found", request.Id);
            return Result.Failures([Error.NotFound]);
        }

        Category? categoryParent = null;
        if (request.ParentId is not null)
        {
            categoryParent = await categoryRepository.GetByIdAsync(new CategoryId(request.ParentId.Value), cancellationToken);
            if (categoryParent == null)
            {
                logger.LogWarning("Parent category with id {Id} was not found", request.ParentId);
                return Result.Failures([Error.NotFound]);
            }
        }

        var result = category.Update(
            request.Title,
            request.Description,
            CategoryType.FromDisplayName<CategoryType>(request.CategoryType),
            categoryParent);

        if (result.IsFailure)
        {
            logger.LogError("Failed to update category: {Errors}", result.Errors);
            return Result.Failures(result.Errors);
        }

        categoryRepository.Update(category);
        await unitOfWork.CommitAsync(cancellationToken);

        logger.LogInformation("Category {CategoryId} updated", category.Id);
        return Result.Success(true);
    }
}
