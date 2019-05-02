using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateDocument.Domain.TestSenario
{
    public class Expectation
    {
        public string AssertType { get; set; }

        public string AssertMessage { get; set; }

        public string ActualValue { get; set; }

        public string ExpectedValue { get; set; }
    }
}
