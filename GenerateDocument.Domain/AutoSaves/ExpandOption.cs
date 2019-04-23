using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateDocument.Domain.AutoSaves
{
    public class ExpandOption
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<OptionField> OptionFields;

        public ExpandOption()
        {
            OptionFields= new List<OptionField>();
        }
    }
}
