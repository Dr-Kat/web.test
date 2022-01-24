using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace CalculatorTests.Pages
{
    public class LoginPage
    {
        private IWebDriver _driver;
        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public IWebElement LoginFld
        {
            get
            {
                return _driver.FindElement(By.Id("login"));
            }
        }

        public IWebElement PasswordFld
        {
            get // IWebElement get()
            {
                return _driver.FindElement(By.Id("password"));
            }
        }

        public IWebElement LoginBtn => _driver.FindElement(By.Id("loginBtn"));
        public IWebElement RemindBtn => _driver.FindElement(By.Id("remindBtn"));

        public string Error => _driver.FindElement(By.Id("errorMessage")).Text;
        public string UserFieldText => _driver.FindElement(By.ClassName("user")).Text;
        public string PasswordFieldText => _driver.FindElement(By.ClassName("pass")).Text;

        public void Login(string login, string password)
        {
            LoginFld.SendKeys(login);
            PasswordFld.SendKeys(password);
            LoginBtn.Click();
            new WebDriverWait(_driver, TimeSpan.FromSeconds(3))
                .Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("//*[@id='calculateBtn' or @id ='errorMessage']")));
        }

        public IWebDriver OpenRemindPasswordView()
        {
            _driver.FindElement(By.Id("remindBtn")).Click();
            new WebDriverWait(_driver, TimeSpan.FromSeconds(3))
                .Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("//*[@id='remindPasswordView']")));
            return _driver.SwitchTo().Frame("remindPasswordView");
        }

        public void RemindPass(string email)
        {
            _driver.FindElement(By.Id("email")).SendKeys(email);
            _driver.FindElement(By.XPath("//button[contains(text(),'Send')]")).Click();
        }

        public (bool IsSuccessful, string Text) RemindPass2(string email)
        {
            _driver.FindElement(By.Id("email")).SendKeys(email);
            _driver.FindElement(By.XPath("//button[contains(text(),'Send')]")).Click();
            try
            {
                IAlert alert = _driver.SwitchTo().Alert();
                string result = alert.Text;
                alert.Accept();
                _driver.SwitchTo().ParentFrame();
                return (true, result);
            }
            catch
            {
                string result = _driver.FindElement(By.Id("message")).Text;
                _driver.SwitchTo().ParentFrame();
                return (false, result);
            }
        }
    }
}
