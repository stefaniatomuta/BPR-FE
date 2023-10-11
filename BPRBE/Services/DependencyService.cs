using BPR.Persistence.Models;
using BPR.Persistence.Repositories;
using BPR.Persistence.Utils;
using BPRBE.Models;
using BPRBE.Validators;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace BPRBE.Services;

public class DependencyService : IDependencyService
{
    private readonly IDependencyRepository _dependencyRepository;
    private readonly IValidatorService _validatorService;
    private readonly ILogger<DependencyService> _logger;

    public DependencyService(IDependencyRepository dependencyRepository, IValidatorService validatorService)
    {
        _dependencyRepository = dependencyRepository;
        _validatorService = validatorService;
    }

    public async Task<IList<ArchitecturalModel>> GetArchitecturalModelsAsync()
    {
        var dbModels = await _dependencyRepository.GetArchitecturalModelsAsync();
        var models = new List<ArchitecturalModel>();
        foreach (var model in dbModels)
        {
            var m = new ArchitecturalModel()
            {
                Id = Guid.Parse(model.Id.ToString()),
                Name = model.Name,
                Components = new List<ArchitecturalComponent>()
            };
    
            foreach (var component in model.Components)
            {
                if (component.Dependencies != null)
                {
                    foreach (var dependencyId in component.Dependencies)
                    {
                        var co = m.Components.First(x => x.Id.Equals(dependencyId));

                        if (co != null)
                        {
                            m.Components.Add(co);
                        }
                    }
                }
            }
            models.Add(m);
        }

        return models;
    }

    public async Task<Result> AddOrEditModelAsync(ArchitecturalModel model)
    {
        var result = await _validatorService.ValidateArchitecturalModelAsync(model);
        // TODO - The dependencyRepository.AddModelAsync() returns Result.Fail if an exception is thrown, but this isn't sent back to the user.
        // I think it would make more sense overall, if the services knew about the Result class (and not the repositories) as it is used as validation on business rules.
        // Instead, maybe it would make more sense if the repository would return 'true'/'false' or an 'int' depending on affected documents, and then base the result off that.
        if (result.Success)
        {
            if (model.Id == default)
            {
                return await _dependencyRepository.AddModelAsync(model);
            }
            return await _dependencyRepository.EditModelAsync(model);
            var document = new ArchitecturalModelCollection();
            document.Id = ObjectId.Parse(model.Id.ToString());
            document.Name = model.Name;
            //var components = model.Components.Select(x => x).ToList();
            //document.Components = components;
            return await _dependencyRepository.AddModelAsync(document);
        }

        return result;
    }

    public async Task<Result> DeleteArchitectureModelAsync(ObjectId id)
    {
        // TODO - Not super happy about the id being ObjectId here - this way the caller (frontend) is reliant on the repository using MongoDB. Perhaps it would make more sense to pass a GUID.
        // Then the repository can parse the GUID to an ObjectId. I could easily do this here, but the FE will still know about mongo when we're getting the models.
        // Could be solved by making another set of models - one specific for the DB (the one we currently have) and another more generic one for the overall domain (which the frontend would know about).
        // Would also make more sense if we switched our project structure to use the 'Clean' architecture or so.
        var deletedModel = await _dependencyRepository.DeleteModelAsync(id);
        return deletedModel is not null
            ? Result.Ok()
            : Result.Fail($"No architecture model found with id: '{id}'");
    }
}