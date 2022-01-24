using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace CalculatorTests.Pages
{
    public class SettingsPage
    {
        private IWebDriver _driver;

        public SettingsPage(IWebDriver driver)
        {
            _driver = driver;
        }
        public IWebElement SaveBtn => _driver.FindElement(By.XPath($"//button[contains(text(),'Save')]"));
        public IWebElement CancelBtn => _driver.FindElement(By.XPath($"//button[contains(text(),'Cancel')]"));


        public SelectElement DateFormatDrdwn => new SelectElement(_driver.FindElement(By.XPath($"//th[contains(text(),'Date format:')]/ ..//select[@id='dateFormat']")));
        public SelectElement NumberFormatDrdwn => new SelectElement(_driver.FindElement(By.XPath($"//th[contains(text(),'Number format:')]/ ..//select[@id='numberFormat']")));
        public SelectElement DefaultCurrencyDrdwn => new SelectElement(_driver.FindElement(By.XPath($"//th[contains(text(),'Default currency:')]/ ..//select[@id='currency']")));

        public string DateFormatValue => _driver.FindElement(By.XPath($"//th[contains(text(),'Date format:')]/ ..//select[@id='dateFormat']")).GetAttribute("value");
        public string NumberFormatValue => _driver.FindElement(By.XPath($"//th[contains(text(),'Number format:')]/ ..//select[@id='numberFormat']")).GetAttribute("value");
        public string DefaultCurrencyValue => _driver.FindElement(By.XPath($"//th[contains(text(),'Default currency:')]/ ..//select[@id='currency']")).GetAttribute("value");

        public string  SettingsSave()
        {
            SaveBtn.Click();
            {
                new WebDriverWait(_driver, TimeSpan.FromSeconds(2))
                 .Until(ExpectedConditions.AlertIsPresent());
                IAlert alert = _driver.SwitchTo().Alert();
                string result = alert.Text;
                alert.Accept();
                _driver.SwitchTo().ParentFrame();
                return result;
            }
}


        public string DateFormat
        {
            get
            {
                string dateFormat = DateFormatDrdwn.SelectedOption.Text;

                return dateFormat;
            }
            set // set(value)
            {
                string dateFormat = value;

                DateFormatDrdwn.SelectByText(dateFormat); 
            }
        }
        public string NumberFormat
        {
            get
            {
                string numberFormat = NumberFormatDrdwn.SelectedOption.Text;

                return numberFormat;
            }
            set // set(value)
            {
                string numberFormat = value;
                NumberFormatDrdwn.SelectByText(numberFormat);
            }
        }
        public string CurrencyFormat
        {
            get
            {
                string currencyFormat = DefaultCurrencyDrdwn.SelectedOption.Text;

                return currencyFormat;
            }
            set // set(value)
            {
                string currencyFormat = value;
                DefaultCurrencyDrdwn.SelectByText(currencyFormat);
            }
        }
    }
}
