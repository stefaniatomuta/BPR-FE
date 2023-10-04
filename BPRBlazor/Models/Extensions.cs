namespace BPRBlazor.Models
{
    public static class Extensions
    {
        public static BPRBE.Models.Persistence.ArchitecturalModel ToBackendModel(this ArchitecturalModel model)
        {
            return new BPRBE.Models.Persistence.ArchitecturalModel
            {
                Name = model.Name,
                Components = model.Components
                    .Select(c => c.ToBackendModel())
                    .ToList()
            };
        }

        public static BPRBE.Models.Persistence.ArchitecturalComponent ToBackendModel(this ArchitecturalComponent component)
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
