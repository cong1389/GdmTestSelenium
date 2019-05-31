using GenerateDocument.Common.Extensions;
using GenerateDocument.Test.PageObjects.BackEnd;
using GenerateDocument.Test.Utilities;
using NUnit.Framework;
using System.Collections.Generic;
using static GenerateDocument.Test.Utilities.PageCommon;

namespace GenerateDocument.Test.PageTest.BackEnd
{
    [TestFixture("Chrome"), Order(1)]
    public class ManagedExtensionTest : PageTestBase
    {
        private AdminManagedConfiguration _adminManagedConfiguration;
        private AdminUserAccounts _adminUserAccounts;
        private AdminExtensions _adminExtensions;
        private AdminOrders _adminOrders;
        private AdminManageGroups _adminManageGroups;
        private AdminGlobalLibrary _adminGlobalLibrary;
        private AdminOrderStatistics _adminOrderStatistics;
        private AdminProjects _adminProjects;
        private ContentCollectionsPage _contentCollectionsPage;
        private AdminProducts _adminProductsPage;
        private AdminProductContent _adminProductContentPage;
        private AdminProductRetired _adminProductRetired;
        private AdminProductDetails _adminProductDetails;
        private AdminSiteOptions _siteOptionsPage;
        private AdminSiteOptions _adminSiteOptions;

        private const string RightMarketUserName = "rightmarket";
        private const string AdminSiteOptionsLabel = "Site Options";

        public ManagedExtensionTest(string environment) : base(environment)
        {

        }

        private static IEnumerable<string> ProductsCase(bool isKit)
        {
            if (isKit)
            {
                yield return "[AUTOTEST][SWF][KIT] Invitation + A4 Poster";
            }
            else
            {
                yield return "[AUTOTEST][SWF] A4 Poster";
            }
        }

        [SetUp]
        public void SetUpOneTestCase()
        {
            _adminManagedConfiguration = new AdminManagedConfiguration(DriverContext.Driver);
            _adminUserAccounts = new AdminUserAccounts(DriverContext.Driver);
            _adminExtensions = new AdminExtensions(DriverContext.Driver);
            _adminOrders = new AdminOrders(DriverContext.Driver);
            _adminManageGroups = new AdminManageGroups(DriverContext.Driver);
            _adminGlobalLibrary = new AdminGlobalLibrary(DriverContext.Driver);
            _adminOrderStatistics = new AdminOrderStatistics(DriverContext.Driver);
            _adminProjects = new AdminProjects(DriverContext.Driver);
            _contentCollectionsPage = new ContentCollectionsPage(DriverContext.Driver);
            _adminProductsPage = new AdminProducts(DriverContext);
            _adminProductContentPage = new AdminProductContent(DriverContext.Driver);
            _adminProductRetired = new AdminProductRetired(DriverContext);
            _adminProductDetails = new AdminProductDetails(DriverContext);
            _siteOptionsPage = new AdminSiteOptions(DriverContext.Driver);
            _adminSiteOptions = new AdminSiteOptions(DriverContext.Driver);
        }

        [Test]
        public void Check_Exempt_RightMarket_Account_Using_Managed_Extension()
        {
            AdminLoginStep(AdminAccount);

            _adminManagedConfiguration.Open();

            _adminManagedConfiguration.ClickToConfigurationTab();
            _adminManagedConfiguration.ChangeLayoutForAdminSite(false);
            _adminManagedConfiguration.SaveSetting();

            Assert.IsTrue(DriverContext.Driver.IsUrlEndsWith("AdminExtensions.aspx"));
            Assert.AreEqual(_adminExtensions.GetTitle(), "Manage extension modules.");
        }

        [Test]
        public void Check_Customer_Admin_Access_ShowOrders()
        {
            AdminLoginStep(CustomerAdminAccount);

            _adminOrders.Open();

            Assert.IsTrue(_adminOrders.GetTitleText().IsEquals("Orders"));
        }

