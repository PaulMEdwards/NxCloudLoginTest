using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;
using FluentAssertions;
using System.Collections.ObjectModel;

namespace SpecFlowTestsProject.AUT {
    [Binding]
    public class LoginSteps : Common {
        By byDivAppHeaderLeft = By.XPath("//div[@class='app-header-left']");
        By byLogo = By.XPath(".//img[contains(@src,'/static/images/logo.png')]");

        By byDivAppHeaderRight = By.XPath("//div[@class='app-header-right']");
        By byLogInLink = By.XPath(".//a[@ng-click='login()' and text()='Log In']");
        By byCreateAccountLink = By.XPath(".//a[@href='/register' and text()='Create Account']");

        By byLogInForm = By.XPath("//form[@name='loginForm']");
        By byLogInEmail = By.XPath(".//input[@id='login_email']");
        By byLogInPassword = By.XPath(".//input[@id='login_password']");
        By byLogInRemember = By.XPath(".//input[@type='checkbox' and @id='remember']");
        By byLogInButton = By.XPath(".//button[text()='Log In']");

        By byAccountSelect = By.XPath("//nx-account-settings-select");
        By byAccountSelectDropDown = By.XPath(".//ul[@aria-labelledby='accountSettingsSelect']");

        By byNoSystemsConnected = By.XPath("//span[contains(text(),'You have no Systems connected to Nx Cloud')]");

        
        [Given(@"I visit the NxVMS landing page")]
        public void GivenIVisitTheNxVMSpage() {
            string url = "http://nxvms.com/";
            Navigate(url);

            WaitForElementToAppear(byLogo);

            webDriver.Url.StartsWith("https").Should().BeTrue();

            //Dismiss survey if promtped
            IWebElement s = WaitForElementToAppear(By.XPath("//div[@id='_hj_survey_invite_container']"), 5);
            if (s != null) {
                s.FindElement(By.LinkText("No thank you.")).Click();
            }

            ReadOnlyCollection<IWebElement> CreateAccountButtons = webDriver.FindElements(byCreateAccountLink);
            if (CreateAccountButtons.Count == 0) {  //Already logged in
                ThenTheUserCanLogoutAndReturnToTheLandingPage();
            }

            WaitForElementToAppear(byLogInLink);

            CheckForAndReportBrowserConsoleLogErrors();
        }

        [Given(@"I access the Log In form")]
        public void GivenIAccessTheLogInForm() {
            IWebElement h = WaitForElementToAppear(byDivAppHeaderRight);
            h.FindElement(byLogInLink).Click();
        }

        [Given(@"I input ""(.*)"" as username and ""(.*)"" as password")]
        public void GivenIInputAsUsernameAndAsPassword(string user, string pass) {
            IWebElement f = WaitForElementToAppear(byLogInForm);
            IWebElement userName = f.FindElement(byLogInEmail);
            IWebElement password = f.FindElement(byLogInPassword);

            userName.SendKeys(user);
            password.SendKeys(pass);
        }

        [Given(@"I set the remember checkbox to ""(.*)""")]
        public void GivenISetTheRememberCheckboxTo(string checkRemember = "false") {
            IWebElement f = WaitForElementToAppear(byLogInForm);
            IWebElement r = f.FindElement(byLogInRemember);

            bool remember = false;
            bool.TryParse(checkRemember, out remember);

            if (r.Selected != remember)
                r.SendKeys(" ");
        }

        [When(@"I click the Log In button")]
        public void WhenIClickTheLoginButton() {
            IWebElement f = WaitForElementToAppear(byLogInForm);
            IWebElement l = f.FindElement(byLogInButton);
            l.Click();
        }

        [Then(@"the Systems page should load without error")]
        public void ThenTheSystemsPageShouldLoadWithoutError() {
            ConfirmLoggedIn(true);

            CheckForAndReportBrowserConsoleLogErrors();
        }

        [Then(@"the user can logout and return to the landing page")]
        public void ThenTheUserCanLogoutAndReturnToTheLandingPage() {
            IWebElement a = WaitForElementToAppear(byAccountSelect);
            IWebElement s = a.FindElement(byAccountSelectDropDown);

            if (!s.Displayed)
                a.Click();

            IWebElement l = s.FindElement(By.LinkText("Log Out"));
            l.Click();

            CheckForAndReportBrowserConsoleLogErrors();
        }

        [Then(@"the Systems page should NOT load")]
        public void ThenTheSystemsPageShouldNOTLoad() {
            ConfirmLoggedIn(false);

            CheckForAndReportBrowserConsoleLogErrors();
        }


        private void ConfirmLoggedIn(bool expectedCondition = true) {
            IWebElement e = null;
            if (expectedCondition) {
                e = WaitForElementToAppear(byNoSystemsConnected);
                e.Displayed.Should().Be(true);
            } else {
                e = WaitForElementToAppear(byNoSystemsConnected, 10);
                e.Should().BeNull("We should not be able to log in, yet it appears that we have.");
            }
        }
        
        public IWebElement WaitForElementToAppear(By ByIdentifier, int timeoutInSeconds = 30) {
            IWebElement e = null;
            WebDriverWait wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(timeoutInSeconds));

            Func<IWebDriver, bool> waitForElement = new Func<IWebDriver, bool>((IWebDriver wD) => {
                e = wD.FindElement(ByIdentifier);
                return e.Displayed;
            });

            try {
                wait.Until(waitForElement);
            } catch { }

            return e;
        }

        public bool CheckForAndReportBrowserConsoleLogErrors(bool failOnErrors = false) {
            ILogs logsObject = webDriver.Manage().Logs;
            ReadOnlyCollection<LogEntry> logsCollection = logsObject.GetLog("browser");

            bool errors = false;

            foreach (LogEntry logEntry in logsCollection) {
                if (logEntry.Level == LogLevel.Severe) {
                    errors = true;
                    Console.WriteLine("Severe Browser Errors enountered:");
                    break;
                }
            }

            foreach (LogEntry logEntry in logsCollection) {
                if (logEntry.Level == LogLevel.Severe) {
                    Console.WriteLine(string.Format("{0}: {1}: {2}", logEntry.Timestamp, logEntry.Level, logEntry.Message));
                }
            }

            if (failOnErrors)
                errors.Should().BeFalse();

            return errors;
        }
    }
}
