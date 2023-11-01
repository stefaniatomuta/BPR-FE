using System.ComponentModel;
using BPR.Mediator.Enums;

namespace BPR.Mediator.Mappers;

public static class ViolationTypeMapper
{
    public static string GetViolationTypeName(ViolationType violationType)
    {
        return violationType switch
        {
            ViolationType.Unknown => "Unknown violation type",
            ViolationType.ForbiddenDependency => "Forbidden dependency",
            ViolationType.MismatchedNamespace => "Mismatched namespace",
            _ => throw new InvalidEnumArgumentException("No valid rule with this name")
        };
    }
    
}