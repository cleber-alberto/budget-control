var builder = DistributedApplication.CreateBuilder(args);

//var pwd = builder.AddParameter("Password", true);

var sqlserver = builder.AddSqlServer("SqlServer", port: 1433)
    .PublishAsContainer()
    .WithBindMount("../../../databases/data/sqlserver", "/var/opt/mssql/data")
    .AddDatabase("BudgetControl");

builder.AddProject<Projects.BudgetControl_Api>("api")
    .WithExternalHttpEndpoints()
    .WithReference(sqlserver);

builder.Build().Run();
