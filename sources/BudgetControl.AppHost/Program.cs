var builder = DistributedApplication.CreateBuilder(args);

var pwd = builder.AddParameter("Password", true);

var sqlserver = builder.AddSqlServer("SqlServer", pwd, 1433)
    .WithDataVolume("BudgetControlData")
    .AddDatabase("BudgetControl");

builder.AddProject<Projects.BudgetControl_Api>("api")
    .WithReference(sqlserver)
    .WithExternalHttpEndpoints();

builder.Build().Run();
