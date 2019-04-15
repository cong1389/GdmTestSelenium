using GenerateDocument.Test.PageObjects.NewApp;
using GenerateDocument.Test.Utilities;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace GenerateDocument.Test.PageTest.NewApp
{
    public partial class NewApp_Test : PageTestBase
    {
        [Test, TestCaseSource(nameof(WorkflowTestResources), new object[] { true, true })]
        public void DesignName_DoNotAllowDuplicate_Or_ExceedMaxlength_WhenCreatingOrRenaming(string templateName, Dictionary<string, bool> settings)
        {
            LoginStep(_returnPage);

            CreateDocumentStep(templateName);

            new MyDesign(DriverContext)
                .NavigateTo();

            CreateDocumentStep(templateName, true);

            var designName = $"[RENAME]{templateName}_{_designNamePrefix}";

            VerifyRenameAction(string.Empty, "Please enter design name");

            VerifyRenameAction(designName, "Please enter a different name");

            var longDesriptionExceedMaxLength = designName + TestUtil.RandomName(200);

            VerifyRenameAction(longDesriptionExceedMaxLength, "Design name should not exceed 200 characters");
        }

        [Test, TestCaseSource(nameof(WorkflowTestResources), new object[] { true, true })]
        public void DesignName_SuccessfullyRenamed_IfValidName(string templateName, Dictionary<string, bool> settings)
        {
            LoginStep(_returnPage);

            CreateDocumentStep(templateName);

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

            designName = $"[RENAME]{templateName}_{_designNamePrefix}";
            newDesignName = $"{designName}_<script>alert('{TestUtil.RandomName(5)}');</script>";
            VerifyRenameAction(newDesignName);
        }

        [Test, TestCaseSource(nameof(WorkflowTestResources), new object[] { true, true })]
        public void DesignName_TruncatedAutomatically_And_AbleToShowFullOrLess_IfItIsLongText(string templateName, Dictionary<string, bool> settings)
        {
            LoginStep(_returnPage);

            CreateDocumentStep(templateName);

            VerifyShowFullLessDesignNameAction(true);

            var shortDesignName = TestUtil.RandomName(5);
            VerifyRenameAction(shortDesignName);
            VerifyShowFullLessDesignNameAction(false);

            var longDesignName = TestUtil.RandomName(200);
            VerifyRenameAction(longDesignName);
            VerifyShowFullLessDesignNameAction(true);
        }
    }
}
