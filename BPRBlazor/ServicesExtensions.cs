using BPR.Models.Persistence;
using BPRBE.Config;
using BPRBE.Models.Persistence;
using BPRBE.Persistence;
using BPRBE.Services;
using BPRBE.Validators;
using BPRBlazor.Services;
using FluentValidation;

namespace BPRBlazor;

public static class ServicesExtensions
{
    public static void AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<MongoArchitecturalModel>, ArchitecturalModelValidator>();
        services.AddScoped<IValidator<MongoArchitecturalComponent>, ArchitecturalComponentValidator>();
        services.AddScoped<IValidatorService, ValidatorService>();
    }

    public static void AddServices(this IServiceCollection services) {
        services.AddScoped<IHttpService, HttpService>();
        services.AddScoped<IDependencyComponentService, DependencyComponentService>();
        services.AddScoped<ICodebaseService, CodebaseService>();
        services.AddScoped<IDependencyRepository, DependencyRepository>();
        services.AddScoped<ICodeExtractionService, CodeExtractionService>();
        services.AddScoped<IAnalysisService, AnalysisService>();
        services.AddScoped<IDependencyService, DependencyService>();
    }

    public static void AddDbConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DatabaseConfig>(configuration.GetSection(DatabaseConfig.Section));
    }
}