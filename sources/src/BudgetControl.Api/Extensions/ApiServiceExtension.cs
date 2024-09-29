using Asp.Versioning;
using BudgetControl.Api.Routes;
using BudgetControl.Application.Extensions;
using BudgetControl.Infrastructure.Extensions;
using BudgetControl.Infrastructure.Persistence;

namespace BudgetControl.Api.Extensions;

public static class ApiServiceExtension
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {

        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services
            .AddApplicationServices()
            .AddInfrastructureServices(configuration);

        services.AddApiVersioning(options => {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("X-Api-Version"),
                new QueryStringApiVersionReader("api-version"));
        }).AddApiExplorer(options => {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;}
        );


        return services;
    }

    public static WebApplication UseApi(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();

            using(var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<BudgetControlDbContext>();
                //context.Database.EnsureCreated();
            }
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        // Enable middleware to serve generated Swagger as a JSON endpoint.
        app.UseSwagger();
        // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
        // specifying the Swagger JSON endpoint.
        app.UseSwaggerUI(options => {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "BudgetControl API V1");
            options.RoutePrefix = string.Empty;
        });

        // Configure the HTTP request pipeline.
        if (app.Services.GetRequiredService<IWebHostEnvironment>().IsDevelopment()) // Update this line
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        var apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .ReportApiVersions()
            .Build();

        app.MapGet("/test", () => "Hello World!");

        app.MapGroup("/categories")
            .MapCategoriesRoutes()
            .WithApiVersionSet(apiVersionSet)
            .WithTags("Categories")
            .WithOpenApi();

        return app;
    }
}
