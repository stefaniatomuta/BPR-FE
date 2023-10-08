// using BPR.Models.Blazor;
//
// namespace BPRBlazor.Models
// {
//     public static class Extensions
//     {
//         public static ArchitecturalModel ToBackendModel(this ArchitecturalModel model)
//         {
//             return new ArchitecturalModel
//             {
//                 Name = model.Name,
//                 Components = model.Components
//                     .Select(c => c.ToBackendModel())
//                     .ToList()
//             };
//         }
//
//         public static ArchitecturalComponent ToBackendModel(this ArchitecturalComponent component)
//         {
//             return new ArchitecturalComponent
//             {
//                 Name = component.Name,
//                 Dependencies = component.Dependencies
//                     .Select(c => c.Id)
//                     .ToList()
//             };
//         }
//     }
// }
