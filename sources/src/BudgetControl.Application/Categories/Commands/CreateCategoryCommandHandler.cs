using BudgetControl.Application.Categories.Commands;
using BudgetControl.Domain.Categories;
using Microsoft.Extensions.Logging;

namespace BudgetControl.Application.Categories.Commands;

public class CreateCategoryCommandHandler(ICategoryRepository categoryRepository, ILogger<CreateCategoryCommandHandler> logger)
    : ICommandHandler<CreateCategoryCommand, CategoryId>
{
    public async Task<Result<CategoryId>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var categoryParent = request.ParentId is not null
            ? await categoryRepository.GetByIdAsync(new CategoryId(request.ParentId.Value), cancellationToken)
            : null;

        var result = Category.Create(
            request.Name,
            request.Description,
            CategoryType.FromDisplayName<CategoryType>(request.CategoryType),
            categoryParent);

        if(result.IsFailure)
        {
            logger.LogError("Failed to create category: {Errors}", result.Errors);
            return Result.Failures<CategoryId>(result.Errors);
        }

        await categoryRepository.AddAsync(result.Value, cancellationToken);
        //await unitOfWork.CommitAsync(cancellationToken);

        logger.LogInformation("Category {CategoryId} created", result.Value.Id);
        return Result.Success(result.Value.Id);
    }
}
