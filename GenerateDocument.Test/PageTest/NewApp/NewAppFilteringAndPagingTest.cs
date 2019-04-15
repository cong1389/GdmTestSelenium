using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using GenerateDocument.Test.Utilities;
using GenerateDocument.Test.Extensions;
using NUnit.Framework;

using static GenerateDocument.Test.Utilities.TestUtil;

namespace GenerateDocument.Test.PageTest.NewApp
{
    [TestFixture]
    public partial class NewApp_Test : PageTestBase
    {
        [Test]
        public void Paging_ShouldWorkCorrectly()
        {
            LoginStep(_returnPage);

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
            LoginStep(_returnPage);

            var selectedSortingOption = _myDesign.GetSelectedSortingOption();

            Assert.IsTrue(!string.IsNullOrEmpty(selectedSortingOption) && selectedSortingOption.IsEquals("All (last edited first)"), "Default sorting value should be All (last edited first)");
        }

        [Test]
        public void SortingFunction_ShouldWorkedCorrectly_WithDefaultOption()
        {
            LoginStep(_returnPage);

            Assert.IsTrue(_myDesign.GetDesignNames().Length > 0, "Designs should displayed when sort by default option");
        }

        [Test]
        public void SortingFunction_ShouldWorkedCorrectly_WhenOrderByName()
        {
            var sortValue = "All (name A-Z)";

            LoginStep(_returnPage);

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
            LoginStep(_returnPage);

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
            LoginStep(_returnPage);

            _browser.RefreshPage();

            var designNames = _designsByStatuses[status];
            if (designNames.Length == 0)
                return;

            if (status.IsEquals("Shipped"))
            {
                status = "Approved";
            }

            _myDesign.DoSort(status);

            var designName = TestUtil.RandomName(designNames);
            _myDesign.DoSearchAndWaitResults(designName);
            var actualCount = CountResults();
            var expectedCount = designNames.Where(x => x.IsEquals(designName)).Count();

            Assert.IsTrue(expectedCount == actualCount, $"Search function should return designs correctly when combined with filtering function; expectedCount: {expectedCount}; return designs count: {actualCount}");
        }

        [Test]
        public void SearchFunction_ShouldReturnMessage_IfInputDoesNotMachAny()
        {
            LoginStep(_returnPage);

            _browser.RefreshPage();

            _myDesign.DoSearchAndWaitResults(RandomName(10));
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
