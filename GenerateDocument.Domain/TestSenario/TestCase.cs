using System.Collections.Generic;

namespace GenerateDocument.Domain.TestSenario
{
    public class TestCase
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string ProductName { get; set; }
        
        public List<Step> Steps { get; set; }
    }
}
