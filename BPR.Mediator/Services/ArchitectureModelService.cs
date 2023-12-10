using BPR.Mediator.Interfaces;
using BPR.Mediator.Utils;
using BPR.Mediator.Validators;
using BPR.Model.Architectures;
using Microsoft.Extensions.Logging;

namespace BPR.Mediator.Services;

public class ArchitectureModelService : IArchitectureModelService
{
    private readonly IArchitectureModelRepository _architectureModelRepository;
    private readonly IValidatorService _validatorService;
    private readonly ILogger<ArchitectureModelService> _logger;

    public ArchitectureModelService(IArchitectureModelRepository architectureModelRepository, IValidatorService validatorService,
        ILogger<ArchitectureModelService> logger)
    {
        _architectureModelRepository = architectureModelRepository;
        _validatorService = validatorService;
        _logger = logger;
    }

    public async Task<IList<ArchitectureModel>> GetArchitectureModelsAsync()
    {
        return await _architectureModelRepository.GetArchitectureModelsAsync();
    }

    public async Task<Result> AddOrEditModelAsync(ArchitectureModel model)
    {
        var result = await _validatorService.ValidateArchitectureModelAsync(model);
        if (result.Success)
        {
            if (model.Id == default)
            {
                var addResult = await _architectureModelRepository.AddModelAsync(model);
                return addResult.Success
                    ? Result.Ok(addResult)
                    : Result.Fail<ArchitectureModel>(addResult.Errors, _logger);
            }

            var editResult = await _architectureModelRepository.EditModelAsync(model);
            return editResult.Success
                ? Result.Ok(editResult)
                : Result.Fail<ArchitectureModel>(editResult.Errors, _logger);
        }

        return result;
    }

    public async Task<Result> DeleteArchitectureModelAsync(Guid id)
    {
        var deletedModel = await _architectureModelRepository.DeleteModelAsync(id);
        return deletedModel is not null
            ? Result.Ok($"Architecture model was deleted successfully")
            : Result.Fail($"No architecture model found with id: '{id}'", _logger);
    }
}