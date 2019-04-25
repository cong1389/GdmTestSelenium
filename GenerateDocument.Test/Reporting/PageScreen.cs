using OpenQA.Selenium;
using System;

namespace GenerateDocument.Test.Reporting
{
    public class PageScreen
    {
        public void TakeScreenshot(IWebDriver browser, string filename)
        {
            try
            {
                var screenshot = ((ITakesScreenshot)browser).GetScreenshot();
                screenshot.SaveAsFile($"{ProjectBaseConfiguration.ScreenshotPath}\\{filename}.png", ScreenshotImageFormat.Png);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
