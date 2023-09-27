using BPRBE.Config;
using BPRBE.Models.Persistence;
using BPRBE.Validators;
using FluentValidation;

namespace BPRBE;

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