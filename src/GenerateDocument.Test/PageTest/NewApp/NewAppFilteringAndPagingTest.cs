using GenerateDocument.Common.Extensions;
using GenerateDocument.Common.Helpers;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace GenerateDocument.Test.PageTest.NewApp
{
    public partial class NewApp_Test : PageTestBase
    {
        private static IEnumerable<string> DesignStatuses => new List<string> { "Unpublished", "Unreviewed", "Rejected", "Shipped" };
        private IDictionary<string, string[]> _designsByStatuses = new Dictionary<string, string[]>();

        [Test]
        public void Paging_ShouldWorkCorrectly()
        {
            LoginStep();

            VerifyPaging();

            foreach (var status in DesignStatuses)
            {
                var designNames = _myDesign.GetDesignNamesByStatus(status);

                _designsByStatuses.Add(status, designNames);
            }
        }

        [Test]
        public void SortingFunction_DefaultOption_MustBe_LastEditedFirst()
        {
            LoginStep();

            var selectedSortingOption = _myDesign.GetSelectedSortingOption();

            Assert.IsTrue(!string.IsNullOrEmpty(selectedSortingOption) && selectedSortingOption.IsEquals("All (last edited first)"), "Default sorting value should be All (last edited first)");
        }

        [Test]
        public void SortingFunction_ShouldWorkedCorrectly_WithDefaultOption()
        {
            LoginStep();

            Assert.IsTrue(_myDesign.GetDesignNames().Length > 0, "Designs should displayed when sort by default option");
        }

        [Test]
        public void SortingFunction_ShouldWorkedCorrectly_WhenOrderByName()
        {
            var sortValue = "All (name A-Z)";

            LoginStep();

            var designNamesBefore = _myDesign.GetDesignNames();

            _myDesign.DoSort(sortValue);

            var designNamesAfter = _myDesign.GetDesignNames();

            bool checkSorted(string[] arr1, string[] arr2)
            {
                var ifSorted = false;
                for (var i = 0; i < arr1.Length; i++)
                {
                    if (!arr1[i].IsEquals(arr2[i]))
                    {
                        ifSorted = true;
                        break;
                    }
                }

                return ifSorted;
            }

            Assert.IsTrue(checkSorted(designNamesBefore, designNamesAfter), "Designs should displayed when sort by Name");

            var selectedSortingOption = _myDesign.GetSelectedSortingOption();

            Assert.IsTrue(!string.IsNullOrEmpty(selectedSortingOption) && selectedSortingOption.IsEquals(sortValue), $"Default sorting value should be {sortValue}");
        }

        [Test, TestCaseSource(nameof(DesignStatuses))]
        public void FilteringFunction_ShouldWorkedCorrectly_WithEachOption(string status)
        {
            LoginStep();

            var expectedNumber = _designsByStatuses[status].Length;
            if (expectedNumber == 0)
                return;

            if (status.IsEquals("Shipped"))
            {
                status = "Approved";
            }

            _myDesign.DoSort(status);

            var designsCountByStatus = _myDesign.CountDesignsByStatus(expectedNumber, status);

            Assert.IsTrue(expectedNumber == designsCountByStatus, $"Designs should displayed when filter by {status} status");

            var selectedSortingOption = _myDesign.GetSelectedSortingOption();

            Assert.IsTrue(!string.IsNullOrEmpty(selectedSortingOption) && selectedSortingOption.IsEquals(status), $"Selected filter value should be {status}");
        }

        [Test, TestCaseSource(nameof(DesignStatuses))]
        public void SearchFunction_ShouldReturnResultsCorrectly_WhenCombinedWithEachFilteringOption(string status)
        {
            LoginStep();

            DriverContext.Driver.RefreshPage();

            var designNames = _designsByStatuses[status];
            if (designNames.Length == 0)
                return;

            if (status.IsEquals("Shipped"))
            {
                status = "Approved";
            }

            _myDesign.DoSort(status);

            var designName = $"{designNames} {NameHelper.RandomName(05)}";
            _myDesign.DoSearchAndWaitResults(designName);
            var actualCount = CountResults();
            var expectedCount = designNames.Count(x => x.IsEquals(designName));

            Assert.IsTrue(expectedCount == actualCount, $"Search function should return designs correctly when combined with filtering function; expectedCount: {expectedCount}; return designs count: {actualCount}");
        }

        [Test]
        public void SearchFunction_ShouldReturnMessage_IfInputDoesNotMachAny()
        {
            LoginStep();

            DriverContext.Driver.RefreshPage();

            _myDesign.DoSearchAndWaitResults(NameHelper.RandomName(10));
            var message = _myDesign.GetNoDesignsMessage();

            Assert.IsTrue(!string.IsNullOrEmpty(message), $"Search function should return no designs message if not found; message content: {message}");
        }

        private int CountResults()
        {
            VerifyPaging();

            return _myDesign.GetDesignNames().Length;
        }

        private void VerifyPaging()
        {
            var canPaging = _myDesign.DoPaging();
            if (canPaging)
            {
                Assert.IsTrue(canPaging, "Paging worked correctly");

                VerifyPaging();
            }
        }
    }
}