        [Test]
        [TestCase("AdminItems.aspx?queue=orders")]
        [TestCase("AdminExtensions.aspx")]
        [TestCase("AdminOrders.aspx?queue=approval")]
        [TestCase("AdminOrders.aspx?queue=prep")]
        [TestCase("AdminOrders.aspx?queue=production")]
        [TestCase("AdminOrders.aspx?queue=shipping")]
        [TestCase("AdminOrders.aspx?queue=finance")]
        [TestCase("AdminLedger.aspx")]
        [TestCase("AdminUserPrivileges.aspx")]
        [TestCase("AdminOptionSet.aspx?ostype=UserProfile")]
        [TestCase("AdminOptionSet.aspx?ostype=Registration")]
        [TestCase("AdminReviews.aspx")]
        [TestCase("AdminApprovalWorkflowAssignments.aspx")]
        [TestCase("AdminAddressBooks.aspx")]
        [TestCase("AdminOptionSet.aspx?ostype=AddressFields")]
        [TestCase("AdminProjects.aspx")]
        [TestCase("AdminProductCategories.aspx")]
        [TestCase("AdminProducts.aspx")]
        [TestCase("AdminOptionSet.aspx?ostype=ProductMetadata")]
        [TestCase("AdminOptionSet.aspx?ostype=AssetMetadata")]
        [TestCase("AdminProductContent.aspx")]
        [TestCase("AdminContentCollections.aspx")]
        [TestCase("AdminUserAccounts.aspx?g=Admin")]
        [TestCase("AdminSiteOptions.aspx?page=CheckoutOptions")]
        [TestCase("AdminOptionSet.aspx?ostype=ShippingStep")]
        [TestCase("AdminOptionSet.aspx?ostype=PaymentStep")]
        [TestCase("AdminPriceTables.aspx")]
        [TestCase("AdminPriceTables.aspx?tabtype=Leaf")]
        [TestCase("AdminTaxTables.aspx")]
        [TestCase("AdminManageGroups.aspx?g=Admin")]
        [TestCase("AdminAdminPrivileges.aspx")]
        [TestCase("AdminNotifications.aspx")]
        [TestCase("AdminSiteOptions.aspx?page=General")]
        [TestCase("AdminSiteOptions.aspx?page=Login")]
        [TestCase("AdminSiteOptions.aspx?page=Localization")]
        [TestCase("AdminSiteOptions.aspx?page=UIConfiguration")]
        [TestCase("AdminSiteOptions.aspx?page=ShoppingCart")]
        [TestCase("AdminSiteOptions.aspx?page=ImageUpload")]
        [TestCase("AdminSiteOptions.aspx?page=Orders")]
        [TestCase("AdminSiteOptions.aspx?page=Security")]
        [TestCase("AdminSocialMedia.aspx?mode=ps")]
        [TestCase("AdminSocialMedia.aspx")]
        [TestCase("AdminSocialMediaOptions.aspx?mode=ps")]
        [TestCase("AdminSocialMediaOptions.aspx")]
        [TestCase("AdminDashboard.aspx")]
        [TestCase("AdminAbout.aspx")]
        [TestCase("AdminManagedConfiguration.aspx")]
        [TestCase("AdminExtensionsEdit.aspx?module=1069")]
        [TestCase("AdminUserDetail.aspx?g=Admin")]
        public void Check_User_Does_Not_Have_Permission_To_Access_Page(string page)
        {
            AdminLoginStep(CustomerAdminAccount);

            Assert.IsTrue(!DriverContext.Driver.IsAbleToAccessPage($"{ProjectBaseConfiguration.HostUrl}/{page}"));
        }

        [Test]
        public void Check_Customer_Admin_Access_UserAccount()
        {
            AdminLoginStep(AdminAccount);

            _adminUserAccounts.Open();

            Assert.IsTrue(_adminUserAccounts.GetTitleText().IsEquals("User Accounts"));
        }

        [Test]
        public void Check_Customer_Admin_Access_UserGroup()
        {
            AdminLoginStep(CustomerAdminAccount);

            _adminManageGroups.Open();

            Assert.IsTrue(_adminManageGroups.GetTitleText().IsEquals("User Groups"));
        }

        [Test]
        public void Check_Customer_Admin_Access_GlobalLibrary()
        {
            AdminLoginStep(CustomerAdminAccount);

            _adminGlobalLibrary.Open();

            Assert.IsTrue(_adminGlobalLibrary.GetTitleText().IsEquals("Global Library"));
        }

