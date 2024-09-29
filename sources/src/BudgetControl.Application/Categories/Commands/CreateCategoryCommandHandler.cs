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
            request.Type);

        if (categoryResult.IsFailure)
        {
            logger.LogError("Failed to create category: {Errors}", categoryResult.Errors);
            return Result.Failures<Guid>(categoryResult.Errors);
        }

        await categoryRepository.AddAsync(categoryResult.Value, cancellationToken);

        var resultDatabase = await unitOfWork.CommitAsync(cancellationToken);
        if (resultDatabase.IsFailure)
        {
            logger.LogCritical("Failed to commit changes: {Errors}", resultDatabase.Errors);
            return Result.Failures<Guid>(resultDatabase.Errors);
        }

        logger.LogInformation("Category {CategoryId} created", categoryResult.Value.Id);
        return Result.Success(categoryResult.Value.Id.Value);
    }
}
