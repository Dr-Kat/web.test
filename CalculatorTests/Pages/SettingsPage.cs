using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        public SelectElement DateFormatDrdwn => new SelectElement(_driver.FindElement(By.XPath($"//td[contains(text(),'Date format:')]/ ..//select")));
        public SelectElement NumberFormatDrdwn => new SelectElement(_driver.FindElement(By.XPath($"//td[contains(text(),'Number format:')]/ ..//select")));
        public SelectElement DefaultCurrencyDrdwn => new SelectElement(_driver.FindElement(By.XPath($"//th[contains(text(),'Default currency:')]/ ..//select")));

        public string DateFormatValue => _driver.FindElement(By.XPath($"//td[contains(text(),'Date format:')]/ ..//select")).GetAttribute("value");
        public string NumberFormatValue => _driver.FindElement(By.XPath($"//td[contains(text(),'Number format:')]/ ..//select")).GetAttribute("value");
        public string DefaultCurrencyValue => _driver.FindElement(By.XPath($"//th[contains(text(),'Defalut currency:')]/ ..//select")).GetAttribute("value");

        public (bool IsSuccessful, string Text) SettingsSave()
        {
            SaveBtn.Click();
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
