﻿using GenerateDocument.Domain.Designs;
using GenerateDocument.Domain.TestSenario;

namespace GenerateDocument.Test.Base
{
    public interface IAutoSave
    {
        void PerformToControlType(Step step, DesignModel designModel);

    }
}