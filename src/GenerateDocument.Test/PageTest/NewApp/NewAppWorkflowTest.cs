using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GenerateDocument.Common.Types;
using GenerateDocument.Test.PageObjects;
using GenerateDocument.Test.PageObjects.BackEnd;
using GenerateDocument.Test.PageObjects.FrontEnd;
using GenerateDocument.Test.PageObjects.NewApp;
using GenerateDocument.Test.Utilities;
using GenerateDocument.Test.WrapperFactory;
using GenerateDocument.Test.Extensions;
using NUnit.Framework;
using NUnit.Framework.Internal;
using OpenQA.Selenium;

using static GenerateDocument.Test.WrapperFactory.ConfigInfo;
using static GenerateDocument.Test.Utilities.PageCommon;

namespace GenerateDocument.Test.PageTest.NewApp
{
    //[TestFixture(BrowserTypes.Firefox)]
    [TestFixture(BrowserTypes.Chrome)]
    [Ignore("Moving to NewApp_Test")]
    public partial class NewAppTest : BaseTest
    {
        private readonly PageCommonAction _action;
        private readonly UserLogin _userLoginPage;
        private readonly MyDesign _myDesign;
        private readonly OneDesign _oneDesign;
        private readonly UserContentStart _userContentStart;
        private readonly UserEditFormFilling _userEditFormFilling;
        private readonly UserEditPrinting _userEditPrinting;
        private readonly UserEditFinish _userEditFinish;
        private readonly UserContentApprovals _userContentApprovals;
        private readonly UserContentApprovalsReview _userContentApprovalsReview;
        private readonly AdminLogin _adminLogin;
        private readonly AdminProducts _adminProducts;
        private readonly AdminProductDetails _adminProductDetails;

        private const string _returnPage = "designs";

        private string _designNamePrefix = string.Empty;
        private bool _isShowSurveyInvitationModal = true;
        private bool _isShowPolicyAgreementModal = true;

        private IDictionary<string, string[]> _designsByStatuses = new Dictionary<string, string[]>();

        public NewAppTest(BrowserTypes browserTypes) : base(browserTypes)
        {
            _action = new PageCommonAction(_browser);
            _userLoginPage = new UserLogin(_browser);
            _myDesign = new MyDesign(_browser);
            _userContentStart = new UserContentStart(_browser);
            _userEditFormFilling = new UserEditFormFilling(_browser);
            _userEditPrinting = new UserEditPrinting(_browser);
            _userEditFinish = new UserEditFinish(_browser);
            _oneDesign = new OneDesign(_browser);
            _userContentApprovals = new UserContentApprovals(_browser);
            _userContentApprovalsReview = new UserContentApprovalsReview(_browser);
            _adminLogin = new AdminLogin(_browser);
            _adminProducts = new AdminProducts(_browser);
            _adminProductDetails = new AdminProductDetails(_browser);

            _designNamePrefix = Guid.NewGuid().ToString();
        }

