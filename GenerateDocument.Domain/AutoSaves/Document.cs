using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateDocument.Domain.AutoSaves
{
    public class Document
    {
        public string Name { get; set; }

        public List<ExpandOption> ExpandOptions;

        public Document()
        {
            ExpandOptions = new List<ExpandOption>();
        }
    }
}
