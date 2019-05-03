using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenerateDocument.Domain.TestSenario;

namespace GenerateDocument.Test.PageObjects.FrontEnd
{
    interface IAutoSave
    {
        void PerformToControlType(Step step);
    }
}
