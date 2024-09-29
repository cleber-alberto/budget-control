using System;

namespace BudgetControl.Application.Categories.Queries;

[ExcludeFromCodeCoverage]
public record FindSubcategoryQuery(Guid Id, Guid CategoryId) : IQuery<SubcategoryResponse>;
