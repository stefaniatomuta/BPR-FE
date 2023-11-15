using BPR.Mediator.Enums;
using BPR.Persistence.Models;

namespace BPR.Mediator.Models;

public class ResultModel {
   public Guid Id { get; set; }
   public double Score { get; set; }
   public DateTime ResultStart { get; set; }
   public DateTime ResultEnd { get; set; }
   public ResultStatus ResultStatus { get; set; }
   public List<ViolationModel> Violations { get; set; } = new();
}
