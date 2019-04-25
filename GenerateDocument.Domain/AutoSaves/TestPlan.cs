using System.Collections.Generic;

namespace GenerateDocument.Domain.AutoSaves
{
    public class TestPlan
    {
        public string Name { get; set; }

        public List<TestCase> Testcases { get; set; }
    }
}
