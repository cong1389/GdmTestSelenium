using System.Linq;

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
            var newValue = paramters.Any(x=>x.ToString().StartsWith("//")) ? paramters.FirstOrDefault()?.ToString() : string.Format(Value, paramters);
            var newKind= paramters.Any(x => x.ToString().StartsWith("//")) ? Locator.XPath : Kind;

            return new ElementLocator(newKind, newValue);
        }
    }
}
