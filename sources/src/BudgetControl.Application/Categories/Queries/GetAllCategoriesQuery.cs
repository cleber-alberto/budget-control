using System;

namespace BudgetControl.Application.Categories.Queries;

public record GetAllCategoriesQuery : IQuery<IEnumerable<CategoryResponse>>;
