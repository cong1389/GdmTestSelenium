using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GenerateDocument.Domain.Designs;

namespace GenerateDocument.Domain.TestSenario
{
    public class TestCase
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string ProductName { get; set; }

        public List<Step> Steps { get; set; }

        public DesignModel DesignModel
        {
            get
            {
                var dic = new Dictionary<string, string>();

                Steps.ForEach(step =>
                {
                    if (step?.MappingModel?.PropertyValue != null)
                    {
                        if (!dic.ContainsKey(step.MappingModel.PropertyName))
                        {
                            dic.Add(step.MappingModel.PropertyName, step.MappingModel.PropertyValue);
                        }
                    }

                });

                return DictionaryToObject<DesignModel>(dic);
            }
        }

        private static T DictionaryToObject<T>(IDictionary<string, string> dict) where T : new()
        {
            var t = new T();

            var properties = t.GetType().GetProperties();

            foreach (var property in properties)
            {
                if (!dict.Any(x => x.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase)))
                {
                    continue;
                }

                var item = dict.First(x => x.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase));

                var tPropertyType = t.GetType().GetProperty(property.Name)?.PropertyType;
              
                var newT = Nullable.GetUnderlyingType(tPropertyType) ?? tPropertyType;
              
                object newA = Convert.ChangeType(item.Value, newT);

                t.GetType().GetProperty(property.Name)?.SetValue(t, newA, null);
            }

            return t;
        }
    }
}
