using System;
using GenerateDocument.Common;
using GenerateDocument.Common.Types;
using NUnit.Framework;

namespace GenerateDocument.Test.PageTest
{
    public class PageTestBase
    {
        private readonly DriverContext _driverContext = new DriverContext();

        protected DriverContext DriverContext => this._driverContext;

        public PageTestBase(string environment)
        {
            Enum.TryParse(environment, out BrowserTypes browserType);
            _driverContext.CrossBrowserEnvironment = browserType;
        }

        [OneTimeSetUp]
        public void SetUp()
        {
            _driverContext.Start();
            _driverContext.Driver.Manage().Window.Maximize();
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            _driverContext.Stop();
        }
    }
}
