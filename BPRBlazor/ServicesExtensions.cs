using BPRBE.Config;
using BPRBE.Validators;
using FluentValidation;
using ArchitecturalComponent = BPRBE.Models.Persistence.ArchitecturalComponent;
using ArchitecturalModel = BPRBE.Models.Persistence.ArchitecturalModel;

namespace BPRBlazor;

public static class ServicesExtensions
{
    public static void AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<ArchitecturalModel>, ArchitecturalModelValidator>();
        services.AddScoped<IValidator<ArchitecturalComponent>, ArchitecturalComponentValidator>();
        services.AddScoped<IValidatorService, ValidatorService>();
    }

    public static void AddDbConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DatabaseConfig>(configuration.GetSection(DatabaseConfig.Section));
    }
}