        private static IEnumerable<Dictionary<string, string>> SetupProductData(bool isLongDesign)
        {
            if (isLongDesign)
            {
                yield return new Dictionary<string, string>
                {
                    ["ProductName"] = "[AUTOTEST][SWF] A4 Poster",
                    ["ProductDescription"] = $"[AUTOTEST][SWF][KIT] Invitation + A4 Poster_{ TestUtil.RandomName(5)}"
                };
            }
            else
            {
                yield return new Dictionary<string, string>
                {
                    ["ProductName"] = "[AUTOTEST][SWF] A4 Poster",
                    ["ProductDescription"] = TestUtil.RandomName(4)
                };
            }
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

        private static IEnumerable<string> DesignStatuses => new List<string> { "Unpublished", "Unreviewed", "Rejected", "Shipped" };

        [Test, Order(1), TestCaseSource(nameof(WorkflowTestResources), new object[] { false, null })]
        public void Design_SuccessfullyCreated_FromSingleDocumentOrKitDocument(string templateName, Dictionary<string, bool> settings)
        {
            LoginStep(_returnPage);

            CreateDocumentStep(templateName);
        }

        [Test, Order(2), TestCaseSource(nameof(WorkflowTestResources), new object[] { true, null })]
        public void Design_SuccessfullyCreated_FromDocumentHasApprovalWorkflow(string templateName, Dictionary<string, bool> settings)
        {
            LoginStep(_returnPage);

            CreateDocumentStep(templateName);
        }

        [Test, Order(3), TestCaseSource(nameof(WorkflowTestResources), new object[] { false, null })]
        public void DesignType_ShouldDisplayedCorrectly_WhenItIsSingleDocumentOrKitDocument(string templateName, Dictionary<string, bool> settings)
        {
            LoginStep(_returnPage);

            VerifyDesignType($"{templateName}_{_designNamePrefix}", settings["IsKit"]);
        }

        [Test, Order(4), TestCaseSource(nameof(WorkflowTestResources), new object[] { false, null })]
        public void DesignStatus_IsUnpublished_BeforePlaceOrder(string templateName, Dictionary<string, bool> settings)
        {
            LoginStep(_returnPage);

            VerifyDesignStatus($"{templateName}_{_designNamePrefix}", ifUnpublished: true);
        }

        [Test, Order(5), TestCaseSource(nameof(WorkflowTestResources), new object[] { true, null })]
        public void DesignStatus_IsUnpublished_BeforeSubmitForApproval(string templateName, Dictionary<string, bool> settings)
        {
            LoginStep(_returnPage);

            VerifyDesignStatus($"{templateName}_{_designNamePrefix}", ifUnpublished: true);
        }

        [Test, Order(6), TestCaseSource(nameof(WorkflowTestResources), new object[] { true, null })]
        public void DesignStatus_IsUnreviewed_WhenWaitingForApproval(string templateName, Dictionary<string, bool> settings)
        {
            LoginStep(_returnPage);

            SubmitForApprovalStep($"{templateName}_{_designNamePrefix}");

            VerifyDesignStatus($"{templateName}_{_designNamePrefix}", ifUnreviewed: true);
        }

        [Test, Order(7), TestCaseSource(nameof(WorkflowTestResources), new object[] { true, true })]
        public void DesignStatus_IsApproved_IfHasBeenApproved(string templateName, Dictionary<string, bool> settings)
        {
            LoginStep(_returnPage);
            CreateDocumentStep(templateName);
            SubmitForApprovalStep($"{templateName}_{_designNamePrefix}");
            ReviewDesignIfHasApprovalWorkflow($"{templateName}_{_designNamePrefix}", settings["IsApproved"]);

            VerifyDesignStatus($"{templateName}_{_designNamePrefix}", ifApproved: settings["IsApproved"]);
        }

        [Test, Order(8), TestCaseSource(nameof(WorkflowTestResources), new object[] { true, false })]
        public void DesignStatus_IsRejected_IfHasBeenRejected(string templateName, Dictionary<string, bool> settings)
        {
            LoginStep(_returnPage);

            ReviewDesignIfHasApprovalWorkflow($"{templateName}_{_designNamePrefix}", false);

            VerifyDesignStatus($"{templateName}_{_designNamePrefix}", ifRejected: true);
        }

        [Test, Order(9), TestCaseSource(nameof(WorkflowTestResources), new object[] { false, null })]
        public void DesignOutputs_SuccessfullyPlacedOrderAndDownloaded_IfDocumentHasNotApprovalWorkflow(string templateName, Dictionary<string, bool> settings)
        {
            try
            {
                LoginStep(_returnPage);

                PlaceOrderStep($"{templateName}_{_designNamePrefix}", settings["IsKit"]);
            }
            finally
            {
                var dir = new DirectoryInfo(NewAppTestDir);
                foreach (var file in dir.GetFiles())
                {
                    file.Delete();
                }
            }
        }

        [Test, Order(10), TestCaseSource(nameof(WorkflowTestResources), new object[] { true, true })]
        public void DesignOutputs_SuccessfullyDownloaded_IfStatusIsApproved(string templateName, Dictionary<string, bool> settings)
        {
            try
            {
                LoginStep(_returnPage);

                PlaceOrderStep($"{templateName}_{_designNamePrefix}", settings["IsKit"], false);
            }
            finally
            {
                var dir = new DirectoryInfo(NewAppTestDir);
                foreach (var file in dir.GetFiles())
                {
                    file.Delete();
                }
            }
        }

        [Test, Order(11), TestCaseSource(nameof(WorkflowTestResources), new object[] { false, null })]
        public void DesignOutputs_SuccessfullyDownloaded_WithoutConfirm_WhenProductHadNewReleaseWithMaintainOriginalOption(string templateName, Dictionary<string, bool> settings)
        {
            try
            {
                AdminLoginStep(AdminAccount);

                _adminProducts.Open();

                _adminProducts.GoToAdminProductDetails(templateName);

                _adminProductDetails.ClickToChangeSetting();

                _adminProductDetails.UpdateSettings();

                _adminProductDetails.ClickToRelease();

                _adminProductDetails.UpdateForMaintainOnly();

                _adminProductDetails.UpdateSettings(true);

                LoginStep(_returnPage);

                VerifyDesignStatus($"{templateName}_{_designNamePrefix}");

                PlaceOrderStep($"{templateName}_{_designNamePrefix}", settings["IsKit"], false);
            }
            finally
            {
                var dir = new DirectoryInfo(NewAppTestDir);
                foreach (var file in dir.GetFiles())
                {
                    file.Delete();
                }
            }
        }

        [Test, Order(12), TestCaseSource(nameof(WorkflowTestResources), new object[] { false, null })]
        public void DesignOutputs_SuccessfullyPlacedOrderAndDownloaded_WhenProductHadNewReleaseWithConvertToLatestOption(string templateName, Dictionary<string, bool> settings)
        {
            try
            {
                AdminLoginStep(AdminAccount);

                _adminProducts.Open();

                _adminProducts.GoToAdminProductDetails(templateName);

                _adminProductDetails.ClickToChangeSetting();

                _adminProductDetails.UpdateSettings();

                _adminProductDetails.ClickToRelease();

                _adminProductDetails.UpdateAndConvertToLatestRelease();

                _adminProductDetails.UpdateSettings(true);

                LoginStep(_returnPage);

                VerifyDesignStatus($"{templateName}_{_designNamePrefix}");

                PlaceOrderStep($"{templateName}_{_designNamePrefix}", settings["IsKit"]);
            }
            finally
            {
                var dir = new DirectoryInfo(NewAppTestDir);
                foreach (var file in dir.GetFiles())
                {
                    file.Delete();
                }
            }
        }

        [Test, Order(13), TestCaseSource(nameof(WorkflowTestResources), new object[] { false, null })]
        public void DesignOutputs_SuccessfullyPlacedOrderAndDownloaded_AfterEdited(string templateName, Dictionary<string, bool> settings)
        {
            try
            {
                LoginStep(_returnPage);

                EditDocumentStep($"{templateName}_{_designNamePrefix}");

                PlaceOrderStep($"{templateName}_{_designNamePrefix}", settings["IsKit"]);
            }
            finally
            {
                var dir = new DirectoryInfo(NewAppTestDir);
                foreach (var file in dir.GetFiles())
                {
                    file.Delete();
                }
            }
        }

        [Test, Order(14), TestCaseSource(nameof(WorkflowTestResources), new object[] { false, null })]
        public void Design_CopyAction_IsNotAbleToDo_IfRetired(string templateName, Dictionary<string, bool> settings)
        {
            //step 1: prepare product from admin
            AdminLoginStep(AdminAccount);
            _adminProducts.Open();
            _adminProducts.CloneToNewProduct(templateName, RetiredPrefix);

            //step 2: user create design
            LoginStep(_returnPage);
            CreateDocumentStep($"{RetiredPrefix}{templateName}");

            //step 3: admin retired this product
            AdminLoginStep(AdminAccount);
            _adminProducts.Open();
            _adminProducts.RetireProduct($"{RetiredPrefix}{templateName}");

            //step 4: verify design actions
            LoginStep(_returnPage);

            var label = _myDesign.GetRetiredOrDeletedLabel($"{RetiredPrefix}{templateName}_{_designNamePrefix}");

            Assert.IsTrue(label.IsEquals("product retired"), "This design must be mark as Product Retired");

            var cloneable = _myDesign.CheckDesignCloneable($"{RetiredPrefix}{templateName}_{_designNamePrefix}");

            Assert.IsFalse(cloneable, "Can not clone new design from design when the product has been retired");
        }

        [Test, Order(15), TestCaseSource(nameof(WorkflowTestResources), new object[] { false, null })]
        public void Design_EditAction_IsNotAbleToDo_IfRetired(string templateName, Dictionary<string, bool> settings)
        {
            LoginStep(_returnPage);

            var editable = _myDesign.CheckDesignEditable($"{RetiredPrefix}{templateName}_{_designNamePrefix}");

            Assert.IsFalse(editable, "Can not edit when the product has been retired");
        }

        [Test, Order(16), TestCaseSource(nameof(WorkflowTestResources), new object[] { false, null })]
        public void Design_CopyAction_IsNotAbleToDo_IfDeleted(string templateName, Dictionary<string, bool> settings)
        {
            AdminLoginStep(AdminAccount);
            _adminProducts.Open();
            _adminProducts.DeleteProduct($"{RetiredPrefix}{templateName}*");

            LoginStep(_returnPage);

            var label = _myDesign.GetRetiredOrDeletedLabel($"{RetiredPrefix}{templateName}_{_designNamePrefix}");

            Assert.IsTrue(label.IsEquals("product deleted"), "This design must be mark as Product Deleted");

            var cloneable = _myDesign.CheckDesignCloneable($"{RetiredPrefix}{templateName}_{_designNamePrefix}");

            Assert.IsFalse(cloneable, "Can not clone new design from design when the product has been deleted");
        }

        [Test, Order(17), TestCaseSource(nameof(WorkflowTestResources), new object[] { false, null })]
        public void Design_ActionPage_IsNotAbleToGo_IfDeleted(string templateName, Dictionary<string, bool> settings)
        {
            LoginStep(_returnPage);

            var accessible = _myDesign.CheckDesignCanGoToActionPage($"{RetiredPrefix}{templateName}_{_designNamePrefix}");

            Assert.IsFalse(accessible, "Design can not view details when the product has been deleted");
        }

        [Test, Order(18), TestCaseSource(nameof(WorkflowTestResources), new object[] { false, null })]
        public void Design_EditAction_IsNotAbleToDo_IfDeleted(string templateName, Dictionary<string, bool> settings)
        {
            LoginStep(_returnPage);

            var editable = _myDesign.CheckDesignEditable($"{RetiredPrefix}{templateName}_{_designNamePrefix}");

            Assert.IsFalse(editable, "Can not edit design when the product has been deleted");
        }

        [Test, Order(19), TestCaseSource(nameof(WorkflowTestResources), new object[] { false, null })]
        public void Design_CopyAction_IsAbleToDo_WhenItIsSingleDocumentOrKitDocument(string templateName, Dictionary<string, bool> settings)
        {
            LoginStep(_returnPage);

            var designName = $"{templateName}_{_designNamePrefix}";

            var copiedDesignName = $"[COPIED]{designName}";

            Assert.IsTrue(_myDesign.CheckDesignCloneable(designName) && _myDesign.DoCopyDesign(designName, copiedDesignName), "It is able to clone new design from single design or kit design");
        }

        [Test, Order(20), TestCaseSource(nameof(WorkflowTestResources), new object[] { true, null })]
        public void Design_CopyAction_IsAbleToDo_FromDocumentHasApprovalWorkflow(string templateName, Dictionary<string, bool> settings)
        {
            LoginStep(_returnPage);

            var designName = $"{templateName}_{_designNamePrefix}";

            var copiedDesignName = $"[COPIED]{designName}";

            Assert.IsTrue(_myDesign.CheckDesignCloneable(designName) && _myDesign.DoCopyDesign(designName, copiedDesignName), "It is able to clone new design when design has approval workflow");
        }

        [Test, Order(21), TestCaseSource(nameof(WorkflowTestResources), new object[] { true, true })]
        public void DesignName_DoNotAllowDuplicate_Or_ExceedMaxlength_WhenCreating(string templateName, Dictionary<string, bool> settings)
        {
            LoginStep(_returnPage);

            CreateDocumentStep(templateName, true);
        }

        [Test, Order(22), TestCaseSource(nameof(WorkflowTestResources), new object[] { true, true })]
        public void DesignName_DoNotAllowDuplicate_Or_ExceedMaxlength_WhenRenaming(string templateName, Dictionary<string, bool> settings)
        {
            var designName = $"[RENAME]{templateName}_{_designNamePrefix}";
            //for debug only
            //LoginStep(_returnPage);
            //_myDesign.CheckGoToActions(designName);
            //end for debug

            VerifyRenameAction(string.Empty, "Please enter design name");

            VerifyRenameAction(designName, "Please enter a different name");

            var longDesriptionExceedMaxLength = designName + TestUtil.RandomName(200);

            VerifyRenameAction(longDesriptionExceedMaxLength, "Design name should not exceed 200 characters");
        }

        [Test, Order(23), TestCaseSource(nameof(WorkflowTestResources), new object[] { true, true })]
        public void DesignName_SuccessfullyRenamed_IfValidName(string templateName, Dictionary<string, bool> settings)
        {
            var designName = $"[RENAME]{templateName}_{_designNamePrefix}";
            var newDesignName = $"{templateName}_{Guid.NewGuid().ToString()}";

            VerifyRenameAction(newDesignName);

            SubmitForApprovalStep(newDesignName);

            var newDesignNameForSecondTimeRename = $"{templateName}_{Guid.NewGuid().ToString()}";

            VerifyRenameAction(newDesignNameForSecondTimeRename);

            ReviewDesignIfHasApprovalWorkflow(newDesignNameForSecondTimeRename, settings["IsApproved"]);

            VerifyDesignStatus(newDesignNameForSecondTimeRename, ifApproved: settings["IsApproved"]);

            _myDesign.CheckGoToActions(newDesignNameForSecondTimeRename);

            _designNamePrefix = Guid.NewGuid().ToString();

            var newDesignNameForThirdTimeRename = $"{templateName}_{_designNamePrefix}";

            VerifyRenameAction(newDesignNameForThirdTimeRename);
        }

        [Test, Order(24), TestCaseSource(nameof(WorkflowTestResources), new object[] { true, true })]
        public void DesignName_SuccessfullyReplaced_AngleBrackets(string templateName, Dictionary<string, bool> settings)
        {
            var designName = $"[RENAME]{templateName}_{_designNamePrefix}";
            var newDesignName = $"{designName}_<script>alert('{TestUtil.RandomName(5)}');</script>";

            VerifyRenameAction(newDesignName);
        }

        [Test, Order(25), TestCaseSource(nameof(WorkflowTestResources), new object[] { true, true })]
        public void DesignName_TruncatedAutomatically_And_AbleToShowFullOrLess_IfItIsLongText(string templateName, Dictionary<string, bool> settings)
        {
            VerifyShowFullLessDesignNameAction(true);
        }

        [Test, Order(26), TestCaseSource(nameof(WorkflowTestResources), new object[] { true, true })]
        public void DesignName_IsNotAbleToShowFullOrLess_IfItIsShortText(string templateName, Dictionary<string, bool> settings)
        {
            //rename to short text
            var shortDesignName = TestUtil.RandomName(5);

            VerifyRenameAction(shortDesignName);

            VerifyShowFullLessDesignNameAction(false);
        }

        [Test, Order(27), TestCaseSource(nameof(WorkflowTestResources), new object[] { true, true })]
        public void DesignName_IsAbleToShowLessImmediately_IfRenameToLongText(string templateName, Dictionary<string, bool> settings)
        {
            var longDesignName = TestUtil.RandomName(200);

            VerifyRenameAction(longDesignName);

            VerifyShowFullLessDesignNameAction(true);
        }

        private void LoginStep(string returnPage = "")
        {
            _myDesign.Open();

            if (_action.NeedToLogin())
            {
                _userLoginPage.Open();

                _userLoginPage.LoginSystem();

                if (!string.IsNullOrEmpty(returnPage))
                {
                    _action.MakesureNavigateToReturnUrlAfterLogin(returnPage);
                }
            }
        }

        private void CreateDocumentStep(string name, bool checkInvalidDescription = false)
        {
            Assert.IsTrue(_browser.CurrentPageIs("designs"), "User is navigated to My Designs page after logged in");

            _action.CreateMopinionCookie();

            _myDesign.CreateDesign();

            Assert.IsTrue(_browser.CurrentPageIs("usercontentstart"), "User should be able to access to User Content Start page when click to Create design button");

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
                var textValue = TestUtil.RandomName(10);

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

                var longDesignNameToFail = $"{designName}{TestUtil.RandomName(maxLength)}";

                verifyInvalidDescription(longDesignNameToFail, $"Design name should not exceed {maxLength} characters");

                //valid description

                designName = $"[RENAME]{name}_{_designNamePrefix}";
            }

            _userEditFinish.ClickToGenerateDocument(designName);

            Assert.IsTrue(_browser.CurrentPageIs("onedesign"), "User is navigated to One design page once process finished");
        }

