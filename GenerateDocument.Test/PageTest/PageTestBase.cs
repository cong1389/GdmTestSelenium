using GenerateDocument.Common;
using NUnit.Framework;

namespace GenerateDocument.Test.PageTest
{
    public class PageTestBase
    {
        private readonly DriverContext driverContext = new DriverContext();

        protected DriverContext DriverContext
        {
            get
            {
                return this.driverContext;
            }
        }

        [OneTimeSetUp]
        public void SetUp()
        {
            driverContext.Start();
            driverContext.Driver.Manage().Window.Maximize();
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            driverContext.Stop();
        }
    }
}
