using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenerateDocument.Common.Types;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;

namespace GenerateDocument.Common
{
    public class DriverContext
    {
        private IWebDriver driver;
        private WebDriverWait driverWait;

        public IWebDriver Driver
        {
            get { return driver; }
        }

        public WebDriverWait BrowserWait(int? timeoutInSecond = null)
        {
            if (driverWait == null)
            {
                driverWait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSecond ?? BaseConfiguration.LongTimeout ));
            }

            return driverWait;
        }

        public void Start(BrowserTypes browserTypes = BrowserTypes.Chrome)
        {
            switch (browserTypes)
            {
                case BrowserTypes.Chrome:
                    var options = new ChromeOptions();
                    options.AddUserProfilePreference("download.default_directory", BaseConfiguration.NewAppTestDir);
                    options.AddUserProfilePreference("profile.default_content_setting_values.automatic_downloads", 1);

                    driver = new ChromeDriver(options);
                    break;

                case BrowserTypes.Firefox:
                    var firefoxOptions = new FirefoxOptions();
                    firefoxOptions.SetPreference("browser.download.dir", BaseConfiguration.NewAppTestDir);
                    firefoxOptions.SetPreference("browser.download.folderList", 2);
                    firefoxOptions.SetPreference("pdfjs.disabled", true);
                    firefoxOptions.SetPreference("browser.helperApps.neverAsk.saveToDisk", "image/png,image/jpeg,application/pdf");

                    driver = new FirefoxDriver(firefoxOptions);
                    break;

                case BrowserTypes.InternetExplorer:
                    driver = new InternetExplorerDriver();
                    break;
            }
        }

        public void Stop()
        {
            if (this.driver != null)
            {
                driver.Manage().Cookies.DeleteAllCookies();
                driver.Dispose();

                var dir = new DirectoryInfo(BaseConfiguration.NewAppTestDir);
                foreach (FileInfo file in dir.GetFiles())
                {
                    file.Delete();
                }

                driver.Quit();
            }
        }
    }
}
