using BPR.Mediator.Interfaces;
using BPR.Mediator.Utils;
using BPR.Mediator.Validators;
using BPR.Model.Architectures;
using Microsoft.Extensions.Logging;

namespace BPR.Mediator.Services;

public class DependencyService : IDependencyService
{
    private readonly IDependencyRepository _dependencyRepository;
    private readonly IValidatorService _validatorService;
    private readonly ILogger<DependencyService> _logger;

    public DependencyService(IDependencyRepository dependencyRepository, IValidatorService validatorService,
        ILogger<DependencyService> logger)
    {
        _dependencyRepository = dependencyRepository;
        _validatorService = validatorService;
        _logger = logger;
    }

    public async Task<IList<ArchitecturalModel>> GetArchitecturalModelsAsync()
    {
        return await _dependencyRepository.GetArchitecturalModelsAsync();
    }

    public async Task<Result> AddOrEditModelAsync(ArchitecturalModel model)
    {
        var result = await _validatorService.ValidateArchitecturalModelAsync(model);
        if (result.Success)
        {
            if (model.Id == default)
            {
                var addResult = await _dependencyRepository.AddModelAsync(model);
                return addResult.Success
                    ? Result.Ok(addResult)
                    : Result.Fail<ArchitecturalModel>(addResult.Errors, _logger);
            }

            var editResult = await _dependencyRepository.EditModelAsync(model);
            return editResult.Success
                ? Result.Ok(editResult)
                : Result.Fail<ArchitecturalModel>(editResult.Errors, _logger);
        }

        return result;
    }

    public async Task<Result> DeleteArchitectureModelAsync(Guid id)
    {
        var deletedModel = await _dependencyRepository.DeleteModelAsync(id);
        return deletedModel is not null
            ? Result.Ok($"Architectural model was deleted successfully")
            : Result.Fail($"No architecture model found with id: '{id}'", _logger);
    }
}