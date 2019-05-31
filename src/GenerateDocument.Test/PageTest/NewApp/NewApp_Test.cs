﻿using GenerateDocument.Common.Extensions;
using GenerateDocument.Common.Helpers;
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
using GenerateDocument.Common.Types;

namespace GenerateDocument.Test.PageTest.NewApp
{
    [TestFixture("Chrome")]
    public partial class NewApp_Test : PageTestBase
    {
        private PageCommonAction _action;
        private MyDesign _myDesign;
        private OneDesign _oneDesign;
        private UserContentStart _userContentStart;
        private UserEditFormFilling _userEditFormFilling;
        private UserEditPrinting _userEditPrinting;
        private UserEditFinish _userEditFinish;
        private UserContentApprovals _userContentApprovals;
        private UserContentApprovalsReview _userContentApprovalsReview;
        private AdminLogin _adminLogin;
        private AdminProducts _adminProducts;
        private AdminProductDetails _adminProductDetails;

        private string _designNamePrefix = string.Empty;
        private bool _isShowSurveyInvitationModal = true;
        private bool _isShowPolicyAgreementModal = true;

        public NewApp_Test(string environment) : base(environment)
        {
        }

        [SetUp]
        public void StartTestCase()
        {
            _designNamePrefix = Guid.NewGuid().ToString();

            _action = new PageCommonAction(DriverContext);
            _myDesign = new MyDesign(DriverContext);
            _userContentStart = new UserContentStart(DriverContext);
            _userEditFormFilling = new UserEditFormFilling(DriverContext);
            _userEditPrinting = new UserEditPrinting(DriverContext);
            _userEditFinish = new UserEditFinish(DriverContext);
            _oneDesign = new OneDesign(DriverContext);
            _userContentApprovals = new UserContentApprovals(DriverContext);
            _userContentApprovalsReview = new UserContentApprovalsReview(DriverContext);
            _adminLogin = new AdminLogin(DriverContext);
            _adminProducts = new AdminProducts(DriverContext);
            _adminProductDetails = new AdminProductDetails(DriverContext);
        }

        [TearDown]
        public void StopTestCase()
        {
            DriverContext.Driver.DeleteAllCookies();
            DriverContext.Driver.ClearAllSessionStorage();
        }

        private static IEnumerable<object> WorkflowTestResources(bool hasApprovalWorkflow, bool? isApproved = null)
        {

            if (hasApprovalWorkflow)
            {
                if (isApproved == null || (isApproved != null && (bool)isApproved))
                {
                    yield return new object[]
                    {
                        "[AUTOTEST][AWF] Business cards",
                        new Dictionary<string, bool>
                        {
                            ["IsApproved"] = true,
                            ["IsKit"] = false,

                        },
                    };
                }

                if (isApproved == null || (isApproved != null && !(bool)isApproved))
                {
                    yield return new object[]
                    {
                        "[AUTOTEST][AWF][REJECTED] Business cards",
                        new Dictionary<string, bool>
                        {
                            ["IsApproved"] = false
                        },
                    };
                }
            }
            else
            {
                yield return new object[]
                {
                    "[AUTOTEST][SWF] A4 Poster",
                    new Dictionary<string, bool>
                    {
                        ["IsKit"] = false
                    },
                };

                yield return new object[]
                {
                    "[AUTOTEST][SWF][KIT] Invitation + A4 Poster",
                    new Dictionary<string, bool>
                    {
                        ["IsKit"] = true
                    },
                };
            }
        }

        private void LoginStep()
        {
            var userLogin = new UserLogin(DriverContext)
                   .NavigateTo()
                   .LoginSystem(ProjectBaseConfiguration.UserId, ProjectBaseConfiguration.UserPassword);
            
            DriverContext.Driver.WaitUntilPresentedUrl(PageTypes.Designs.ToString());

            Assert.IsTrue(userLogin.CheckAfterLoginPage(PageTypes.Designs.ToString()));
        }

