using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateDocument.Domain.AutoSaves
{
    public class Control
    {
        public string Id { get; set; }

        public string Type { get; set; }

        public string Value { get; set; }

        public string GroupName { get; set; }

        public string GroupId { get; set; }

        public List<Control> Dependencies { get; set; }
    }

    public class TestCase
    {
        public string Name { get; set; }

        public string Description { get; set; }
        public string ProductName { get; set; }
        
        public List<Control> Controls { get; set; }
    }

    public class TestPlan
    {
        public string Name { get; set; }

        public List<TestCase> Testcases { get; set; }
    }
}
