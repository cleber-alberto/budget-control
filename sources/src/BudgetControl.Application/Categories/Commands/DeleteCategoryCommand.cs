using System;

namespace BudgetControl.Application.Categories.Commands;

public record DeleteCategoryCommand(Guid Id) : ICommand;
