// using BPR.Models.Blazor;
// using Microsoft.AspNetCore.Components;
// using Microsoft.AspNetCore.Components.Web;
// using Microsoft.JSInterop;
//
// namespace BPRBlazor.Pages;
//
// public partial class CreateArchitecturalModelForm : ComponentBase
// {
//     private ArchitecturalModel _model = new();
//     private List<(string Message, string Class)> _resultMessages = new();
//
//     private string _dependencyString = "";
//
//     private (double ClientX, double ClientY) _dragStartCoordinates;
//     private ArchitecturalComponent? _draggingComponent;
//
//     private void AddArchitecturalComponent()
//     {
//         var component = new ArchitecturalComponent()
//         {
//             Id = _model.Components.Any() ? _model.Components.Max(c => c.Id) + 1 : 0
//         };
//
//         _model.Components.Add(component);
//     }
//
//     private void RemoveArchitecturalComponent(ArchitecturalComponent component)
//     {
//         _model.Components.Remove(component);
//     }
//
//     private async Task CreateArchitecturalModel()
//     {
//         try
//         {
//             _resultMessages = new();
//             var result = await repository.AddModelAsync(_model.ToBackendModel());
//             if (result.Success)
//             {
//                 _model = new();
//                 _resultMessages.Add(("Model successfully added!", "success"));
//             }
//             else
//             {
//                 foreach (var error in result.Errors)
//                 {
//                     _resultMessages.Add((error, "error"));
//                 }
//             }
//         }
//         catch (Exception ex)
//         {
//             _resultMessages.Add((ex.Message, "error"));
//         }
//     }
//
//     private void AddDependency(int parentComponentId, int dependencyComponentId)
//     {
//         _dependencyString = "";
//
//         if (parentComponentId == dependencyComponentId)
//         {
//             return;
//         }
//
//         var parentComponent = _model.Components.First(c => c.Id == parentComponentId);
//
//         if (!parentComponent.Dependencies.Any(c => c.Id == dependencyComponentId))
//         {
//             parentComponent.Dependencies.Add(_model.Components.First(c => c.Id == dependencyComponentId));
//         }
//     }
//
//     private void OnDragComponentStart(DragEventArgs args, ArchitecturalComponent component)
//     {
//         _dragStartCoordinates = (args.ClientX, args.ClientY);
//         _draggingComponent = component;
//     }
//
//     private async Task OnDropComponent(DragEventArgs args)
//     {
//         if (_draggingComponent == null)
//         {
//             return;
//         }
//
//         var difference = (args.ClientX - _dragStartCoordinates.ClientX, args.ClientY - _dragStartCoordinates.ClientY);
//         var offset = await JS.InvokeAsync<HTMLElementOffset>("getElementOffset", _draggingComponent.Id);
//         _draggingComponent.Style = $"left: {offset.Left + difference.Item1}px; top: {offset.Top + difference.Item2}px";
//     }
//
//     private class HTMLElementOffset
//     {
//         public double Left { get; set; }
//         public double Top { get; set; }
//     }
// }
