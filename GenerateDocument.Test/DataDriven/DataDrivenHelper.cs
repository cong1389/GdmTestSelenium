using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using NUnit.Framework;

namespace GenerateDocument.Test.DataDriven
{
    public static class DataDrivenHelper
    {
        public static IEnumerable<TestCaseData> ReadDataDriveFile(string folder, string testData, string[] diffParam, [Optional] string testName)
        {
            var doc = XDocument.Load(folder);

            if (!doc.Descendants(testData).Any())
            {
                throw new ArgumentNullException(string.Format(" Exception while reading Data Driven file\n row '{0}' not found \n in file '{1}'", testData, folder));
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
                    throw new Exception(
                        string.Format(
                            " Exception while reading Data Driven file\n test data '{0}' \n test name '{1}' \n searched key '{2}' not found in row \n '{3}'  \n in file '{4}'",
                            testData,
                            testName,
                            e.Message,
                            element,
                            folder));
                }

                var data = new TestCaseData(testParams);
                data.SetName(testCaseName);
                yield return data;
            }
        }

        /// <summary>
        /// Get the name of test case from value of parameters.
        /// </summary>
        /// <param name="diffParam">The difference parameter.</param>
        /// <param name="testParams">The test parameters.</param>
        /// <param name="testCaseName">Name of the test case.</param>
        /// <returns>Test case name</returns>
        /// <exception cref="NullReferenceException">Exception when trying to set test case name</exception>
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
