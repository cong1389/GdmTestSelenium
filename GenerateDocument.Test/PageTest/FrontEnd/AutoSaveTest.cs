using GenerateDocument.Common.Extensions;
using GenerateDocument.Common.Helpers;
using GenerateDocument.Test.DataProviders;
using GenerateDocument.Test.PageObjects;
using GenerateDocument.Test.PageObjects.BackEnd;
using GenerateDocument.Test.PageObjects.FrontEnd;
using GenerateDocument.Test.PageObjects.NewApp;
using GenerateDocument.Test.Utilities;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GenerateDocument.Domain.AutoSaves;

namespace GenerateDocument.Test.PageTest.FrontEnd
{
    [TestFixture]
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

        private const string ImageFolderRelativePath = @"Content\imageTest";
        private const string DesignOptionImage = "starbucks.jpeg";
        private const string FooterOptionFirstImage = "starbucks.jpeg";
        private const string FooterOptionSecondImage = "pepsi.png";
        private const string ImageOptionFirstImage = "bg1.jpg";
        private const string ImageOptionSecondImage = "bg2.jpg";

        public const string WarningMessageExpect = "This name is being used by another design";

        [SetUp]
        public void SetUpOneTestCase()
        {
            _action = new PageCommonAction(DriverContext.Driver);
            _userContentStart = new UserContentStart(DriverContext.Driver);
            _userEditFormFilling = new UserEditFormFilling(DriverContext);
            _userEditPrinting = new UserEditPrinting(DriverContext);
            _userEditFinish = new UserEditFinish(DriverContext);
            _adminProducts = new AdminProducts(DriverContext.Driver);
            _adminProductDetails = new AdminProductDetails(DriverContext.Driver);
            _adminOptionSet = new AdminOptionSet(DriverContext.Driver);
            _adminOptionField = new AdminOptionField(DriverContext.Driver);
            _adminProductRetired = new AdminProductRetired(DriverContext.Driver);
            _adminExtensions = new AdminExtensions(DriverContext.Driver);

            _myDesign = new MyDesign(DriverContext);
            _oneDesign = new OneDesign(DriverContext);
        }

        private static IEnumerable<Dictionary<string, string>> SetupProductData()
        {
            yield return new Dictionary<string, string>
            {
                ["ProductName"] = "[AUTOTEST][SWF] A4 Poster",
                ["ProductDescription"] = NameHelper.RandomName(4)
            };

            //yield return new Dictionary<string, string>
            //{
            //    ["ProductName"] = "[AUTOTEST][SWF] A4 Poster",
            //    ["ProductDescription"] = $"<script>alert('lll' {TestUtil.RandomName(5)}ttribute=\")</ script > valid attribute = \" <valid><!-- \"'<>& --></valid><valid>"
            //};

            //yield return new Dictionary<string, string>
            //{
            //    ["ProductName"] = "[AUTOTEST][SWF] A4 Poster",
            //    ["ProductDescription"] = $"<valid><!-- <{TestUtil.RandomName(5)}>& --></valid>"
            //};

            //yield return new Dictionary<string, string>
            //{
            //    ["ProductName"] = "[AUTOTEST][SWF] A4 Poster",
            //    ["ProductDescription"] = $"<valid><![CDATA[\"'<{TestUtil.RandomName(5)}>&]]></valid>"
            //};
        }

        [Test, TestCaseSource(nameof(SetupProductData))]
        public void HappyCase_SendTextAndUploadImage_InputSaved(Dictionary<string, string> setupProduct)
        {
            AdminSiteSwitchAutoSave(() => AdminConfigExtension(), () => AdminCustomA4PosterProduct(setupProduct["ProductName"]));//TODO

            UserSiteLoginStep();

            _myDesign.CreateDesign();

            var documentBefore = _userContentStart.SearchDocument(setupProduct["ProductName"]);
            Assert.IsTrue(!string.IsNullOrEmpty(documentBefore.Id));
            _userContentStart.SelectDocument(documentBefore.Id);

            CheckAutoSaveForViewDesignOptions();
            CheckAutoSaveForViewTextOptions();
            CheckAutoSaveForViewFooterOptions();

            _userEditFormFilling.ClickToNextStep();
            _userEditPrinting.ClickToNextStep();

            _userEditFinish.EnterOrderName(setupProduct["ProductDescription"]);
            _userEditFinish.ClickToFinishDesign();

            Assert.IsTrue(_oneDesign.GetDesignName().IsEquals(setupProduct["ProductDescription"].EncodeSpecialCharacters()));
        }

