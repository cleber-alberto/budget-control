using BudgetControl.Domain.Categories;
using Microsoft.Extensions.Logging;

namespace BudgetControl.Application.Categories.Commands;

public class CreateCategoryCommandHandler(
    IUnitOfWork unitOfWork,
    ICategoryRepository categoryRepository,
    ILogger<CreateCategoryCommandHandler> logger) : ICommandHandler<CreateCategoryCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var categoryResult = Category.Create(
            request.Title,
            request.Description,
            CategoryType.FromDisplayName<CategoryType>(request.CategoryType));

        if (categoryResult.IsFailure)
        {
            logger.LogError("Failed to create category: {Errors}", categoryResult.Errors);
            return Result.Failures<Guid>(categoryResult.Errors);
        }

        if (request.Subcategories is not null && request.Subcategories.Any())
        {
            var resultSubcategory = CreateSubcategories(categoryResult.Value, request);
            if (resultSubcategory.IsFailure)
            {
                return Result.Failures<Guid>(resultSubcategory.Errors);
            }
        }

        await categoryRepository.AddAsync(categoryResult.Value, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        logger.LogInformation("Category {CategoryId} created", categoryResult.Value.Id);
        return Result.Success(categoryResult.Value.Id.Value);
    }

    private Result CreateSubcategories(Category category, CreateCategoryCommand request)
    {
        Result subcategoryResult = null!;

        foreach (var subcategory in request.Subcategories)
        {
            subcategoryResult = category.CreateSubcategory(subcategory.Title, subcategory.Description);

            if (subcategoryResult.IsFailure)
            {
                logger.LogError("Failed to create subcategory: {Errors}", subcategoryResult.Errors);
                return Result.Failures(subcategoryResult.Errors);
            }
        }

        return subcategoryResult.IsFailure
            ? Result.Failures(subcategoryResult.Errors)
            : Result.Success(true);
    }
}
