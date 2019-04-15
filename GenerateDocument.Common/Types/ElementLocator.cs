using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateDocument.Common.Types
{
    public class ElementLocator
    {
        public Locator Kind { get; set; }

        public string Value { get; set; }

        public ElementLocator(Locator kind, string value)
        {
            Kind = kind;
            Value = value;
        }
        public ElementLocator Format(params object[] paramters)
        {
            return new ElementLocator(this.Kind, string.Format(Value, paramters));
        }
    }
}