        [Test]
        public void ConditionalControls_ReloadPage_InputSaved()
        {
            string documentName = "[AUTOTEST] Social Media Post";

            Dictionary<string, string> valueInputs = new Dictionary<string, string>();
            Dictionary<string, string> valueOutputs = new Dictionary<string, string>();

            UserSiteLoginStep();

            _myDesign.CreateDesign();

            var documentBefore = _userContentStart.SearchDocument(documentName);
            Assert.IsTrue(!string.IsNullOrEmpty(documentBefore.Id));
            _userContentStart.SelectDocument(documentBefore.Id);

            _userEditFormFilling
                .ExpandOptions("Layout options", "div1")
                .SelectByValue("FIELD_1024", "Fundraiser");
            Assert.IsTrue(_action.GetNotifyMessage);

            _userEditFormFilling
                .ExpandOptions("Image and Text options", "div2")
                .SelectByValue("FIELD_1045", "Visit our Just Giving page:");
            valueInputs = _userEditFormFilling.EnteringValueInputsTextInOptions("div2", NameHelper.RandomName(10));
            Assert.IsTrue(_action.GetNotifyMessage);

            //Verify value entered when reload page
            DriverContext.Driver.RefreshPage();

            _userEditFormFilling
                .ExpandOptions("Image and Text options", "div2");
            valueOutputs = _userEditFormFilling.GetValueInputsTextInOptions("div2");

            CollectionAssert.AreEqual(valueInputs, valueOutputs, "The value inputs do not same the value outputs");

            _userEditFormFilling.ClickToNextStep();
            _userEditPrinting.ClickToNextStep();

            _userEditFinish
                .EnterOrderName(NameHelper.RandomName(10))
                .ClickToFinishDesign();
        }

        [Test]
        [TestCaseSource(typeof(DataDriven), "Conditional")]
        public void TestConditional(TestCase testcase)
        {
            UserSiteLoginStep();

            _myDesign.CreateDesign();

            var documentBefore = _userContentStart.SearchDocument(testcase.ProductName);
            Assert.IsTrue(!string.IsNullOrEmpty(documentBefore.Id));
            _userContentStart.SelectDocument(documentBefore.Id);

            foreach (var control in testcase.Controls)
            {
                _userEditFormFilling.ExpandOptions(control.GroupName, control.GroupId);
                SendKeyToControl(control, out Dictionary<string, string> valueInputs);
                Assert.IsTrue(_action.GetNotifyMessage);

                VerifyDependenciesControl(control.Dependencies);
            }

            _userEditFormFilling.ClickToNextStep();
            _userEditPrinting.ClickToNextStep();

            _userEditFinish
                .EnterOrderName(NameHelper.RandomName(10))
                .ClickToFinishDesign();

        }

        private void VerifyDependenciesControl(List<Control> dependencies)
        {
            if (!dependencies.Any())
            {
                return;
            }

            foreach (var control in dependencies)
            {
                _userEditFormFilling.ExpandOptions(control.GroupName, control.GroupId);
                SendKeyToControl(control, out Dictionary<string, string> valueInputs);

                Assert.IsTrue(_action.GetNotifyMessage);
            }
        }

        private Dictionary<string, string> SendKeyToControl(Control control, out Dictionary<string, string> valueInputs)
        {
            valueInputs = new Dictionary<string, string>();

            string value;
            switch (control.Type)
            {
                case "dropbox":
                case "listbox":
                    _userEditFormFilling.SelectByValue(control.Id, control.Value);
                    break;

                case "textbox":
                    value = string.IsNullOrEmpty(control.Value) ? NameHelper.RandomName(10) : control.Value;
                    valueInputs.Add(control.Id, value);
                    _userEditFormFilling.EnteringValueInputTextInOptions(control.Id, value);
                    break;

                case "textarea":
                    value = string.IsNullOrEmpty(control.Value) ? NameHelper.RandomName(100) : control.Value;
                    valueInputs.Add(control.Id, value);
                    _userEditFormFilling.EnteringValueInputTextInOptions(control.Id, value);
                    break;

                case "radio":
                    _userEditFormFilling.TickRadio(control.Id, control.Value);
                    break;

                case "checkbox":
                    _userEditFormFilling.TickOrUnTickCheckBox(control.Id, Boolean.Parse(control.Value));
                    break;
            }


            return valueInputs;
        }

