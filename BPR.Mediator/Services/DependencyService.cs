using AutoMapper;
using BPR.Mediator.Models;
using BPR.Mediator.Validators;
using BPR.Persistence.Models;
using BPR.Persistence.Repositories;
using BPR.Persistence.Utils;
using Microsoft.Extensions.Logging;

namespace BPR.Mediator.Services;

public class DependencyService : IDependencyService
{
    private readonly IDependencyRepository _dependencyRepository;
    private readonly IValidatorService _validatorService;
    private readonly ILogger<DependencyService> _logger;
    private readonly IMapper _mapper;

    public DependencyService(IDependencyRepository dependencyRepository, IMapper mapper, IValidatorService validatorService, ILogger<DependencyService> logger)
    {
        _dependencyRepository = dependencyRepository;
        _validatorService = validatorService;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IList<ArchitecturalModel>> GetArchitecturalModelsAsync()
    {
        var dbModels = await _dependencyRepository.GetArchitecturalModelsAsync();
        var list = _mapper.Map<IList<ArchitecturalModelCollection>, List<ArchitecturalModel>>(dbModels);
        return list;
    }

    public async Task<Result> AddOrEditModelAsync(ArchitecturalModel model)
    {
        var result = await _validatorService.ValidateArchitecturalModelAsync(model);
        if (result.Success)
        {
            if (model.Id == default)
            {
                var addResult = await _dependencyRepository.AddModelAsync(
                    _mapper.Map<ArchitecturalModel, ArchitecturalModelCollection>(model));
                return addResult.Success ? Result.Ok(addResult) : Result.Fail<ArchitecturalModelCollection>(addResult.Errors, _logger);
            }
            var editResult = await _dependencyRepository.EditModelAsync(_mapper.Map<ArchitecturalModel, ArchitecturalModelCollection>(model));
            return editResult.Success ? Result.Ok(editResult) : Result.Fail<ArchitecturalModelCollection>(editResult.Errors, _logger);
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