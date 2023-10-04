﻿using BPR.Models.Persistence;
using BPRBE.Models.Persistence;

namespace BPRBE.Validators;

public interface IValidatorService
{
    public Task<Result> ValidateArchitecturalModelAsync(MongoArchitecturalModel model);
}