        [Test, Ignore("NotUsed")]
        public void Check_Customer_Admin_Access_OrderAnalytics()
        {
            AdminLoginStep(CustomerAdminAccount);

            _adminOrderStatistics.Open();

            Assert.IsTrue(_adminOrderStatistics.GetTitleText().IsEquals("Order Analytics"));
        }

        [Test]
        public void Check_Hidding_RightMarket_Account_AdminPage()
        {
            AdminLoginStep(AdminAccount);

            _adminUserAccounts.OpenAdminGroup();

            Assert.That(_adminUserAccounts.GetAccountNames(), Does.Not.Contains(RightMarketUserName));
        }

        [Test]
        public void Check_Hidding_RightMarket_Account_UserPage()
        {
            AdminLoginStep(AdminAccount);

            _adminUserAccounts.Open();

            Assert.That(_adminUserAccounts.GetAccountNames(), Does.Not.Contains(RightMarketUserName));
        }

        [Test]
        public void Check_Managed_Extension_Change_Text_ProjectPage()
        {
            ChangeRMLayoutForAdminSite();

            _adminProjects.Open();

            var tipText = @"Important: If you are updating an existing project, please use the 'Update Project' feature to avoid duplicate projects being created.";

            Assert.IsTrue(_adminProjects.GetTipTitleText().IsEquals(tipText));

            ChangeDefaultForAdminSite();
        }

        [Test]
        public void Check_Managed_Extension_Change_Text_Upload_Button()
        {
            ChangeRMLayoutForAdminSite();

            _adminProjects.Open();

            Assert.IsTrue(_adminProjects.GetUploadButtonText().IsEquals("upload"));

            ChangeDefaultForAdminSite();
        }

        [Test]
        public void Check_Managed_Extension_Change_Show_Upload_Panel()
        {
            ChangeRMLayoutForAdminSite();

            _adminProjects.Open();

            _adminProjects.ClickToUploadButton();

            Assert.IsTrue(_adminProjects.IsUploadBrowserButtonDisplayed());

            ChangeDefaultForAdminSite();
        }

        [Test]
        public void Check_Managed_Extension_Change_Text_Upload_Content()
        {
            ChangeRMLayoutForAdminSite();

            _adminProjects.Open();

            Assert.IsTrue(_adminProjects.GetTextOfLinkTextUploadContent().IsEquals("Upload Content"));

            ChangeDefaultForAdminSite();
        }

        [Test]
        public void Check_Managed_Extension_Change_Show_Upload_Content()
        {
            ChangeRMLayoutForAdminSite();

            _adminProjects.Open();

            _adminProjects.ClickToLinkTextUploadContent();

            Assert.IsTrue(_adminProjects.IsUploadBrowserButtonDisplayed());

            ChangeDefaultForAdminSite();
        }

        [Test]
        public void Check_Managed_Extension_Change_Text_When_Add_New_Project()
        {
            ChangeRMLayoutForAdminSite();

            _contentCollectionsPage.Open();

            Assert.IsTrue(_contentCollectionsPage.GetAdminOptFldImageListDivText().IsEquals("Files (Project Packages)"));

            ChangeDefaultForAdminSite();
        }

        [Test]
        public void Check_Managed_Extension_Change_Text_LinkProject()
        {
            ChangeRMLayoutForAdminSite();

            _adminProductsPage.NavigateTo();

            Assert.IsTrue(_adminProductsPage.GetLinkProjectText().IsEquals("Import Projects"));

            ChangeDefaultForAdminSite();
        }

        [Test, TestCaseSource(nameof(ProductsCase), new object[] { true })]
        public void Check_Managed_Extension_Change_Icon_ProductKit(string productName)
        {
            ChangeRMLayoutForAdminSite();

            _adminProductsPage.NavigateTo();

            var kitIconUrl = $"{ProjectBaseConfiguration.HostUrl}/Images/ROI360ProductKit.gif";

            Assert.IsTrue(_adminProductsPage.GetKitIconSrc(productName).IsEquals(kitIconUrl));

            ChangeDefaultForAdminSite();
        }

