using BPR.Analysis.Services;
using BPR.Analysis.Services.Analyses;
using BPR.MachineLearningIntegration.Mappers;
using BPR.MachineLearningIntegration.RabbitMq;
using BPR.Mediator.Interfaces;
using BPR.Mediator.Interfaces.Messaging;
using BPR.Mediator.Services;
using BPR.Mediator.Validators;
using BPR.Model.Architectures;
using BPR.Persistence.Config;
using BPR.Persistence.Repositories;
using BPR.Persistence.Utils;
using BPRBlazor.Mappers;
using BPRBlazor.Services;
using FluentValidation;
using System.Text.Json;
using BPR.Model.Rules;

namespace BPRBlazor;

public static class Extensions
{
    public static void AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<ArchitectureModel>, ArchitectureModelValidator>();
        services.AddScoped<IValidator<ArchitectureComponent>, ArchitectureComponentValidator>();
        services.AddScoped<IValidator<Rule>, RuleValidator>();
        services.AddScoped<IValidatorService, ValidatorService>();
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IDependencyComponentService, DependencyComponentService>();
        services.AddScoped<ICodebaseService, CodebaseService>();
        services.AddScoped<IResultService, ResultService>();
        services.AddScoped<IArchitectureModelRepository, ArchitectureModelRepository>();
        services.AddScoped<IResultRepository, ResultRepository>();
        services.AddScoped<ICodeExtractionService, CodeExtractionService>();
        services.AddScoped<IAnalysisService, AnalysisService>();
        services.AddScoped<NamespaceAnalysis>();
        services.AddScoped<DependencyAnalysis>();
        services.AddScoped<ExtendedAnalysisResultsHandler>();
        services.AddScoped<IArchitectureModelService, ArchitectureModelService>();
        services.AddScoped<IRuleRepository, RuleRepository>();
        services.AddScoped<IRuleService, RuleService>();

        services.AddScoped<ISender, RabbitMqSender>();
        services.AddSingleton<IConsumer, RabbitMqConsumer>();

        services.AddAutoMapper(typeof(MachineLearningMapper).Assembly);
        services.AddAutoMapper(typeof(MapperProfile).Assembly);
        services.AddAutoMapper(typeof(ServiceMappers).Assembly);

        services.AddSingleton(new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        });
    }

    public static void AddBlazorServices(this IServiceCollection services)
    {
        services.AddScoped<ToastService>();
        services.AddHostedService<RabbitMqBackgroundService>();
    }

    public static void AddDbConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DatabaseConfig>(configuration.GetSection(DatabaseConfig.Section));
    }
}