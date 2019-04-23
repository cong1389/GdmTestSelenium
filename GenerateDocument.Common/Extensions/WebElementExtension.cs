using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
