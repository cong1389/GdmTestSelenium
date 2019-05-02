using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateDocument.Domain.TestSenario
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
}
