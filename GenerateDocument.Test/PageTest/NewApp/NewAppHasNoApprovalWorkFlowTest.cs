using GenerateDocument.Common.Extensions;
using GenerateDocument.Common.Helpers;
using GenerateDocument.Test.WrapperFactory;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using static GenerateDocument.Test.Utilities.PageCommon;
using static GenerateDocument.Test.WrapperFactory.ConfigInfo;

namespace GenerateDocument.Test.PageTest.NewApp
{
    public partial class NewApp_Test : PageTestBase
    {
        [Test, TestCaseSource(nameof(WorkflowTestResources), new object[] { false, null })]
        public void DesignOutputs_VerifyStatusDesign_SuccessfullyDownloaded(string templateName, Dictionary<string, bool> settings)
        {
            try
            {
                LoginStep(_returnPage);

                CreateDocumentStep(templateName);

                VerifyDesignType($"{templateName}_{_designNamePrefix}", settings["IsKit"]);
                VerifyDesignStatus($"{templateName}_{_designNamePrefix}", ifUnpublished: true);

                _myDesign.GoToActions($"{templateName}_{_designNamePrefix}");

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

        [Test, TestCaseSource(nameof(WorkflowTestResources), new object[] { false, null })]
        public void DesignOutputs_AfterEdited_SuccessfullyPlacedOrderAndDownloaded(string templateName, Dictionary<string, bool> settings)
        {
            try
            {
                LoginStep(_returnPage);

                CreateDocumentStep(templateName);

                VerifyDesignType($"{templateName}_{_designNamePrefix}", settings["IsKit"]);
                VerifyDesignStatus($"{templateName}_{_designNamePrefix}", ifUnpublished: true);

                _myDesign.NavigateTo();

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

        [Test, TestCaseSource(nameof(WorkflowTestResources), new object[] { false, null })]
        public void RetiredDocument_CopyAndEditAction_IsNotAbleToDo(string templateName, Dictionary<string, bool> settings)
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

            var editable = _myDesign.CheckDesignEditable($"{RetiredPrefix}{templateName}_{_designNamePrefix}");
            Assert.IsFalse(editable, "Can not edit when the product has been retired");
        }

        [Test, TestCaseSource(nameof(WorkflowTestResources), new object[] { false, null })]
        public void Design_CopyAction_IsNotAbleToDo_IfDeleted(string templateName, Dictionary<string, bool> settings)
        {
            //step 1: prepare product from admin
            AdminLoginStep(AdminAccount);
            _adminProducts.Open();
            _adminProducts.CloneToNewProduct(templateName, DeletedPrefix);

            //step 2: user create design
            LoginStep(_returnPage);
            CreateDocumentStep($"{DeletedPrefix}{templateName}");

            //step 3: admin retired this product
            AdminLoginStep(AdminAccount);
            _adminProducts.Open();
            _adminProducts.DeleteProduct($"{DeletedPrefix}{templateName}");

            LoginStep(_returnPage);

            var label = _myDesign.GetRetiredOrDeletedLabel($"{DeletedPrefix}{templateName}_{_designNamePrefix}");
            Assert.IsTrue(label.IsEquals("product deleted"), "This design must be mark as Product Deleted");

            //Do not allow copy
            var cloneable = _myDesign.CheckDesignCloneable($"{DeletedPrefix}{templateName}_{_designNamePrefix}");
            Assert.IsFalse(cloneable, "Can not clone new design from design when the product has been deleted");

            //Do not allow go to detail page
            var accessible = _myDesign.CheckDesignCanGoToActionPage($"{DeletedPrefix}{templateName}_{_designNamePrefix}");
            Assert.IsFalse(accessible, "Design can not view details when the product has been deleted");

            //Do not allow edit action
            var editable = _myDesign.CheckDesignEditable($"{DeletedPrefix}{templateName}_{_designNamePrefix}");
            Assert.IsFalse(editable, "Can not edit design when the product has been deleted");
        }

        [Test, TestCaseSource(nameof(WorkflowTestResources), new object[] { false, null })]
        public void Design_CopyAction_IsAbleToDo_WhenItIsSingleDocumentOrKitDocument(string templateName, Dictionary<string, bool> settings)
        {
            LoginStep(_returnPage);

            CreateDocumentStep(templateName);

            VerifyDesignType($"{templateName}_{_designNamePrefix}", settings["IsKit"]);
            VerifyDesignStatus($"{templateName}_{_designNamePrefix}", ifUnpublished: true);

            _myDesign.NavigateTo();

            var designName = $"{templateName}_{_designNamePrefix}";

            var copiedDesignName = $"[COPIED]{designName}";

            Assert.IsTrue(_myDesign.CheckDesignCloneable(designName) && _myDesign.DoCopyDesign(designName, copiedDesignName), "It is able to clone new design from single design or kit design");
        }

        [Test]
        [TestCaseSource(nameof(WorkflowTestResources), new object[] { false, null })]
        public void DesignOutputs_SuccessfullyDownloaded_WithoutConfirm_WhenProductHadNewReleaseWithMaintainOriginalOption(string templateName, Dictionary<string, bool> settings)
        {
            LoginStep(_returnPage);

            CreateDocumentStep(templateName);

            VerifyDesignType($"{templateName}_{_designNamePrefix}", settings["IsKit"]);
            VerifyDesignStatus($"{templateName}_{_designNamePrefix}", ifUnpublished: true);

            _myDesign.GoToActions($"{templateName}_{_designNamePrefix}");

            PlaceOrderStep($"{templateName}_{_designNamePrefix}", settings["IsKit"]);

            VerifyDesignStatus($"{templateName}_{_designNamePrefix}");

            FilesHelper.DeleteAllFiles(ConfigInfo.NewAppTestDir);

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

            _myDesign.GoToActions($"{templateName}_{_designNamePrefix}");

            PlaceOrderStep($"{templateName}_{_designNamePrefix}", settings["IsKit"], false);
        }

        [Test, TestCaseSource(nameof(WorkflowTestResources), new object[] { false, null })]
        public void DesignOutputs_SuccessfullyPlacedOrderAndDownloaded_WhenProductHadNewReleaseWithConvertToLatestOption(string templateName, Dictionary<string, bool> settings)
        {
            try
            {
                LoginStep(_returnPage);

                CreateDocumentStep(templateName);

                VerifyDesignType($"{templateName}_{_designNamePrefix}", settings["IsKit"]);
                VerifyDesignStatus($"{templateName}_{_designNamePrefix}", ifUnpublished: true);

                _myDesign.GoToActions($"{templateName}_{_designNamePrefix}");

                PlaceOrderStep($"{templateName}_{_designNamePrefix}", settings["IsKit"]);

                VerifyDesignStatus($"{templateName}_{_designNamePrefix}");

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

                _myDesign.GoToActions($"{templateName}_{_designNamePrefix}");

                PlaceOrderStep($"{templateName}_{_designNamePrefix}", settings["IsKit"]);

                FilesHelper.DeleteAllFiles(ConfigInfo.NewAppTestDir);
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

    }
}
