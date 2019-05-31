using GenerateDocument.Common.Extensions;
using GenerateDocument.Common.Helpers;
using GenerateDocument.Common.Types;
using GenerateDocument.Domain.Designs;
using GenerateDocument.Domain.TestSenario;
using GenerateDocument.Test.DataProviders;
using GenerateDocument.Test.PageObjects;
using GenerateDocument.Test.PageObjects.BackEnd;
using GenerateDocument.Test.PageObjects.FrontEnd;
using GenerateDocument.Test.PageObjects.NewApp;
using log4net;
using NUnit.Framework;
using System;
using System.Reflection;

namespace GenerateDocument.Test.PageTest.FrontEnd
{
    [TestFixture("Chrome")]
    [TestFixture("Firefox")]
    [NonParallelizable]
    public class AutoSaveTest : PageTestBase
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public AutoSaveTest(string environment) : base(environment)
        {

        }

        [Test]
        [TestCaseSource(typeof(DataDriven), "ConditionalControl")]
        public void TestConditionalControlOfDataDriven(TestCase testcase)
        {
            foreach (var step in testcase.Steps)
            {
                PerformToPageType(step, testcase.DesignModel);
                ValidationStep(step);
            }
        }

        [Test]
        [TestCaseSource(typeof(DataDriven), "SpecialCharacters")]
        public void SpecialCharactersOfDataDriven(TestCase testcase)
        {
            foreach (var step in testcase.Steps)
            {
                PerformToPageType(step, testcase.DesignModel);
                ValidationStep(step);
            }
        }

        [Test]
        [TestCaseSource(typeof(DataDriven), "MigrationControl")]
        public void MigrationControlOfDataDriven(TestCase testcase)
        {
            foreach (var step in testcase.Steps)
            {
                PerformToPageType(step, testcase.DesignModel);
                ValidationStep(step);
            }
        }

        [Test]
        [TestCaseSource(typeof(DataDriven), "NewAppWorkFlow")]
        public void NewAppWorkFlow(TestCase testcase)
        {
            foreach (var step in testcase.Steps)
            {
                PerformToPageType(step, testcase.DesignModel);
                ValidationStep(step);
            }
        }

        private void PerformToPageType(Step step, DesignModel designModel)
        {
            var currentPage = step.ControlType.Equals(ControlTypes.Browser.ToString(), StringComparison.OrdinalIgnoreCase) ? step.ControlValue : DriverContext.Driver.GetCurrentPage();

            Enum.TryParse(currentPage, true, out PageTypes pageTypes);

            logger.Info($"currentPage: {currentPage}; controlType: {step.ControlType}; controlId: {step.ControlId}; controlValue: {step.ControlValue}");

            switch (pageTypes)
            {
                case PageTypes.Login:
                    new UserLogin(DriverContext).PerformToControlType(step, designModel);
                    break;

                case PageTypes.UserContentStart:
                    new UserContentStart(DriverContext).PerformToControlType(step, designModel);
                    break;

                case PageTypes.UserEditFormFilling:
                    new UserEditFormFilling(DriverContext).PerformToControlType(step, designModel);
                    break;

                case PageTypes.UserEditPrinting:
                    new UserEditPrinting(DriverContext).PerformToControlType(step, designModel);
                    break;

                case PageTypes.UserEditFinish:
                    new UserEditFinish(DriverContext).PerformToControlType(step, designModel);
                    break;

                case PageTypes.AdminLogin:
                    new AdminLogin(DriverContext).PerformToControlType(step, designModel);
                    break;

                case PageTypes.AdminProducts:
                    new AdminProducts(DriverContext).PerformToControlType(step, designModel);
                    break;

                case PageTypes.AdminProductDetails:
                    new AdminProductDetails(DriverContext).PerformToControlType(step, designModel);
                    break;

                case PageTypes.AdminOptionSet:
                    new AdminOptionSet(DriverContext).PerformToControlType(step, designModel);
                    break;

                case PageTypes.AdminOptionField:
                    new AdminOptionField(DriverContext).PerformToControlType(step, designModel);
                    break;

                case PageTypes.AdminProductRetired:
                    new AdminProductRetired(DriverContext).PerformToControlType(step, designModel);
                    break;

                case PageTypes.Designs:
                    new MyDesign(DriverContext).PerformToControlType(step, designModel);
                    break;

                case PageTypes.OneDesign:
                    new OneDesign(DriverContext).PerformToControlType(step, designModel);
                    break;
            }
        }

        private void ValidationStep(Step step)
        {
            step.Expectations.ForEach(x =>
            {
                Enum.TryParse(x.AssertType, true, out ValidationTypes validationType);

                switch (validationType)
                {
                    case ValidationTypes.IsDisplayNotification:
                        Console.WriteLine($"Validation by contronlId: {step.ControlId}; controlValue: {step.ControlValue}");
                        Assert.IsTrue(new PageCommonAction(DriverContext).GetNotifyMessage(), x.AssertMessage);
                        break;

                    case ValidationTypes.EqualLableImage:
                        if (step.ControlType.Equals(ControlTypes.Image.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            var actualValue = new UserEditFormFilling(DriverContext).GetImageNameAfterUploaded(step.ControlId);
                            Assert.AreEqual(actualValue, x.ExpectedValue, x.AssertMessage);
                        }
                        break;

                    case ValidationTypes.EqualCompareOutputFile:
                        var downloadedFilesCount = FilesHelper.CountFiles(ProjectBaseConfiguration.DownloadFolder);
                        Console.WriteLine($"DownloadedFilesCount: {downloadedFilesCount}; ExpectedValue: {x.ExpectedValue}");
                        Assert.IsTrue(int.Parse(x.ExpectedValue) == downloadedFilesCount, x.AssertMessage);
                        break;

                }
            });
        }
    }
}
