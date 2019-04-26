﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.XPath;
using GenerateDocument.Domain.AutoSaves;

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
            var testCases = new List<TestCase>();
            var controls = new List<Control>();

            while (xpi.MoveNext())
            {
                testPlan.Name = xpi.Current.GetAttribute("name", xpn.NamespaceURI);
                testPlan.Testcases = new List<TestCase>();

                var testCasesNode = xpi.Current.SelectChildren("testcases", xpn.NamespaceURI);
                while (testCasesNode.MoveNext())
                {
                    var testCaseNode = testCasesNode.Current.SelectChildren("testcase", xpn.NamespaceURI);
                    while (testCaseNode.MoveNext())//Read each testcase node
                    {
                        //Get attr for test case
                        var testCase = new TestCase
                        {
                            Name = testCaseNode.Current.GetAttribute("name", xpn.NamespaceURI),
                            Description = testCaseNode.Current.GetAttribute("description", xpn.NamespaceURI),
                            ProductName = testCaseNode.Current.GetAttribute("productname", xpn.NamespaceURI),
                            Controls = new List<Control>()
                        };

                        var controlsNode = testCaseNode.Current.SelectChildren("controls", xpn.NamespaceURI);
                        while (controlsNode.MoveNext())//Read each control node
                        {
                            var ctrlsNode = controlsNode.Current;
                            var ctrs = ctrlsNode.SelectChildren("control", xpn.NamespaceURI);
                            while (ctrs.MoveNext())
                            {
                                var controlNode = ctrs.Current;
                                var control = new Control()
                                {
                                    Id = controlNode.GetAttribute("id", xpn.NamespaceURI),
                                    Value = controlNode.GetAttribute("value", xpn.NamespaceURI),
                                    Type = controlNode.GetAttribute("type", xpn.NamespaceURI),
                                    GroupName = controlNode.GetAttribute("groupName", xpn.NamespaceURI),
                                    GroupId = controlNode.GetAttribute("groupId", xpn.NamespaceURI),
                                    Dependencies = new List<Control>()
                                };

                                //Get image
                                var imageNode = controlNode.SelectChildren("image", xpn.NamespaceURI);
                               

                                //Get Dependencies nodes
                                    var dependenciesNode = controlNode.SelectChildren("dependencies", xpn.NamespaceURI);
                                while (dependenciesNode.MoveNext())
                                {
                                    var currentDependenciesNode = dependenciesNode.Current;
                                    var currentDependenciesCtrNode = currentDependenciesNode.SelectChildren("control", xpn.NamespaceURI);
                                    while (currentDependenciesCtrNode.MoveNext())
                                    {
                                        var dependenciesCtrNode = currentDependenciesCtrNode.Current;
                                        control.Dependencies.Add(new Control()
                                        {
                                            Id = dependenciesCtrNode.GetAttribute("id", xpn.NamespaceURI),
                                            Value = dependenciesCtrNode.GetAttribute("value", xpn.NamespaceURI),
                                            Type = dependenciesCtrNode.GetAttribute("type", xpn.NamespaceURI),
                                            GroupName = dependenciesCtrNode.GetAttribute("groupName", xpn.NamespaceURI),
                                            GroupId = dependenciesCtrNode.GetAttribute("groupId", xpn.NamespaceURI)
                                        });
                                    }

                                }

                                testCase.Controls.Add(control);
                            }

                        }

                        testPlan.Testcases.Add(testCase);

                    }
                }
            }

            return testPlan;

        }


        public static IEnumerable<TestCaseData> ReadDataDriveFile(string folder, string testData, string[] diffParam, [Optional] string testName)
        {
            var doc = XDocument.Load(folder);

            if (!doc.Descendants(testData).Any())
            {
                throw new ArgumentNullException("");
            }

            foreach (XElement element in doc.Descendants(testData))
            {
                var testParams = element.Attributes().ToDictionary(k => k.Name.ToString(), v => v.Value);

                var testCaseName = string.IsNullOrEmpty(testName) ? testData : testName;
                try
                {
                    testCaseName = TestCaseName(diffParam, testParams, testCaseName);
                }
                catch (Exception e)
                {
                    throw new Exception("");
                }

                var data = new TestCaseData(testParams);
                data.SetName(testCaseName);
                yield return data;
            }
        }

        private static string TestCaseName(string[] diffParam, Dictionary<string, string> testParams, string testCaseName)
        {
            if (diffParam != null && diffParam.Any())
            {
                foreach (var p in diffParam)
                {
                    string keyValue;
                    bool keyFlag = testParams.TryGetValue(p, out keyValue);

                    if (keyFlag)
                    {
                        if (!string.IsNullOrEmpty(keyValue))
                        {
                            testCaseName += "_" + Regex.Replace(keyValue, "[^0-9a-zA-Z]+", "_");
                        }
                    }
                    else
                    {
                        throw new Exception(p);
                    }
                }
            }

            return testCaseName;
        }
    }
}
