using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateDocument.Domain.TestSenario
{
    public class Step
    {
        public string ControlId { get; set; }

        public string ControlValue { get; set; }

        public string ControlType { get; set; }

        public string Action { get; set; }

        public List<Expectation> Expectations { get; set; }

        public Step()
        {
            Expectations= new List<Expectation>();
        }
    }
}
