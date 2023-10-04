﻿using BPRBE.Config;
using BPRBE.Persistence;
using BPRBE.Services;
using BPRBE.Validators;
using BPRBlazor.Services;
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

    public static void AddServices(this IServiceCollection services) {
        services.AddScoped<IHttpService, HttpService>();
        services.AddScoped<IDependencyComponentService, DependencyComponentService>();
        services.AddScoped<ICodebaseService, CodebaseService>();
        services.AddScoped<IDependencyRepository, DependencyRepository>();
        services.AddScoped<IDependencyService, DependencyService>();
    }

    public static void AddDbConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DatabaseConfig>(configuration.GetSection(DatabaseConfig.Section));
    }
}