        private void EditDocumentStep(string designName)
        {
            _myDesign.ClickToEditDesign(designName);

            Assert.IsTrue(_browser.CurrentPageIs("usereditformfilling"), "User should edit design by clicking to edit button");

            var displayedTextareas = _userEditFormFilling.GetAllInputFields();
            if (displayedTextareas == null)
            {
                _userEditFormFilling.ClickToViewTextOptions();

                displayedTextareas = _userEditFormFilling.GetAllInputFields();
            }

            foreach (var item in displayedTextareas)
            {
                var textValue = TestUtil.RandomName(10);

                item.Clear();
                item.SendKeys(textValue);
                item.SendKeys(Keys.Tab);
            }

            _userEditFormFilling.ClickToNextStep();

            _userEditPrinting.ClickToNextStep();

            _userEditFinish.ClickToGenerateDocument();

            Assert.IsTrue(_browser.CurrentPageIs("onedesign"), "User is navigated to One design page once process finished");
        }

        private void SubmitForApprovalStep(string name)
        {
            _myDesign.CheckGoToActions(name);

            var designName = _oneDesign.GetDesignName(name);

            Assert.IsTrue(!string.IsNullOrEmpty(designName) && designName.IsEquals(name), "Design name is displayed correctly");

            _oneDesign.SubmitApprovalWorkflow();
        }

