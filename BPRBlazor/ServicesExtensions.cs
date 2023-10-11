using BPR.Analysis.Services;
using BPRBE.Config;
using BPRBE.Models.Persistence;
using BPRBE.Persistence;
using BPRBE.Services;
using BPRBE.Validators;
using BPRBlazor.Mappers;
using BPRBlazor.Services;
using BPRBlazor.State;
using FluentValidation;

namespace BPRBlazor;

public static class ServicesExtensions
{
    public static void AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<ArchitecturalModel>, ArchitecturalModelValidator>();
        services.AddScoped<IValidator<ArchitecturalComponent>, ArchitecturalComponentValidator>();
        services.AddScoped<IValidator<Rule>, RuleValidator>();
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
        services.AddScoped<IRuleRepository, RuleRepository>();
        services.AddScoped<IRuleService, RuleService>();
        services.AddAutoMapper(typeof(MapperProfile).Assembly);
    }

    public static void AddDbConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DatabaseConfig>(configuration.GetSection(DatabaseConfig.Section));
    }

    public static void AddStateMachine(this IServiceCollection services) {
        services.AddScoped<StateContainer>();
    }
}