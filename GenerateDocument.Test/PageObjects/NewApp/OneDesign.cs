using GenerateDocument.Common;
using GenerateDocument.Common.Extensions;
using GenerateDocument.Common.Types;
using GenerateDocument.Test.WrapperFactory;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace GenerateDocument.Test.PageObjects.NewApp
{
    public class OneDesign : PageBaseObject
    {
        private ElementLocator
            _modalOpen = new ElementLocator(Locator.XPath, "//body[@class='modal-open']"),
            _surveyWindow = new ElementLocator(Locator.XPath, "//iframe[@id='surveyWindow']"),
            _takeTheSurveyButton = new ElementLocator(Locator.XPath, "//button[@ng-click='openSurveyForm()']"),
            _surveySubmitBtn = new ElementLocator(Locator.Id, "surveySubmitBtn"),
            _lastPage = new ElementLocator(Locator.Id, "lastPage"),
            _closeModalBtn = new ElementLocator(Locator.Id, "closeModalBtn"),
            _mopinionActiveClass = new ElementLocator(Locator.ClassName, "mopinion-slider-active"),
            _completedModalCloseBtn = new ElementLocator(Locator.XPath, "//a[@ng-click='$dismiss()']"),
            _downloadDesignButtons = new ElementLocator(Locator.XPath, "//div[@id='componentInfo']//ul[@class='product-activities']//li//button[@ng-click='downloadDesign(action.jobName)']");

        public OneDesign(DriverContext driverContext) : base(driverContext)
        {
        }

        public List<IWebElement> GetDownloadDesignButtons()
        {
            return DriverContext.Driver.GetElements(_downloadDesignButtons, 1).ToList();
        }

        public string GetDesignName(string expectedName = null)
        {
            if (string.IsNullOrEmpty(expectedName))
            {
                return DriverContext.BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@class='product-title']//h2"))).Text;
            }
            else
            {
                var designName = string.Empty;

                void getDesignName()
                {
                    var i = 0;
                    do
                    {
                        designName = DriverContext.BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@class='product-title']//h2"))).Text;

                        if (designName.IsEquals(expectedName))
                        {
                            break;
                        }

                        Thread.Sleep(1000);
                        i++;
                    } while (i < 3);
                }

                getDesignName();

                return designName;
            }
        }

        public string GetSelectedComponentName(bool isKit)
        {
            if (!isKit)
                return string.Empty;

            return DriverContext.BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@class='dropdown']//button"))).Text.Trim();
        }

        public void DownloadDesign(bool needToPublishFirst, ref bool isShowSurveyInvitationModal, string designName = null)
        {
            var downloadButtons = GetDownloadDesignButtons();

            int account = 0;
            foreach (var button in downloadButtons)
            {
                Driver.ScrollToView(button);
                button.Click();

                ExitForDownloadCompletedModal();

                if (needToPublishFirst)
                {
                    needToPublishFirst = false;

                    //verify modal visibility 
                    DriverContext.BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.ClassName("modal-open")));

                    //agree publish design
                    DriverContext.BrowserWait().Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[@ng-click='createDesign()']"))).Click();

                    //verify modal is closed
                    DriverContext.BrowserWait().Until(ExpectedConditions.InvisibilityOfElementLocated(By.ClassName("modal-open")));
                }

                account++;
                TakeSurveyIfVisible(isShowSurveyInvitationModal, account);

            }
            account = -1;
            TakeSurveyIfVisible(isShowSurveyInvitationModal, account);

            if (isShowSurveyInvitationModal)
            {
                isShowSurveyInvitationModal = false;
            }

            //wait until all download buttons are show checked icon
            void makeSureAllOutputsAreDownloaded(int expectedNumber)
            {
                var downloadedButtonsCount = DriverContext.BrowserWait().Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(
                    By.XPath("//div[@id='componentInfo']//ul[@class='product-activities']//li//button[contains(@class, 'downloaded')]"))).Count;

                if (downloadedButtonsCount != expectedNumber)
                {
                    makeSureAllOutputsAreDownloaded(expectedNumber);
                }
            }

            makeSureAllOutputsAreDownloaded(downloadButtons.Count);
        }

        private void ExitForDownloadCompletedModal()
        {
            bool.TryParse(Driver.GetSessionStorage("hasShowedFeedback"), out bool hasShowedFeedback);
            Console.WriteLine($"hasShowedFeedback: {hasShowedFeedback}");
            if (hasShowedFeedback)
            {
                var modalEle = Driver.WaitUntilPresentedElement(_modalOpen, e => e.Displayed, BaseConfiguration.LongTimeout);
                if (modalEle != null)
                {
                    // Driver.SwitchTo().Frame(modalEle);
                    Driver.GetElement(_completedModalCloseBtn).Click();

                    //Driver.SwitchToParent();
                    Driver.WaitUntilElementIsNoLongerFound(_modalOpen, BaseConfiguration.ShortTimeout);
                }
            }
        }

        public void SubmitApprovalWorkflow()
        {
            DriverContext.BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@ng-click='submitForApproval()']"))).Click();

            //verify modal visibility 
            DriverContext.BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.ClassName("modal-open")));

            //agree publish design
            DriverContext.BrowserWait().Until(ExpectedConditions.ElementToBeClickable(By.XPath("//li//button[@ng-click='submitForApproval()']"))).Click();

            //verify modal is closed
            DriverContext.BrowserWait().Until(ExpectedConditions.InvisibilityOfElementLocated(By.ClassName("modal-open")));

            Thread.Sleep(2000);
        }

        public void TakeSurveyIfVisible(bool visibility, int countAccess)
        {
            var hasFeedbackCookies = Driver.CheckExistedCookie($"MSFeedbackSent{ConfigInfo.MopinionFormId}");
            Console.WriteLine($"Access take survey fun: {countAccess} times");
            Console.WriteLine($"TakeSurveyIfVisible with visibility:{visibility}, hasFeedbackCookies:{hasFeedbackCookies} ");
            if (!hasFeedbackCookies)
            {
                Driver.WaitUntilPresentedElement(_modalOpen, BaseConfiguration.LongTimeout);

                var takeTheSurveyButton = Driver.WaitUntilPresentedElement(_takeTheSurveyButton, e => e.Displayed, BaseConfiguration.LongTimeout);
                if (takeTheSurveyButton != null)
                {
                    takeTheSurveyButton.Click();

                    var surveyWindowEle = Driver.WaitUntilPresentedElement(_surveyWindow, BaseConfiguration.ShortTimeout);
                    if (surveyWindowEle != null)
                    {
                        Driver.SwitchTo().Frame(surveyWindowEle);
                        Driver.GetElement(_surveySubmitBtn).Click();

                        // Driver.WaitUntilElementIsNoLongerFound(_surveySubmitBtn, BaseConfiguration.ShortTimeout);
                        Driver.GetElement(_closeModalBtn).Click();

                        Driver.SwitchToParent();

                        Driver.WaitUntilElementIsNoLongerFound(_mopinionActiveClass, BaseConfiguration.ShortTimeout);
                    }
                }
            }
        }

        public string RenameDesign(string name)
        {
            DriverContext.BrowserWait().Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//div[contains(@class, 'fblw-timeline-item')]")));

            DriverContext.BrowserWait().Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[@ng-click='renameDesign()']"))).Click();

            DriverContext.BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.ClassName("modal-open")));

            var textField = DriverContext.BrowserWait().Until(ExpectedConditions.ElementToBeClickable(By.Name("designName")));
            textField.Clear();
            if (!string.IsNullOrEmpty(name))
            {
                textField.SendKeys(name);
            }

            //wait to renameDesign button ready for clicking
            DriverContext.BrowserWait().Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//button[contains(@class, 'disabled')]")));
            DriverContext.BrowserWait().Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[@ng-click='renameDesign()']"))).Click();

            var messageElement =
                Driver.FindElements(By.XPath("//p[contains(@class, 'text-danger')]")).FirstOrDefault();

            if (messageElement != null)
            {
                var message = messageElement.Text;

                DriverContext.BrowserWait().Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[@ng-click='$dismiss()']"))).Click();

                //verify modal is closed
                DriverContext.BrowserWait().Until(ExpectedConditions.InvisibilityOfElementLocated(By.ClassName("modal-open")));

                Driver.RefreshPage();

                return message;
            }

            //verify modal is closed
            DriverContext.BrowserWait().Until(ExpectedConditions.InvisibilityOfElementLocated(By.ClassName("modal-open")));

            Driver.RefreshPage();

            return string.Empty;
        }

        public bool CheckShowFullLessDesignNameActionLinkVisibility(bool visibilityExpectation, bool needShowFullName = true)
        {
            DriverContext.BrowserWait().Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//div[contains(@class, 'fblw-timeline-item')]")));

            if (visibilityExpectation)
            {
                if (needShowFullName)
                {
                    return DriverContext.BrowserWait().Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Show full name"))) != null;
                }

                return DriverContext.BrowserWait().Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Show less"))) != null;
            }

            return Driver.FindElements(By.LinkText("Show less")).Any();
        }

        public bool CheckShowFullLessDesignNameActionLinkBehaviors(bool needShowFullName)
        {
            DriverContext.BrowserWait().Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//div[contains(@class, 'fblw-timeline-item')]")));

            try
            {
                if (needShowFullName)
                {
                    DriverContext.BrowserWait().Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Show full name"))).Click();

                    DriverContext.BrowserWait().Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Show less"))).Click();

                    return true;
                }

                DriverContext.BrowserWait().Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Show less"))).Click();

                DriverContext.BrowserWait().Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Show full name"))).Click();

                return true;
            }
            catch
            {
                return false;
            }

        }

    }
}