        private void ReviewDesignIfHasApprovalWorkflow(string designName, bool isApproved)
        {
            _userContentApprovals.Open();

            _userContentApprovals.ReviewDesign(designName);

            _userContentApprovalsReview.HandleApprovalProcess(isApproved);
        }

        private void PlaceOrderStep(string name, bool isKit, bool needToPublishFirst = true)
        {
            _myDesign.CheckGoToActions(name);

            var designName = _oneDesign.GetDesignName();

            Assert.IsTrue(!string.IsNullOrEmpty(designName) && designName.IsEquals(name), "Design name is displayed correctly");

            var downloadOptionsCount = _oneDesign.DownloadButtons().Count;

            Assert.IsTrue(downloadOptionsCount >= 1, "Download options should be available");

            _oneDesign.DownloadDesign(needToPublishFirst, ref _isShowSurveyInvitationModal);

            var downloadedFilesCount = CountDownloadFiles(downloadOptionsCount);

            Assert.IsTrue(downloadedFilesCount == downloadOptionsCount, $"Files downloaded successfully; files count: {downloadedFilesCount}");

            var componentName = $"{_oneDesign.GetSelectedComponentName(isKit)}";

            var namePart = isKit ? $"{name}_{componentName}_" : $"{name}_";

            var downloadedFileNames = _oneDesign.DownloadButtons().Select(x => $"{namePart}{x.Text}".CorrectFileNameOnWindows().Trim()).ToArray();

            Assert.IsTrue(VerifyDownloadFileNames(downloadedFileNames));
        }

