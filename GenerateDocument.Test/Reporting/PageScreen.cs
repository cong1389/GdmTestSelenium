using OpenQA.Selenium;
using GenerateDocument.Test.WrapperFactory;
using System;
using System.Drawing.Imaging;

using static GenerateDocument.Test.WrapperFactory.ConfigInfo;

namespace GenerateDocument.Test.Reporting
{
    public class PageScreen
    {
        public void TakeScreenshot(IWebDriver browser, string filename)
        {
            try
            {
                var screenshot = ((ITakesScreenshot)browser).GetScreenshot();
                screenshot.SaveAsFile($"{ScreenshotPath}\\{filename}.png", ScreenshotImageFormat.Png);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
