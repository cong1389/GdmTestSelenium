using System.Collections.Generic;

namespace GenerateDocument.Domain.TestSenario
{
    public class TestPlan
    {
        public string Name { get; set; }
        
        public List<TestCase> TestCases { get; set; }

        public TestPlan()
        {
           
        }
    }
}
