using NUnit.Framework;
using static GenerateDocument.Test.WrapperFactory.ConfigInfo;

namespace GenerateDocument.Test.Utilities
{
    public static class PageCommon
    {
        public static readonly string AdminGlobalLibraryPage = $"{HostUrl}/AdminGlobalLibrary.aspx";
        public static readonly string AdminLoginPage = $"{HostUrl}/AdminLogin.aspx";
        public static readonly string AdminManagedConfigurationPage = $"{HostUrl}/AdminManagedConfiguration.aspx";
        public static readonly string AdminManageGroupsPage = $"{HostUrl}/AdminManageGroups.aspx?g=User";
        public static readonly string AdminOrdersPage = $"{HostUrl}/AdminOrders.aspx";
        public static readonly string AdminOrderStatisticsPage = $"{HostUrl}/AdminOrderStatistics.aspx";
        public static readonly string AdminProductContentPage = $"{HostUrl}/AdminProductContent.aspx";
        public static readonly string AdminProductsPage = $"{HostUrl}/AdminProducts.aspx";
        public static readonly string AdminProjectsPage = $"{HostUrl}/AdminProjects.aspx";
        public static readonly string AdminSiteOptionsPage = $"{HostUrl}/AdminSiteOptions.aspx";
        public static readonly string AdminUserAccountsPage = $"{HostUrl}/AdminUserAccounts.aspx";
        public static readonly string AdminUserAccountAdminGroupPage = $"{HostUrl}/AdminUserAccounts.aspx?g=Admin";
        public static readonly string AdminContentCollectionsPage = $"{HostUrl}/AdminContentCollections.aspx";
        public static readonly string UserContentShoppingCartBasketPage = $"{HostUrl}/usercontentshoppingcart.aspx?page=basket";
        public static readonly string UserContentShoppingCartDraftPage = $"{HostUrl}/usercontentshoppingcart.aspx?page=draft";
        public static readonly string UserContentStartPage = $"{HostUrl}/UserContentStart.aspx";
        public static readonly string UserLoginPage = $"{HostUrl}/Login.aspx";
        public static readonly string UserContentOrdersPage = $"{HostUrl}/UserContentOrders.aspx";
        public static readonly string UserContentApprovalsPage = $"{HostUrl}/UserContentApprovals.aspx";
        public static readonly string UserContentApprovalsReviewPage = $"{HostUrl}/UserContentApprovalsReview.aspx";
        public static readonly string AdminExtensionsPage = $"{HostUrl}/AdminExtensions.aspx";
        public static readonly string MyDesignPage = $"{NewAppUrl}/#/designs";
        public static readonly string DesignDetailAndActionPage = $"{NewAppUrl}/#/onedesign/";

        public static readonly string RetiredPrefix = "[RETIRED]";
        public static readonly string DeletedPrefix = "[DELETED]";

        public static readonly Account CustomerAdminAccount = new Account { UserName = CustomerAdminUserName, Password = CustomerAdminPassword };
        public static readonly Account TestAccount = new Account { UserName = "RightMarket", Password = "123" };
        public static readonly Account SocialNetworkAccount = new Account { UserName = "rm.devtest@yahoo.com", Password = "thaopv" };
        public static readonly Account AdminAccount = new Account { UserName = AdminId, Password = AdminPassword };
    }

    public struct Account
    {
        public string UserName;
        public string Password;
    }

    public struct ProductInfo
    {
        public string Id;
        public string Name;
    }

    public struct AutoSaveField
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public string Id { get; set; }
    }
}
