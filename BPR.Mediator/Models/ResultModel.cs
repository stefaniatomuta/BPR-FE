namespace BPR.Mediator.Models;

public class ResultModel {
   public double Score { get; set; }
   public List<ViolationModel> Violations { get; set; } = new();

}
