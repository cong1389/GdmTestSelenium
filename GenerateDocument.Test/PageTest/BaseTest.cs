using log4net;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using System;
using System.IO;
using System.Reflection;
using GenerateDocument.Common.Types;
using GenerateDocument.Test.WrapperFactory;
using OpenQA.Selenium.Edge;
using static GenerateDocument.Test.WrapperFactory.ConfigInfo;

namespace GenerateDocument.Test.PageTest
{
    public abstract class BaseTest
    {
        protected IWebDriver _browser;

        public static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public BaseTest()
        {
            SetupBrowser();
        }

        public BaseTest(BrowserTypes browserTypes)
        {
            SetupBrowser(browserTypes);
        }

        [OneTimeSetUp]
        public void SetUp()
        {
            if (!Directory.Exists(NewAppTestDir))
            {
                Directory.CreateDirectory(NewAppTestDir);
            }

            _browser.Manage().Window.Maximize();

            Console.WriteLine("Finished OneTimeSetUp");
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            _browser.Manage().Cookies.DeleteAllCookies();
            _browser.Dispose();

            var dir = new DirectoryInfo(NewAppTestDir);
            foreach (FileInfo file in dir.GetFiles())
            {
                file.Delete();
            }

            Console.WriteLine("Finished OneTimeTearDown");
        }

        private void SetupBrowser(BrowserTypes browserTypes = BrowserTypes.Chrome)
        {
            switch (browserTypes)
            {
                case BrowserTypes.Chrome:
                    var options = new ChromeOptions();
                    options.AddUserProfilePreference("download.default_directory", NewAppTestDir);
                    options.AddUserProfilePreference("profile.default_content_setting_values.automatic_downloads", 1);

                    _browser = new ChromeDriver(options);
                    break;

                case BrowserTypes.Firefox:
                    var firefoxOptions = new FirefoxOptions();
                    firefoxOptions.SetPreference("browser.download.dir", NewAppTestDir);
                    firefoxOptions.SetPreference("browser.download.folderList", 2);
                    firefoxOptions.SetPreference("pdfjs.disabled", true);
                    firefoxOptions.SetPreference("browser.helperApps.neverAsk.saveToDisk", "image/png,image/jpeg,application/pdf");

                    _browser = new FirefoxDriver(firefoxOptions);
                    break;

                case BrowserTypes.InternetExplorer:
                    _browser = new InternetExplorerDriver();
                    break;
            }
        }
    }
}