        private void UserSiteLoginStep()
        {
            _userContentStart.NavigateTo();

            new UserLogin(DriverContext)
                .NavigateTo()
               .LoginSystem(ProjectBaseConfiguration.UserId, ProjectBaseConfiguration.UserPassword);
        }

        private void AdminSiteLoginStep()
        {
            new AdminLogin(DriverContext)
                .NavigateTo()
                .LoginSystem(ProjectBaseConfiguration.AdminId, ProjectBaseConfiguration.AdminPassword);
        }

        private void AdminSiteSwitchAutoSave(params Action[] actions)
        {
            AdminSiteLoginStep();

            foreach (var action in actions)
            {
                action.Invoke();
            }

            new AdminLogout(DriverContext)
                .ClickLogOutButton();
        }

        private void AdminConfigExtension(bool enableAutoSave = true)
        {
            _adminExtensions.Open();

            _adminExtensions.ClickGeneratedDocumentManagerLink();

            _adminExtensions.SelectAutoSaveSwitch(enableAutoSave);

            _adminExtensions.ClickUpdateSettings();
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

        private void CheckAutoSaveForViewDesignOptions()
        {
            UploadDesignOptionFile();
            //ToggleCheckRadioButton();
        }

        private void CheckAutoSaveForViewTextOptions()
        {
            _userEditFormFilling.ClickToViewTextOptions();

            var fields = new List<AutoSaveField>();
            var displayedTextareas = _userEditFormFilling.GetAllDisplayedTextareas();
            Assert.IsTrue(displayedTextareas.Any());

            foreach (var item in displayedTextareas)
            {
                var textValue = NameHelper.RandomName(10);

                item.Clear();
                item.SendKeys(textValue);
                item.SendKeys(Keys.Tab);

                Assert.IsTrue(_action.GetNotifyMessage);

                fields.Add(new AutoSaveField
                {
                    Id = item.GetAttribute("id"),
                    Value = textValue
                });
            }
        }

        private void CheckAutoSaveForViewFooterOptions()
        {
            _userEditFormFilling.ClickToViewFooterOptions();

            //Assert.IsTrue(!_userEditFormFilling.IsSupportingLogosDropdownDisplayed());
            //Assert.IsTrue(!_userEditFormFilling.IsFirstSupportingLogosSectionDisplayed());
            //Assert.IsTrue(!_userEditFormFilling.IsSecondSupportingLogoSectionDisplayed());

            _userEditFormFilling.ClickToSelectSupportingLogos();
            _userEditFormFilling.ClickToSelectTwoSupportingLogos();

            Assert.IsTrue(_userEditFormFilling.IsSupportingLogosDropdownDisplayed());
            Assert.IsTrue(_userEditFormFilling.IsFirstSupportingLogosSectionDisplayed());
            Assert.IsTrue(_userEditFormFilling.IsSecondSupportingLogoSectionDisplayed());

            var firstImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ImageFolderRelativePath, FooterOptionFirstImage);
            UploadImage(FooterOptionFirstImage
                , firstImagePath
                , () => _userEditFormFilling.ClickToUploadFirstSupportingLogo(firstImagePath)
                , () => _userEditFormFilling.ClickToViewFooterOptions(true)
                , _userEditFormFilling.GetFirstSupportingLogoValue);

            var secondImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ImageFolderRelativePath, FooterOptionSecondImage);
            UploadImage(FooterOptionSecondImage
                , secondImagePath
                , () => _userEditFormFilling.ClickToUploadSecondSupportingLogo(secondImagePath)
                , () => _userEditFormFilling.ClickToViewFooterOptions(true)
                , _userEditFormFilling.GetSecondSupportingLogoValue);
        }

        private void UploadDesignOptionFile()
        {
            _userEditFormFilling.ClickToViewDesignOptions();

            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ImageFolderRelativePath, DesignOptionImage);
            Assert.IsTrue(File.Exists(path));
            _userEditFormFilling.UploadDesignOptionFile(path);

            _userEditFormFilling.ClickToViewDesignOptions();
            var uploadedFileName = _userEditFormFilling.GetUploadLogoValue();

            var isCheckUploadedImageName = CheckUploadedImageName(uploadedFileName, DesignOptionImage);

            Assert.IsTrue(isCheckUploadedImageName);
        }

        private void UploadImageOptionFile()
        {
            _userEditFormFilling.ClickToViewImageOptions();

            var firstImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ImageFolderRelativePath, ImageOptionFirstImage);
            UploadImage(ImageOptionFirstImage, firstImagePath, () => _userEditFormFilling.ClickToUploadFirstImageOptionFile(firstImagePath), () => _userEditFormFilling.ClickToViewImageOptions(true), _userEditFormFilling.GetFirstUploadImageValue);

            var secondImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ImageFolderRelativePath, ImageOptionSecondImage);
            UploadImage(ImageOptionSecondImage, secondImagePath, () => _userEditFormFilling.ClickToUploadSecondImageOptionFile(secondImagePath), () => _userEditFormFilling.ClickToViewImageOptions(true), _userEditFormFilling.GetSecondUploadImageValue);
        }

        private void UploadImage(string fileName, string path, Action uploadImageAction, Action expandSection, Func<string> getUploadedImageName)
        {
            Assert.IsTrue(File.Exists(path));

            uploadImageAction.Invoke();

            expandSection.Invoke();

            var uploadedFileName = getUploadedImageName.Invoke();

            var isCheckUploadedImageName = CheckUploadedImageName(uploadedFileName, fileName);

            Assert.IsTrue(isCheckUploadedImageName);
        }

        private static bool CheckUploadedImageName(string uploadedImageName, string expectedImageName)
        {
            Func<string, string> getNameOfFile = (string fileName) => fileName.Substring(0, fileName.LastIndexOf("."));

            Func<string, string> getExtension = (string fileName) => fileName.Substring(fileName.LastIndexOf("."));

            var nameOfUploadedImage = getNameOfFile.Invoke(uploadedImageName);
            var nameOfExpectedImage = getNameOfFile.Invoke(expectedImageName);

            var uploadedImageExtension = getExtension.Invoke(uploadedImageName);
            var expectedImageExtension = getExtension.Invoke(expectedImageName);

            return nameOfUploadedImage.Contains(nameOfExpectedImage) && uploadedImageExtension.Equals(expectedImageExtension);
        }

        private void ToggleCheckRadioButton()
        {
            int selectIndex = SetDesignOptionLayout();

            _action.UserLogout();

            new UserLogin(DriverContext)
                .NavigateTo()
                .LoginSystem(ProjectBaseConfiguration.UserId, ProjectBaseConfiguration.UserPassword);

            var documentAfter = _userContentStart.SearchDocument(ProjectBaseConfiguration.A4PosterName);
            _userContentStart.SelectDocument(documentAfter.Id);
            Assert.IsTrue(documentAfter.Name.IsContains("In progress") && _userContentStart.IsInprogressDocument());
            _userContentStart.SelectCompleteExistingDocument();

            CheckDesignOptionLayout(selectIndex);
        }

        private int SetDesignOptionLayout()
        {
            _userEditFormFilling.ClickToViewDesignOptions();

            var designOptionLayoutRadiosBefore = _userEditFormFilling.GetDesignOptionLayoutRadios();
            Assert.IsTrue(designOptionLayoutRadiosBefore.Any());

            var index = designOptionLayoutRadiosBefore.Count / 2;

            _userEditFormFilling.SetSelectDesignOptionLayout(designOptionLayoutRadiosBefore, index);

            return index;
        }

        private void CheckDesignOptionLayout(int index)
        {
            _userEditFormFilling.ClickToViewDesignOptions();

            var designOptionLayoutRadiosAfter = _userEditFormFilling.GetDesignOptionLayoutRadios();
            Assert.IsTrue(_userEditFormFilling.CheckSelectDesignOptionLayout(designOptionLayoutRadiosAfter, index));

            _userEditFormFilling.SetSelectDesignOptionLayout(designOptionLayoutRadiosAfter, 0);
            _userEditFormFilling.ClickToViewDesignOptions(false);
        }

    }
}
