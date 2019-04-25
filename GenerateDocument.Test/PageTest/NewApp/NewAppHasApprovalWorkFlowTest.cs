using GenerateDocument.Test.PageObjects.NewApp;
using NUnit.Framework;
using System.Collections.Generic;
using GenerateDocument.Common.Helpers;

namespace GenerateDocument.Test.PageTest.NewApp
{
    public partial class NewApp_Test : PageTestBase
    {
        [Test, TestCaseSource(nameof(WorkflowTestResources), new object[] { true, null })]
        public void Design_SuccessfullyCreated_FromDocumentHasApprovalWorkflow(string templateName, Dictionary<string, bool> settings)
        {
            LoginStep(_returnPage);

            CreateDocumentStep(templateName);

            VerifyDesignStatus($"{templateName}_{_designNamePrefix}", ifUnpublished: true);

            _myDesign.GoToActions($"{templateName}_{_designNamePrefix}");

            SubmitForApprovalStep($"{templateName}_{_designNamePrefix}");

            VerifyDesignStatus($"{templateName}_{_designNamePrefix}", ifUnreviewed: true);
        }

        [Test, TestCaseSource(nameof(WorkflowTestResources), new object[] { true, true })]
        public void DesignOutputs_VerifyStatusDesign_SuccessfullyDownloaded_HasApproval(string templateName, Dictionary<string, bool> settings)
        {
            try
            {
                LoginStep(_returnPage);

                CreateDocumentStep(templateName);

                VerifyDesignStatus($"{templateName}_{_designNamePrefix}", ifUnpublished: true);

                _myDesign.GoToActions($"{templateName}_{_designNamePrefix}");

                SubmitForApprovalStep($"{templateName}_{_designNamePrefix}");

                ReviewDesignIfHasApprovalWorkflow($"{templateName}_{_designNamePrefix}", settings["IsApproved"]);

                _action.UserLogout();
                LoginStep(_returnPage);

                VerifyDesignStatus($"{templateName}_{_designNamePrefix}", ifApproved: settings["IsApproved"]);

                _myDesign.GoToActions($"{templateName}_{_designNamePrefix}");

                PlaceOrderStep($"{templateName}_{_designNamePrefix}", settings["IsKit"], false);
            }
            finally
            {
                FilesHelper.DeleteAllFiles(ProjectBaseConfiguration.NewAppTestDir);
            }

        }

        [Test, TestCaseSource(nameof(WorkflowTestResources), new object[] { true, false })]
        public void DesignStatus_IsRejected_IfHasBeenRejected(string templateName, Dictionary<string, bool> settings)
        {
            LoginStep(_returnPage);

            CreateDocumentStep(templateName);

            VerifyDesignStatus($"{templateName}_{_designNamePrefix}", ifUnpublished: true);

            _myDesign.GoToActions($"{templateName}_{_designNamePrefix}");

            SubmitForApprovalStep($"{templateName}_{_designNamePrefix}");

            ReviewDesignIfHasApprovalWorkflow($"{templateName}_{_designNamePrefix}", false);

            VerifyDesignStatus($"{templateName}_{_designNamePrefix}", ifRejected: true);
        }

        [Test, TestCaseSource(nameof(WorkflowTestResources), new object[] { true, null })]
        public void Design_CopyAction_IsAbleToDo_FromDocumentHasApprovalWorkflow(string templateName, Dictionary<string, bool> settings)
        {
            LoginStep(_returnPage);

            CreateDocumentStep(templateName);

            new MyDesign(DriverContext)
                .NavigateTo();

            var designName = $"{templateName}_{_designNamePrefix}";

            var copiedDesignName = $"[COPIED]{designName}";

            Assert.IsTrue(_myDesign.CheckDesignCloneable(designName) && _myDesign.DoCopyDesign(designName, copiedDesignName), "It is able to clone new design when design has approval workflow");
        }
    }
}
