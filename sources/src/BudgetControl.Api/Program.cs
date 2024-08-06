
using System.Diagnostics.CodeAnalysis;
using BudgetControl.Api.Extensions;
using BudgetControl.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.AddSqlServerDbContext<BudgetControlDbContext>("BudgetControlConnection", static settings => {
        settings.DisableHealthChecks = true;
        settings.DisableRetry = true;
        settings.DisableTracing = true;
    });

builder.Services.AddApiServices(configuration);

var app = builder.Build();
app.UseApi();
app.Run();

[ExcludeFromCodeCoverage]
public partial class Program;
