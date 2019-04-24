namespace GenerateDocument.Test.Utilities
{
    public static class PageCommon
    {
        public static readonly string AdminGlobalLibraryPage = $"{ProjectBaseConfiguration.HostUrl}/AdminGlobalLibrary.aspx";
        public static readonly string AdminLoginPage = $"{ProjectBaseConfiguration.HostUrl}/AdminLogin.aspx";
        public static readonly string AdminManagedConfigurationPage = $"{ProjectBaseConfiguration.HostUrl}/AdminManagedConfiguration.aspx";
        public static readonly string AdminManageGroupsPage = $"{ProjectBaseConfiguration.HostUrl}/AdminManageGroups.aspx?g=User";
        public static readonly string AdminOrdersPage = $"{ProjectBaseConfiguration.HostUrl}/AdminOrders.aspx";
        public static readonly string AdminOrderStatisticsPage = $"{ProjectBaseConfiguration.HostUrl}/AdminOrderStatistics.aspx";
        public static readonly string AdminProductContentPage = $"{ProjectBaseConfiguration.HostUrl}/AdminProductContent.aspx";
        public static readonly string AdminProductsPage = $"{ProjectBaseConfiguration.HostUrl}/AdminProducts.aspx";
        public static readonly string AdminProjectsPage = $"{ProjectBaseConfiguration.HostUrl}/AdminProjects.aspx";
        public static readonly string AdminSiteOptionsPage = $"{ProjectBaseConfiguration.HostUrl}/AdminSiteOptions.aspx";
        public static readonly string AdminUserAccountsPage = $"{ProjectBaseConfiguration.HostUrl}/AdminUserAccounts.aspx";
        public static readonly string AdminUserAccountAdminGroupPage = $"{ProjectBaseConfiguration.HostUrl}/AdminUserAccounts.aspx?g=Admin";
        public static readonly string AdminContentCollectionsPage = $"{ProjectBaseConfiguration.HostUrl}/AdminContentCollections.aspx";
        public static readonly string UserContentStartPage = $"{ProjectBaseConfiguration.HostUrl}/UserContentStart.aspx";
        public static readonly string UserLoginPage = $"{ProjectBaseConfiguration.HostUrl}/Login.aspx";
        public static readonly string UserContentOrdersPage = $"{ProjectBaseConfiguration.HostUrl}/UserContentOrders.aspx";
        public static readonly string UserContentApprovalsPage = $"{ProjectBaseConfiguration.HostUrl}/UserContentApprovals.aspx";
        public static readonly string AdminExtensionsPage = $"{ProjectBaseConfiguration.HostUrl}/AdminExtensions.aspx";
        public static readonly string MyDesignPage = $"{ProjectBaseConfiguration.NewAppUrl}/#/designs";

        public static readonly string RetiredPrefix = "[RETIRED]";
        public static readonly string DeletedPrefix = "[DELETED]";

        public static readonly Account CustomerAdminAccount = new Account { UserName = ProjectBaseConfiguration.CustomerAdminUserName, Password = ProjectBaseConfiguration.CustomerAdminPassword };
        public static readonly Account TestAccount = new Account { UserName = "RightMarket", Password = "123" };
        public static readonly Account AdminAccount = new Account { UserName = ProjectBaseConfiguration.AdminId, Password = ProjectBaseConfiguration.AdminPassword };
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
