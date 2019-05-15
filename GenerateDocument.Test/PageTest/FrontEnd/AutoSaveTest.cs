using System;
using GenerateDocument.Common.Extensions;
using GenerateDocument.Domain.TestSenario;
using GenerateDocument.Test.DataProviders;
using GenerateDocument.Test.PageObjects;
using GenerateDocument.Test.PageObjects.FrontEnd;
using GenerateDocument.Test.PageObjects.NewApp;
using NUnit.Framework;
using System.Linq;
using GenerateDocument.Common.Helpers;
using GenerateDocument.Common.Types;
using GenerateDocument.Test.PageObjects.BackEnd;
using System.Collections.Generic;

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
        private AdminExtensions _adminExtensions;
        private AdminProducts _adminProducts;
        private AdminProductDetails _adminProductDetails;
        private AdminOptionSet _adminOptionSet;
        private AdminOptionField _adminOptionField;
        private AdminProductRetired _adminProductRetired;

        private MyDesign _myDesign;
        private OneDesign _oneDesign;

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
                PerformToPageType(step);
                CheckingExpectation(step);
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
                PerformToPageType(step);
                CheckingExpectation(step);
            }
        }

        [Test]
        [TestCaseSource(typeof(DataDriven), "MigrationControl")]
        public void MigrationControlOfDataDriven(TestCase testcase)
        {
            UserSiteLoginStep();

            _myDesign.CreateDesign();

            var documentBefore = _userContentStart.SearchDocument(testcase.ProductName);
            Assert.IsTrue(!string.IsNullOrEmpty(documentBefore.Id));
            _userContentStart.SelectDocument(documentBefore.Id);

            foreach (var step in testcase.Steps)
            {
                PerformToPageType(step);
                CheckingExpectation(step);
            }
        }

        [Test]
        public void VerifyContentFromPdf()
        {
            var filePath = @"D:\Project\Practice\GdmTest\generateddocumenttest\GenerateDocument.Test\Content\imageTest\D-F6117503_00001.pdf";
            var text = FilesHelper.ExtractTextFromPdf(filePath);
        }

        private void AdminCustomA4PosterProduct(string productName)
        {
            _adminProducts.Open();

            _adminProducts.GoToAdminProductDetails(productName);

            _adminProductDetails.ClickToFormFilling();

            _adminOptionSet.ClickToFooterLink();

            _adminOptionField.ClickToSelectLogoAndSupportingLogos();

            _adminOptionField.ClickToUpdateSetting();

            _adminOptionSet.ClickToGoback();

            _adminProductDetails.ClickToRelease();

            _adminProductRetired.ClickToUpdateSetting();
        }

        //Need to improve
        private void PerformToPageType(Step step)
        {
            var currentPage = step.Action.Equals("navigate")? step.ControlValue: DriverContext.Driver.GetCurrentPage()?.Split('.')[0];
         
            Enum.TryParse(currentPage, out PageTypes pageTypes);
            
            switch (pageTypes)
            {
                case PageTypes.UserEditFormFilling:
                    _userEditFormFilling.PerformToControlType(step);
                    break;

                case PageTypes.UserEditPrinting:
                    _userEditPrinting.PerformToControlType(step);
                    break;

                case PageTypes.UserEditFinish:
                    _userEditFinish.PerformToControlType(step);
                    break;

                case PageTypes.AdminProducts:
                    break;
            }
        }

        private void CheckingExpectation(Step step)
        {
            if (!step.Expectations.Any())
            {
                return;
            }

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
