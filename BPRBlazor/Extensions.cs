using BPR.Analysis.Services;
using BPR.Mediator.Interfaces;
using BPR.Mediator.Interfaces.Messaging;
using BPR.Mediator.Services;
using BPR.Mediator.Services.Messaging;
using BPR.Mediator.Validators;
using BPR.Model.Api;
using BPR.Model.Architectures;
using BPR.Persistence.Config;
using BPR.Persistence.Repositories;
using BPR.Persistence.Utils;
using BPRBlazor.Mappers;
using BPRBlazor.Services;
using FluentValidation;
using System.Text.Json;

namespace BPRBlazor;

public static class Extensions
{
    public static void AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<ArchitecturalModel>, ArchitecturalModelValidator>();
        services.AddScoped<IValidator<ArchitecturalComponent>, ArchitecturalComponentValidator>();
        services.AddScoped<IValidator<Rule>, RuleValidator>();
        services.AddScoped<IValidatorService, ValidatorService>();
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IDependencyComponentService, DependencyComponentService>();
        services.AddScoped<ICodebaseService, CodebaseService>();
        services.AddScoped<IResultService, ResultService>();
        services.AddScoped<IDependencyRepository, DependencyRepository>();
        services.AddScoped<IResultRepository, ResultRepository>();
        services.AddScoped<ICodeExtractionService, CodeExtractionService>();
        services.AddScoped<IAnalysisService, AnalysisService>();
        services.AddScoped<IDependencyService, DependencyService>();
        services.AddScoped<IRuleRepository, RuleRepository>();
        services.AddScoped<IRuleService, RuleService>();

        services.AddScoped<ISender, RabbitMqSender>();
        services.AddSingleton<IConsumer<MLAnalysisResponseModel>, RabbitMqConsumer<MLAnalysisResponseModel>>();
        services.AddHostedService<RabbitMqBackgroundService<MLAnalysisResponseModel>>();

        services.AddAutoMapper(typeof(MapperProfile).Assembly);
        services.AddAutoMapper(typeof(ServiceMappers).Assembly);

        services.AddSingleton(new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        });
    }

    public static void AddDbConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DatabaseConfig>(configuration.GetSection(DatabaseConfig.Section));
    }

    public static void CleanUp()
    {
        if (Directory.Exists("../temp"))
        {
            Directory.Delete("../temp", true);
        }
    }
}