        private int CountDownloadFiles(int expectNumber)
        {
            int countDownloadedFiles()
            {
                return new DirectoryInfo(NewAppTestDir).GetFiles()
                    .Count(x => new[] { ".jpg", ".pdf" }.Contains(x.Extension.ToLower()));
            }

            var count = 0;
            var i = 0;

            do
            {
                count = countDownloadedFiles();
                if (count == expectNumber)
                    break;
                i++;
                Thread.Sleep(1000);
            } while (i < 40);

            return count;
        }

        private void VerifyDesignType(string designName, bool isKit)
        {
            if (!_browser.CurrentPageIs("designs"))
            {
                _myDesign.Open();
            }

            var type = _myDesign.GetDesignType(designName);
            if (isKit)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(type) && type.IsEquals("pack"), "Document showed caption has PACK if it's Kit type");
            }
            else
            {
                Assert.IsTrue(string.IsNullOrEmpty(type), "No caption displayed if it is not Kit type");
            }
        }

        private void VerifyDesignStatus(string designName, bool? ifUnpublished = null, bool? ifApproved = null, bool? ifUnreviewed = null, bool? ifRejected = null)
        {
            _myDesign.Open();

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
            var visibility = _oneDesign.CheckShowFullLessDesignNameActionLinkVisibility(expectation);
            Assert.IsTrue(visibility);

            if (visibility)
            {
                var isCorrectBehaviors = _oneDesign.CheckShowFullLessDesignNameActionLinkBehaviors(expectation);
                Assert.IsTrue(isCorrectBehaviors);
            }
        }

        private void AdminLoginStep(Account account)
        {
            _adminLogin.Open();
            _adminLogin.LoginSystem(account.UserName, account.Password);
        }

        private bool VerifyDownloadFileNames(string[] expectedFileNames)
        {
            var fileNames = new DirectoryInfo(NewAppTestDir).GetFiles()
                .Where(x => new[] { ".jpg", ".pdf" }.Contains(x.Extension.ToLower())).Select(x => Path.GetFileNameWithoutExtension(x.Name).Trim());

            var result = fileNames.All(x => expectedFileNames.Contains(x)) && expectedFileNames.All(x => fileNames.Contains(x));

            return result;
        }
    }
}
