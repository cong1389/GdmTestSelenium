using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenerateDocument.Domain.TestSenario;
using GenerateDocument.Test.Base;
using GenerateDocument.Test.PageObjects.FrontEnd;

namespace GenerateDocument.Test.Base
{
    public class AutoSaveContext
    {
        private readonly IAutoSave[] _autoSaves;

        public AutoSaveContext(params IAutoSave[] autoSaves)
        {
            _autoSaves = autoSaves;
        }
    }
}
