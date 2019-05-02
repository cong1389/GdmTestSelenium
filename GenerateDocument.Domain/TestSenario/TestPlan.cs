using System.Collections.Generic;

namespace GenerateDocument.Domain.TestSenario
{
    public class TestPlan
    {
        public string Name { get; set; }

        public List<TestCase> Testcases { get; set; }
    }
}