        private void CreateDocumentStep(string name, bool checkInvalidDescription = false)
        {
            Assert.IsTrue(DriverContext.Driver.IsUrlEndsWith("designs"), "User is navigated to My Designs page after logged in");

            _action.CreateMopinionCookie();

            _myDesign.CreateDesign();

            Assert.IsTrue(DriverContext.Driver.IsUrlEndsWith("usercontentstart"), "User should be able to access to User Content Start page when click to Create design button");

            if (_isShowPolicyAgreementModal)
            {
                _userContentStart.AgreeWithPrivacy();

                _isShowPolicyAgreementModal = false;
            }

            _userContentStart.ClickToCreateDesign(name);

            var displayedTextareas = _userEditFormFilling.GetAllInputFields();
            if (displayedTextareas == null)
            {
                _userEditFormFilling.ClickToViewTextOptions();

                displayedTextareas = _userEditFormFilling.GetAllInputFields();
            }

            foreach (var item in displayedTextareas)
            {
                var textValue = NameHelper.RandomName(10);

                item.Clear();
                item.SendKeys(textValue);
                item.SendKeys(Keys.Tab);
            }

            _userEditFormFilling.ClickToNextStep();

            _userEditPrinting.ClickToNextStep();

            var designName = $"{name}_{_designNamePrefix}";

            if (checkInvalidDescription)
            {
                void verifyInvalidDescription(string description, string validationMessage)
                {
                    _userEditFinish.EnterOrderName(description);

                    var errorMessage = _userEditFinish.GetErrorMessage();

                    Assert.IsTrue(errorMessage.IsEquals(validationMessage), $"The error message should be {validationMessage}; but actual is {errorMessage}");
                }

                verifyInvalidDescription(designName, "This name is being used by another design");

                var maxLength = 200;

                var longDesignNameToFail = $"{designName}{NameHelper.RandomName(maxLength)}";

                verifyInvalidDescription(longDesignNameToFail, $"Design name should not exceed {maxLength} characters");

                //valid description

                designName = $"[RENAME]{name}_{_designNamePrefix}";
            }

            _userEditFinish.EnterOrderName(designName);
            _userEditFinish.ClickToFinishDesign();

            Assert.IsTrue(DriverContext.Driver.IsUrlEndsWith("onedesign"), "User is navigated to One design page once process finished");
        }

        private void EditDocumentStep(string designName)
        {
            _myDesign.ClickToEditDesign(designName);

            Assert.IsTrue(DriverContext.Driver.IsUrlEndsWith("usereditformfilling"), "User should edit design by clicking to edit button");

            var displayedTextareas = _userEditFormFilling.GetAllInputFields();
            if (displayedTextareas == null)
            {
                _userEditFormFilling.ClickToViewTextOptions();

                displayedTextareas = _userEditFormFilling.GetAllInputFields();
            }

            foreach (var item in displayedTextareas)
            {
                var textValue = NameHelper.RandomName(10);

                item.Clear();
                item.SendKeys(textValue);
                item.SendKeys(Keys.Tab);
            }

            _userEditFormFilling.ClickToNextStep();

            _userEditPrinting.ClickToNextStep();

            _userEditFinish.ClickToFinishDesign();

            Assert.IsTrue(DriverContext.Driver.IsUrlEndsWith("onedesign"), "User is navigated to One design page once process finished");
        }

        private void SubmitForApprovalStep(string name)
        {
            //_myDesign.GoToActions(name);

            var designName = _oneDesign.GetDesignName(name);

            Assert.IsTrue(!string.IsNullOrEmpty(designName) && designName.IsEquals(name), "Design name is displayed correctly");

            _oneDesign.SubmitApprovalWorkflow();
        }

        private void ReviewDesignIfHasApprovalWorkflow(string designName, bool isApproved)
        {
            _userContentApprovals.NavigateTo();

            _userContentApprovals.ReviewDesign(designName);

            _userContentApprovalsReview.HandleApprovalProcess(isApproved);
        }

