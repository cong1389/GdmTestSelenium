using GenerateDocument.Domain.Designs;
using GenerateDocument.Domain.TestSenario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.XPath;

namespace GenerateDocument.Test.DataProviders
{
    public static class DataDrivenHelper
    {
        public static TestPlan ReadOnlyData(string dataPath)
        {
            var xpd = new XPathDocument(dataPath);
            var xpn = xpd.CreateNavigator();
            var xpi = xpn.Select("/testplan");

            var testPlan = new TestPlan();

            while (xpi.MoveNext())
            {
                testPlan.Name = xpi.Current.GetAttribute("name", xpn.NamespaceURI);
                testPlan.TestCases = new List<TestCase>();

                var testCasesChildNode = xpi.Current.SelectChildren("testcases", xpn.NamespaceURI);
                while (testCasesChildNode.MoveNext())
                {
                    var testCaseChildNode = testCasesChildNode.Current.SelectChildren("testcase", xpn.NamespaceURI);
                    while (testCaseChildNode.MoveNext())//Read each testcase node
                    {
                        var arguments = new Dictionary<string, string>();

                        //Get attr for test case
                        var testCase = new TestCase
                        {
                            Name = testCaseChildNode.Current.GetAttribute("name", xpn.NamespaceURI),
                            Description = testCaseChildNode.Current.GetAttribute("description", xpn.NamespaceURI),
                            ProductName = testCaseChildNode.Current.GetAttribute("productname", xpn.NamespaceURI),
                            Steps = new List<Step>()
                        };

                        var argumentsChildNode = testCaseChildNode.Current.SelectChildren("arguments", xpn.NamespaceURI);
                        while (argumentsChildNode.MoveNext())//Read each argument node
                        {
                            var argumentChildNode = argumentsChildNode.Current.SelectChildren("argument", xpn.NamespaceURI);
                            while (argumentChildNode.MoveNext())
                            {
                                arguments.Add(argumentChildNode.Current.GetAttribute("name", xpn.NamespaceURI), argumentChildNode.Current.GetAttribute("value", xpn.NamespaceURI));
                            }
                        }

                        var stepsNode = testCaseChildNode.Current.SelectChildren("steps", xpn.NamespaceURI);
                        while (stepsNode.MoveNext())//Read each steps node
                        {
                            var stepsCurrentNode = stepsNode.Current;
                            var stepChild = stepsCurrentNode.SelectChildren("step", xpn.NamespaceURI);
                            while (stepChild.MoveNext())//Read each step node
                            {
                                var stepCurrentNode = stepChild.Current;
                                var step = new Step
                                {
                                    ControlType = stepCurrentNode.GetAttribute("controltype", xpn.NamespaceURI),
                                    Action = stepCurrentNode.GetAttribute("action", xpn.NamespaceURI),
                                    ControlId = stepCurrentNode.GetAttribute("controlid", xpn.NamespaceURI),
                                    ControlValue = stepCurrentNode.GetAttribute("controlvalue", xpn.NamespaceURI),
                                    Argument = DictionaryToObject<ArgumentModel>(arguments)
                                };

                                var expectationsChildNode = stepChild.Current.SelectChildren("expectations", xpn.NamespaceURI);
                                while (expectationsChildNode.MoveNext())
                                {
                                    var expectationChildNode = expectationsChildNode.Current.SelectChildren("expectation", xpn.NamespaceURI);
                                    while (expectationChildNode.MoveNext())//Read each expectation node
                                    {
                                        var expectaionCurrentNode = expectationChildNode.Current;
                                        var expectation = new Expectation()
                                        {
                                            AssertType = expectaionCurrentNode.GetAttribute("asserttype", xpn.NamespaceURI),
                                            AssertMessage = expectaionCurrentNode.GetAttribute("assertmessage", xpn.NamespaceURI),
                                            ExpectedValue = expectaionCurrentNode.GetAttribute("expectedvalue", xpn.NamespaceURI),
                                        };

                                        step.Expectations.Add(expectation);
                                    }
                                }

                                var customValueChildNode = stepChild.Current.SelectChildren("customvaluecontrol", xpn.NamespaceURI);
                                while (customValueChildNode.MoveNext())//Read each customvalue node
                                {
                                    var currentNode = customValueChildNode.Current;
                                    int.TryParse(currentNode.GetAttribute("length", xpn.NamespaceURI), out var length);
                                    step.CustomValueStep = new CustomValueStep
                                    {
                                        FormatType = currentNode.GetAttribute("formatype", xpn.NamespaceURI),
                                        Length = length,
                                        Value = currentNode.GetAttribute("value", xpn.NamespaceURI),
                                        ArgumentName = currentNode.GetAttribute("argumentname", xpn.NamespaceURI),
                                        Expression = currentNode.GetAttribute("expression", xpn.NamespaceURI)
                                    };
                                }

                                var mappingModelChildNode = stepChild.Current.SelectChildren("mappingmodel", xpn.NamespaceURI);
                                while (mappingModelChildNode.MoveNext())
                                {
                                    var currentNode = mappingModelChildNode.Current;
                                    step.MappingModel = new MappingModel<DesignModel>()
                                    {
                                        PropertyName = currentNode.GetAttribute("propertyname", xpn.NamespaceURI)
                                    };
                                }

                                testCase.Steps.Add(step);
                            }

                        }

                        testPlan.TestCases.Add(testCase);

                    }
                }
            }

            return testPlan;
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
