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
            return Result.Failures([Error.NotFound()]);
        }

        var resultCategory = category.Update(
            new CategoryId(request.Id),
            request.Title,
            request.Description,
            CategoryType.FromDisplayName<CategoryType>(request.CategoryType));

        if (resultCategory.IsFailure)
        {
            logger.LogError("Failed to update category: {Errors}", resultCategory.Errors);
            return Result.Failures(resultCategory.Errors);
        }

        var resultSubcategory = UpdateSubcategories(category, request);

        if(resultSubcategory.IsFailure)
        {
            return Result.Failures(resultSubcategory.Errors);
        }

        categoryRepository.Update(category);
        await unitOfWork.CommitAsync(cancellationToken);

        logger.LogInformation("Category {CategoryId} updated", category.Id);
        return Result.Success(true);
    }

    private Result UpdateSubcategories(Category category, UpdateCategoryCommand request)
    {
        foreach (var subcategory in request.Subcategories)
        {
            Result<Subcategory> subcategoryResult = null!;
            var subcategoryFound = category.Subcategories.FirstOrDefault(x => x.Title.Value.Equals(subcategory.Title, StringComparison.OrdinalIgnoreCase));

            if (subcategoryFound == null)
            {
                subcategoryResult = category.CreateSubcategory(
                    subcategory.Title,
                    subcategory.Description);
            }
            else
            {
                subcategoryResult = category.UpdateSubcategory(
                    new SubcategoryId(subcategory.Id),
                    subcategory.Title,
                    subcategory.Description);
            }

            if (subcategoryResult.IsFailure)
            {
                logger.LogError("Failed to update subcategory: {Errors}", subcategoryResult.Errors);
                return Result.Failures(subcategoryResult.Errors);
            }
        }

        return Result.Success(true);
    }
}
