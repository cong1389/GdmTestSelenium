﻿using System.Collections;
using NUnit.Framework;

namespace GenerateDocument.Test.DataProviders
{
    public static class DataDriven
    {
        public static IEnumerable Conditional
        {
            get
            {
                var testPlan = DataDrivenHelper.ReadOnlyData(ProjectBaseConfiguration.DataDrivenFile);
                foreach (var testPlanTestcase in testPlan.Testcases)
                {
                    var testCaseData = new TestCaseData(testPlanTestcase);
                    testCaseData.SetName("conditional");

                    yield return testCaseData;
                }
            }
        }

        //public static IEnumerable Credentials
        //{
        //    get
        //    {
        //        return DataDrivenHelper.ReadDataDriveFile(@"D:\Project\Practice\GdmTestSelenium\GdmTestSelenium\GenerateDocument.Test\DataProviders\DataDriven.xml"
        //      , "credential"
        //      , new[] { "user", "password" }
        //      , "AutoSave");
        //    }
        //}
    }
}