        private void PlaceOrderStep(string name, bool isKit, bool needToPublishFirst = true)
        {
            var designName = _oneDesign.GetDesignName();
            Assert.IsTrue(!string.IsNullOrEmpty(designName) && designName.IsEquals(name), "Design name is displayed correctly");

            var countButtons = _oneDesign.GetDownloadDesignButtons().Count;
            Assert.IsTrue(countButtons >= 1, "Download options should be available");

            FilesHelper.DeleteAllFiles(ProjectBaseConfiguration.DownloadFolder);
            _oneDesign.DownloadDesign(needToPublishFirst, ref _isShowSurveyInvitationModal);

            var downloadedFilesCount = FilesHelper.CountFiles(ProjectBaseConfiguration.DownloadFolder);
            Assert.IsTrue(downloadedFilesCount == countButtons, $"Files downloaded successfully; files count: {downloadedFilesCount}");

            var componentName = $"{_oneDesign.GetSelectedComponentName(isKit)}";

            var namePart = isKit ? $"{name}_{componentName}_" : $"{name}_";

            var downloadedFileNames = _oneDesign.GetDownloadDesignButtons().Select(x => $"{namePart}{x.Text}".CorrectFileNameOnWindows().Trim()).ToArray();

            Assert.IsTrue(VerifyDownloadFileNames(downloadedFileNames));
        }

        private void VerifyDesignType(string designName, bool isKit)
        {
            if (!DriverContext.Driver.IsUrlEndsWith("designs"))
            {
                _myDesign.NavigateTo();
            }

            if (isKit)
            {
                var type = _myDesign.GetDesignType(designName);
                Assert.IsTrue(type.IsEquals("pack"), "Document showed caption has PACK if it's Kit type");
            }
        }

        private void VerifyDesignStatus(string designName, bool? ifUnpublished = null, bool? ifApproved = null, bool? ifUnreviewed = null, bool? ifRejected = null)
        {
            _myDesign.NavigateTo();

            var status = _myDesign.GetDesignStatus(designName);

            if (ifUnpublished ?? false)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(status) && status.IsEquals("unpublished"), "Design must be mark as Unpublished before place order");
            }
            else if (ifUnreviewed ?? false)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(status) && status.IsEquals("unreviewed"), "Design must be mark as Unreviewed when waited for approval");
            }
            else if (ifApproved ?? false)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(status) && status.IsEquals("approved"), "Design must be mark as Approved if design has been approved");
            }
            else if (ifRejected ?? false)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(status) && status.IsEquals("rejected"), "Design must be mark as Rejected if design has been rejected");
            }
            else
            {
                Assert.IsTrue(string.IsNullOrEmpty(status), "No status displayed after placed order");
            }
        }

        private void VerifyRenameAction(string newName, string expectedValidationMessage = "")
        {
            var validationMessage = _oneDesign.RenameDesign(newName);

            var encodedName = newName.Replace("<", "&lt;").Replace(">", "&gt;");

            var newDesignName = _oneDesign.GetDesignName(encodedName);

            if (string.IsNullOrEmpty(expectedValidationMessage))
            {
                Assert.IsTrue(newDesignName.IsEquals(encodedName), $"The design was rename to new name: {encodedName} successfully");
            }
            else
            {
                Assert.IsTrue(validationMessage.IsEquals(expectedValidationMessage));
            }
        }

        private void VerifyShowFullLessDesignNameAction(bool expectation)
        {
            var isVisibilityLinkShowLess = _oneDesign.CheckShowFullLessDesignNameActionLinkVisibility(expectation);

            if (isVisibilityLinkShowLess)
            {
                Assert.IsTrue(isVisibilityLinkShowLess);

                var isCorrectBehaviors = _oneDesign.CheckShowFullLessDesignNameActionLinkBehaviors(expectation);
                Assert.IsTrue(isCorrectBehaviors);
            }
            else
            {
                Assert.IsFalse(isVisibilityLinkShowLess);
            }
        }

        private void AdminLoginStep(Account account)
        {
            _adminLogin.NavigateTo();
            _adminLogin.LoginSystem(account.UserName, account.Password);
        }

        private bool VerifyDownloadFileNames(string[] expectedFileNames)
        {
            var fileNames = new DirectoryInfo(ProjectBaseConfiguration.DownloadFolder).GetFiles()
                .Where(x => new[] { ".jpg", ".pdf" }.Contains(x.Extension.ToLower())).Select(x => Path.GetFileNameWithoutExtension(x.Name).Trim());

            var result = fileNames.All(x => expectedFileNames.Contains(x)) && expectedFileNames.All(x => fileNames.Contains(x));

            return result;
        }



    }
}