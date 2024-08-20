using System;
using BudgetControl.Domain.Categories;
using Microsoft.Extensions.Logging;

namespace BudgetControl.Application.Categories.Commands;

public class DeleteCategoryCommandHandler(
    IUnitOfWork unitOfWork,
    ICategoryRepository categoryRepository,
    ILogger<DeleteCategoryCommand> logger
    ) : ICommandHandler<DeleteCategoryCommand>
{
    public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(new CategoryId(request.Id), cancellationToken);
        if (category == null)
        {
            logger.LogWarning("Category with id {Id} was not found", request.Id);
            return Result.Failures([Error.NotFound()]);
        }

        category.Delete();
        categoryRepository.Update(category);
        await unitOfWork.CommitAsync(cancellationToken);

        logger.LogInformation("Category {CategoryId} deleted", category.Id);

        return Result.Success(true);
    }
}
