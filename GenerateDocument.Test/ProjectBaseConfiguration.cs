using System.Configuration;
using System.IO;
using NUnit.Framework;

namespace GenerateDocument.Test
{
    public static class ProjectBaseConfiguration
    {
        private static readonly string CurrentDirectory = TestContext.CurrentContext.WorkDirectory;

        public static string DataDrivenFile 
        {
            get
            {
                return Path.Combine(Directory.GetParent(CurrentDirectory).Parent.FullName, ConfigurationManager.AppSettings["dataDrivenFile"]);
            }
        }

        public static readonly int TimeoutInSecond = int.Parse(ConfigurationManager.AppSettings["TIMEOUT_IN_SECOND"]);
        public static readonly string HostUrl = ConfigurationManager.AppSettings["HOST_URL"];
        public static readonly string NewAppUrl = ConfigurationManager.AppSettings["NEWAPP_URL"];

        public static readonly string AdminId = ConfigurationManager.AppSettings["ADMIN_ID"];
        public static readonly string AdminPassword = ConfigurationManager.AppSettings["ADMIN_PASSWORD"];

        public static readonly string UserId = ConfigurationManager.AppSettings["USER_ID"];
        public static readonly string UserPassword = ConfigurationManager.AppSettings["USER_PASSWORD"];

        public static readonly string CustomerAdminUserName = ConfigurationManager.AppSettings["CUSTOMER_ADMIN"];
        public static readonly string CustomerAdminPassword = ConfigurationManager.AppSettings["CUSTOMER_PASSWORD"];

        public static readonly string ScreenshotPath = ConfigurationManager.AppSettings["SCREENSHOT_PATH"];

        public static readonly string A4PosterName = "A4 Poster";

        public static readonly string NewAppTestDir = @"C:\NewAppTestDir";

        public static readonly string MopinionFormId = ConfigurationManager.AppSettings["MopinionFormId"];
    }
}