        [Test, TestCaseSource(nameof(ProductsCase), new object[] { false })]
        public void Check_Managed_Extension_Change_Icon_ProductVariableData(string productName)
        {
            ChangeRMLayoutForAdminSite();

            _adminProductsPage.NavigateTo();

            var vdpIconUrl = $"{ProjectBaseConfiguration.HostUrl}/Images/ROI360ProductVariableData.gif";

            Assert.IsTrue(_adminProductsPage.GetVdpIconSrc(productName).IsEquals(vdpIconUrl));

            ChangeDefaultForAdminSite();
        }

        [Test]
        public void Check_Changing_Text_AdminProductContent()
        {
            ChangeRMLayoutForAdminSite();

            _adminProductContentPage.Open();

            _adminProductContentPage.SelectProductType("HTML");

            Assert.IsTrue(_adminProductContentPage.GetLabelProductTypeText().IsEquals("This presents a HTML template that can be customized interactively and ordered"));

            ChangeDefaultForAdminSite();
        }

        [Test]
        public void Check_Changing_Text_AdminProductRetire()
        {
            ChangeRMLayoutForAdminSite();

            _adminProductsPage.NavigateTo();
            _adminProductsPage.ClickToAdvanceFlyer();

            _adminProductDetails.ClickToChangeSetting();
            _adminProductDetails.SelectSubmitJobCheckBox();
            _adminProductDetails.ClickDownloadRadio3();
            _adminProductDetails.SettingPriceNone();
            _adminProductDetails.UpdateSettings();
            _adminProductDetails.ClickToRelease();

            var secondMessage = "Users can order and reorder products but the product will keep the original product settings. So if you have made any changes to the product in Studio or Storefront (e.g. added a new variable to the product), then users reordering or editing products they have previously created will not see the new changes. This option should be used for releases related to configuration changes and no migration is needed e.g. Product data changes or reconfiguring FormFilling";
            var thirdMessage = "This will update the products in the basket and any reordered product to the live release settings. However this release can cause validation errors and could in rare cases disable the product from being ordered. This option should be used for design updates and user will be charged another click for the migration\r\nWe strongly recommend that this setting is not used for ‘kit’ products.";

            Assert.IsTrue(_adminProductRetired.GetTipNoIndentsTexts()[2].IsEquals(secondMessage));
            Assert.IsTrue(_adminProductRetired.GetTipNoIndentsTexts()[3].IsEquals(thirdMessage));

            ChangeDefaultForAdminSite();
        }

        [Test]
        public void Check_RightMarketAdmin_Access_SiteOption_Orders()
        {
            ChangeRMLayoutForAdminSite();

            _adminSiteOptions.Open();

            _adminSiteOptions.ClickToOrdersTab();

            Assert.IsTrue(_siteOptionsPage.GetTitleText().IsEquals(AdminSiteOptionsLabel));
            Assert.IsTrue(_siteOptionsPage.IsChangeSettingButtonEnabled());

            ChangeDefaultForAdminSite();
        }

        [Test]
        public void Check_RightMarketAdmin_Access_SiteOption_Security()
        {
            ChangeRMLayoutForAdminSite();

            _siteOptionsPage.Open();

            _siteOptionsPage.ClickToSecurityTab();

            Assert.IsTrue(_siteOptionsPage.GetTitleText().IsEquals(AdminSiteOptionsLabel));
            Assert.IsTrue(_siteOptionsPage.IsChangeSettingButtonEnabled());

            ChangeDefaultForAdminSite();
        }

        private void ChangeDefaultForAdminSite()
        {
            AdminLoginStep(AdminAccount);

            _adminManagedConfiguration.Open();
            _adminManagedConfiguration.ClickToConfigurationTab();
            _adminManagedConfiguration.ChangeLayoutForAdminSite(false);
            _adminManagedConfiguration.SaveSetting();
        }

        private void AdminLoginStep(Account account)
        {
            new AdminLogin(DriverContext)
            .NavigateTo()
            .LoginSystem(account.UserName, account.Password);
        }

        private void ChangeRMLayoutForAdminSite()
        {
            AdminLoginStep(AdminAccount);

            _adminManagedConfiguration.Open();

            _adminManagedConfiguration.ClickToConfigurationTab();
            _adminManagedConfiguration.ChangeLayoutForAdminSite(true);
            _adminManagedConfiguration.SaveSetting();
        }
    }
}
