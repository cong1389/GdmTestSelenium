using OpenQA.Selenium;

namespace GenerateDocument.Common.Extensions
{
    public static class WebElementExtension
    {
        public static void OnClickJavaScript(this IWebElement element)
        {
            var jsExecutor = (IJavaScriptExecutor)element.ToDriver();
            jsExecutor.ExecuteScript("arguments[0].click();", element);
        }
    }
}
