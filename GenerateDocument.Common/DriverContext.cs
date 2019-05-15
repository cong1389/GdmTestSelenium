using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenerateDocument.Common.Extensions;
using GenerateDocument.Common.Types;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;

namespace GenerateDocument.Common
{
    public class DriverContext
    {
        private WebDriverWait _driverWait;

        public IWebDriver Driver { get; private set; }

        //Need to remove after refactor finished
        public WebDriverWait BrowserWait(int? timeoutInSecond = null)
        {
            return _driverWait ?? (_driverWait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutInSecond ?? BaseConfiguration.LongTimeout)));
        }

        public BrowserTypes CrossBrowserEnvironment { get; set; }

        public void Start()
        {
            switch (CrossBrowserEnvironment)
            {
                case BrowserTypes.Firefox:
                    var firefoxOptions = new FirefoxOptions();
                    firefoxOptions.SetPreference("browser.download.dir", BaseConfiguration.NewAppTestDir);
                    firefoxOptions.SetPreference("browser.download.folderList", 2);
                    firefoxOptions.SetPreference("pdfjs.disabled", true);
                    firefoxOptions.SetPreference("browser.helperApps.neverAsk.saveToDisk", "image/png,image/jpeg,application/pdf");

                    Driver = new FirefoxDriver(firefoxOptions);
                    break;

                case BrowserTypes.IE:
                    var ieOptions= new InternetExplorerOptions();

                    Driver = new InternetExplorerDriver(ieOptions);
                    break;

                case BrowserTypes.Edge:
                    var edgeOptons = new EdgeOptions();
                    edgeOptons.PageLoadStrategy = (PageLoadStrategy)EdgePageLoadStrategy.Eager;

                    Driver = new EdgeDriver(edgeOptons);

                    break;

                default:
                    var options = new ChromeOptions();
                    options.AddUserProfilePreference("download.default_directory", BaseConfiguration.NewAppTestDir);
                    options.AddUserProfilePreference("profile.default_content_setting_values.automatic_downloads", 1);

                    Driver = new ChromeDriver(options);
                    break;
            }
        }

        public void Stop()
        {
            if (this.Driver != null)
            {
                Driver.DeleteAllCookies();
                Driver.Dispose();

                var dir = new DirectoryInfo(BaseConfiguration.NewAppTestDir);
                foreach (FileInfo file in dir.GetFiles())
                {
                    file.Delete();
                }

                Driver.Quit();
            }
        }
    }
}
