using BPRBlazor.ViewModels;

namespace BPRBlazor.ViewModels
{
    public static class Extensions
    {
        public static BPRBE.Models.Persistence.ArchitecturalModel ToBackendModel(this ArchitecturalModelViewModel model)
        {
            return new BPRBE.Models.Persistence.ArchitecturalModel
            {
                Name = model.Name,
                Components = model.Components
                    .Select(c => c.ToBackendModel())
                    .ToList()
            };
        }

        public static BPRBE.Models.Persistence.ArchitecturalComponent ToBackendModel(this ArchitecturalComponentViewModel component)
        {
            return new BPRBE.Models.Persistence.ArchitecturalComponent
            {
                Name = component.Name,
                Dependencies = component.Dependencies
                    .Select(c => c.Id)
                    .ToList()
            };
        }
    }
}
