using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.XPath;
using GenerateDocument.Domain.TestSenario;

namespace GenerateDocument.Test.DataProviders
{
    public static class DataDrivenHelper
    {
        public static TestPlan ReadOnlyData(string dataPath)
        {
            XPathDocument xpd = new XPathDocument(dataPath);
            XPathNavigator xpn = xpd.CreateNavigator();
            XPathNodeIterator xpi = xpn.Select("/testplan");

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
                        //Get attr for test case
                        var testCase = new TestCase
                        {
                            Name = testCaseChildNode.Current.GetAttribute("name", xpn.NamespaceURI),
                            Description = testCaseChildNode.Current.GetAttribute("description", xpn.NamespaceURI),
                            ProductName = testCaseChildNode.Current.GetAttribute("productname", xpn.NamespaceURI),
                            Steps = new List<Step>()
                        };

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
                                    ControlValue = stepCurrentNode.GetAttribute("controlvalue", xpn.NamespaceURI)
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
                                    var customCurrentNode = customValueChildNode.Current;
                                    int.TryParse(customCurrentNode.GetAttribute("length", xpn.NamespaceURI), out int length);
                                    step.CustomValueStep = new CustomValueStep
                                    {
                                        FormatType = customCurrentNode.GetAttribute("formatype", xpn.NamespaceURI),
                                        Length = length,
                                        Value = customCurrentNode.GetAttribute("value", xpn.NamespaceURI),
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
    }
}
