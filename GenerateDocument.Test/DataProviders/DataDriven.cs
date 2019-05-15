using System.Collections;
using NUnit.Framework;

namespace GenerateDocument.Test.DataProviders
{
    public static class DataDriven
    {
        public static IEnumerable ConditionalControl
        {
            get
            {
                var dataDriven = DataDrivenHelper.ReadOnlyData(ProjectBaseConfiguration.GetDataDrivenForConditional);
                foreach (var testCases in dataDriven.TestCases)
                {
                    var testCaseData = new TestCaseData(testCases);
                    testCaseData.SetName($"{testCases.Name}_{testCases.ProductName}");

                    yield return testCaseData;
                }
            }
        }

        public static IEnumerable SpecialCharacters
        {
            get
            {
                var dataDriven = DataDrivenHelper.ReadOnlyData(ProjectBaseConfiguration.GetDataDrivenForSpecialCharacters);
                foreach (var testCases in dataDriven.TestCases)
                {
                    var testCaseData = new TestCaseData(testCases);
                    testCaseData.SetName($"{testCases.Name}_{testCases.ProductName}");

                    yield return testCaseData;
                }
            }
        }

        //public static IEnumerable MigrationControl
        //{
        //    get
        //    {
        //        var dataDriven = DataDrivenHelper.ReadOnlyData(ProjectBaseConfiguration.GetDataDrivenForMigration);
        //        foreach (var testCases in dataDriven.TestCases)
        //        {
        //            var testCaseData = new TestCaseData(testCases);
        //            testCaseData.SetName($"{testCases.Name}_{testCases.ProductName}");

        //            yield return testCaseData;
        //        }
        //    }
        //}
    }
}
