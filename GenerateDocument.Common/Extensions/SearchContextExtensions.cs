﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenerateDocument.Common.Helpers;
using GenerateDocument.Common.Types;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Support.UI;

namespace GenerateDocument.Common.Extensions
{
    public static class SearchContextExtensions
    {
        public static IWebElement GetElement(this ISearchContext element, ElementLocator locator)
        {
            return GetElement(element, locator, BaseConfiguration.LongTimeout, e => e.Displayed && e.Enabled);
        }

        public static IWebElement GetElement(this ISearchContext element, ElementLocator locator, Func<IWebElement, bool> condition)
        {
            return GetElement(element, locator, BaseConfiguration.LongTimeout, condition);
        }

        public static IWebElement GetElement(this ISearchContext element, ElementLocator locator, double timeout)
        {
            return element.GetElement(locator, timeout, e => e.Displayed & e.Enabled);
        }

        public static IWebElement GetElement(this ISearchContext element, ElementLocator locator, double timeout, Func<IWebElement, bool> condition)
        {
            var driver = ToDriver(element);

            var by = locator.ToBy();
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));

            wait.Until(x =>
            {
                var ele = element.FindElement(by);
                return condition(ele);
            });

            return element.FindElement(by);
        }

        public static IList<IWebElement> GetElements(this ISearchContext element, ElementLocator locator)
        {
            return element.FindElements(locator.ToBy()).Where(e => e.Displayed && e.Enabled).ToList();
        }

        public static IList<IWebElement> GetElements(this ISearchContext element, ElementLocator locator, Func<IWebElement, bool> condition)
        {
            return element.FindElements(locator.ToBy()).Where(condition).ToList();
        }

        public static IList<IWebElement> GetElements(this ISearchContext element, ElementLocator locator, int minNumberOfElements)
        {
            IList<IWebElement> elements = null;

            WaitHelper.Wait(
                () => (elements = GetElements(element, locator, e => e.Displayed && e.Enabled).ToList()).Count >= minNumberOfElements,
                TimeSpan.FromSeconds(BaseConfiguration.LongTimeout),
                "Timeout while getting elements");

            return elements;
        }

        public static IWebDriver ToDriver(this ISearchContext element)
        {
            var wrapsDriver = element as IWrapsDriver;
            return wrapsDriver == null ? (IWebDriver)element : wrapsDriver.WrappedDriver;
        }

        public static T GetElement<T>(this ISearchContext searchContext, ElementLocator locator)
            where T : class, IWebElement
        {
            IWebElement webElemement = searchContext.GetElement(locator);
            return webElemement.As<T>();
        }

        public static T GetElement<T>(this ISearchContext searchContext, ElementLocator locator, Func<IWebElement, bool> condition)
            where T : class, IWebElement
        {
            IWebElement webElemement = searchContext.GetElement(locator, condition);
            return webElemement.As<T>();
        }

        public static T GetElement<T>(this ISearchContext searchContext, ElementLocator locator, double timeout)
            where T : class, IWebElement
        {
            IWebElement webElemement = searchContext.GetElement(locator, timeout);
            return webElemement.As<T>();
        }

        private static T As<T>(this IWebElement webElement)
            where T : class, IWebElement
        {
            var constructor = typeof(T).GetConstructor(new[] { typeof(IWebElement) });

            if (constructor != null)
            {
                return constructor.Invoke(new object[] { webElement }) as T;
            }

            throw new ArgumentNullException($"Constructor for type {typeof(T)} is null.");
        }
    }
}
