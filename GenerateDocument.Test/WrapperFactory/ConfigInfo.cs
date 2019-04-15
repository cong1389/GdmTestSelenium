using System.Configuration;
using GenerateDocument.Test.Utilities;

namespace GenerateDocument.Test.WrapperFactory
{
    public static class ConfigInfo
    {
        public static readonly string BrowserName = ConfigurationManager.AppSettings["BROWSER_NAME"];
        public static readonly int TimeoutInSecond = int.Parse(ConfigurationManager.AppSettings["TIMEOUT_IN_SECOND"]);
        public static readonly string HostUrl = ConfigurationManager.AppSettings["HOST_URL"];
        public static readonly string NewAppUrl = ConfigurationManager.AppSettings["NEWAPP_URL"];

        public static readonly string AdminId = ConfigurationManager.AppSettings["ADMIN_ID"];
        public static readonly string AdminPassword = ConfigurationManager.AppSettings["ADMIN_PASSWORD"];

        public static readonly string UserId = ConfigurationManager.AppSettings["USER_ID"];
        public static readonly string UserPassword = ConfigurationManager.AppSettings["USER_PASSWORD"];

        public static readonly string CustomerAdminUserName = ConfigurationManager.AppSettings["CUSTOMER_ADMIN"];
        public static readonly string CustomerAdminPassword = ConfigurationManager.AppSettings["CUSTOMER_PASSWORD"];

        public static readonly string UserViewStagedName = ConfigurationManager.AppSettings["USER_VIEW_STAGED_NAME"];

        public static readonly string UserViewStagedPassword = ConfigurationManager.AppSettings["USER_VIEW_STAGED_PASSWORD"];

        public static readonly string ScreenshotPath = ConfigurationManager.AppSettings["SCREENSHOT_PATH"];

        public static readonly string BasketPosterName = ConfigurationManager.AppSettings["BASKET_POSTER_NAME"];

        public static readonly string DraftPosterName = ConfigurationManager.AppSettings["DRAFT_POSTER_NAME"];

        public static readonly string AdvancedFlyerName = ConfigurationManager.AppSettings["ADVANCED_FLYER_NAME"];

        public static readonly string A4PosterName = "A4 Poster";

        public static readonly string FinancialKitName = "Financial Services Kit";

        public static readonly string BusinessCardhName = "BusinessCard";

        public static readonly string NewAppTestDir = @"C:\NewAppTestDir";

        public static readonly string MopinionFormId = ConfigurationManager.AppSettings["MopinionFormId"];
    }
}
