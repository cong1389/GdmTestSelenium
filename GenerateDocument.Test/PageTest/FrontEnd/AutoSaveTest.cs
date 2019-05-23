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
    public class AutoSaveTest : PageTestBase
    {
        private PageCommonAction _action;
        private UserContentStart _userContentStart;
        private UserEditFormFilling _userEditFormFilling;
        private UserEditPrinting _userEditPrinting;
        private UserEditFinish _userEditFinish;

        private AdminLogin _adminLogin;
        private AdminProducts _adminProducts;

        private MyDesign _myDesign;

        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public AutoSaveTest(string environment) : base(environment)
        {

        }

        [SetUp]
        public void SetUpOneTestCase()
        {
            _action = new PageCommonAction(DriverContext.Driver);
            _userContentStart = new UserContentStart(DriverContext);
            _userEditFormFilling = new UserEditFormFilling(DriverContext);
            _userEditPrinting = new UserEditPrinting(DriverContext);
            _userEditFinish = new UserEditFinish(DriverContext);

            _adminLogin = new AdminLogin(DriverContext);
            _adminProducts = new AdminProducts(DriverContext);


            _myDesign = new MyDesign(DriverContext);
        }

        [Test]
        [TestCaseSource(typeof(DataDriven), "ConditionalControl")]
        public void TestConditionalControlOfDataDriven(TestCase testcase)
        {
            UserSiteLoginStep();

            _myDesign.CreateDesign();

            var documentBefore = _userContentStart.SearchDocument(testcase.ProductName);
            Assert.IsTrue(!string.IsNullOrEmpty(documentBefore.Id));
            _userContentStart.SelectDocument(documentBefore.Id);

            foreach (var step in testcase.Steps)
            {
                PerformToPageType(step, testcase.DesignModel);
                ValidateExpectation(step);
            }
        }

        [Test]
        [TestCaseSource(typeof(DataDriven), "SpecialCharacters")]
        public void SpecialCharactersOfDataDriven(TestCase testcase)
        {
            UserSiteLoginStep();

            _myDesign.CreateDesign();

            var documentBefore = _userContentStart.SearchDocument(testcase.ProductName);
            Assert.IsTrue(!string.IsNullOrEmpty(documentBefore.Id));
            _userContentStart.SelectDocument(documentBefore.Id);

            foreach (var step in testcase.Steps)
            {
                PerformToPageType(step, testcase.DesignModel);
                ValidateExpectation(step);
            }
        }

        [Test]
        [TestCaseSource(typeof(DataDriven), "MigrationControl")]
        public void MigrationControlOfDataDriven(TestCase testcase)
        {
            foreach (var step in testcase.Steps)
            {
                PerformToPageType(step, testcase.DesignModel);
                ValidateExpectation(step);
            }
        }

        //  [Test]
        public void VerifyContentFromPdf()
        {
            var filePath = @"D:\Project\Practice\GdmTest\generateddocumenttest\GenerateDocument.Test\Content\imageTest\D-F6117503_00001.pdf";
            var text = FilesHelper.ExtractTextFromPdf(filePath);
        }

        private void AdminCustomA4PosterProduct(string productName)
        {
            _adminProducts.NavigateTo();

            _adminProducts.GoToAdminProductDetails(productName);

            //_adminProductDetails.ClickToFormFilling();

            //_adminOptionSet.ClickToFooterLink();

            //_adminOptionField.ClickToSelectLogoAndSupportingLogos();

            //_adminOptionField.ClickToUpdateSetting();

            //_adminOptionSet.ClickToGoback();

            //_adminProductDetails.ClickToRelease();

            //_adminProductRetired.ClickToUpdateSetting();
        }

        private void PerformToPageType(Step step, DesignModel designModel)
        {
            var currentPage = step.Action.Equals("navigate") ? step.ControlValue : DriverContext.Driver.GetCurrentPage();

            Enum.TryParse(currentPage, true, out PageTypes pageTypes);

            logger.Info($"currentPage: {currentPage}; controlType: {step.ControlType}; controlId: {step.ControlId}; controlValue: {step.ControlValue}");

            switch (pageTypes)
            {
                case PageTypes.Login:
                    new UserLogin(DriverContext).PerformToControlType(step, designModel);
                    break;

                case PageTypes.UserContentStart:
                    _userContentStart.PerformToControlType(step, designModel);
                    break;

                case PageTypes.UserEditFormFilling:
                    _userEditFormFilling.PerformToControlType(step, designModel);
                    break;

                case PageTypes.UserEditPrinting:
                    _userEditPrinting.PerformToControlType(step, designModel);
                    break;

                case PageTypes.UserEditFinish:
                    _userEditFinish.PerformToControlType(step, designModel);
                    break;

                case PageTypes.AdminLogin:
                    _adminLogin.PerformToControlType(step, designModel);
                    break;

                case PageTypes.AdminProducts:
                    new AdminProducts(DriverContext).PerformToControlType(step, designModel);
                    break;

                case PageTypes.AdminProductDetails:
                    new AdminProductDetails(DriverContext)
                        .PerformToControlType(step, designModel);
                    break;

                case PageTypes.AdminOptionSet:
                    new AdminOptionSet(DriverContext)
                        .PerformToControlType(step, designModel);
                    break;

                case PageTypes.AdminOptionField:
                    new AdminOptionField(DriverContext)
                        .PerformToControlType(step, designModel);
                    break;

                case PageTypes.AdminProductRetired:
                    new AdminProductRetired(DriverContext)
                        .PerformToControlType(step, designModel);
                    break;

                case PageTypes.Designs:
                    new MyDesign(DriverContext)
                        .PerformToControlType(step, designModel);
                    break;

                case PageTypes.OneDesign:
                    new OneDesign(DriverContext)
                        .PerformToControlType(step, designModel);
                    break;
            }
        }

        private void ValidateExpectation(Step step)
        {
            step.Expectations.ForEach(x =>
            {
                switch (x.AssertType)
                {
                    case "istrue":
                        Assert.IsTrue(_action.GetNotifyMessage, x.AssertMessage);
                        break;

                    case "equals":
                        if (step.ControlType.Equals("image"))
                        {
                            var actualValue = _userEditFormFilling.GetImageNameAfterUploaded(step.ControlId);
                            Assert.AreEqual(actualValue, x.ExpectedValue, x.AssertMessage);
                        }
                        break;

                    case "compareoutputfile":
                        var downloadedFilesCount = FilesHelper.CountFiles(ProjectBaseConfiguration.NewAppTestDir);
                        Console.WriteLine($"downloadedFilesCount: {downloadedFilesCount}; ExpectedValue: {x.ExpectedValue}");
                        Assert.IsTrue(int.Parse(x.ExpectedValue) == downloadedFilesCount, x.AssertMessage);
                        break;

                }
            });
        }

        private void UserSiteLoginStep()
        {
            _userContentStart.NavigateTo();

            new UserLogin(DriverContext)
                .NavigateTo()
               .LoginSystem(ProjectBaseConfiguration.UserId, ProjectBaseConfiguration.UserPassword);
        }

    }
}
