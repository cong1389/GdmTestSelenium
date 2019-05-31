using GenerateDocument.Common;
using OpenQA.Selenium;

namespace GenerateDocument.Test.PageObjects
{
    public class PageBaseObject
    {
        protected PageBaseObject(DriverContext driverContext)
        {
            this.DriverContext = driverContext;
            this.Driver = driverContext.Driver;
        }

        protected IWebDriver Driver { get; set; }

        protected DriverContext DriverContext { get; set; }


    }
}
