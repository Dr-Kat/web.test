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
    public class CalculatorPage
    {
        private IWebDriver _driver;

        public CalculatorPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public IWebElement DepAmountFld
        {
            get
            {
                return _driver.FindElement(By.XPath($"//td[contains(text(),'Deposit Amount: *')]/ ..//input"));
            }
        }
        public IWebElement DepAmountSign => _driver.FindElement(By.XPath($"//td[contains(text(),'Deposit Amount: *')]/ ..//td[3]"));


        public IWebElement RateInterestFld => _driver.FindElement(By.XPath($"//td[contains(text(),'Rate of interest: *')]/ ..//input"));

        public IWebElement InvestTermFld => _driver.FindElement(By.XPath($"//td[contains(text(),'Investment Term: *')]/ ..//input"));

        public SelectElement DateDayDrdwn => new SelectElement(_driver.FindElement(By.XPath($"//td[contains(text(),'Start date: *')]/ ..//select[@id='day']")));

        public SelectElement DateMonthDrdwn => new SelectElement(_driver.FindElement(By.XPath($"//td[contains(text(),'Start date: *')]/ ..//select[@id='month']")));

        public SelectElement DateYearDrdwn => new SelectElement(_driver.FindElement(By.XPath("//td[contains(text(),'Start date: *')]/ ..//select[@id='year']")));
        //10/08/2022
        public string StartDate
        {
            get
            {
                string day = DateDayDrdwn.SelectedOption.Text;
                var month = DateTime.ParseExact(DateMonthDrdwn.SelectedOption.Text, "MMMM", CultureInfo.InvariantCulture).Month;
                string year = DateYearDrdwn.SelectedOption.Text;

                return $"{day}/{month}/{year}";
            }
            set // set(value)
            {
                string day = value.Split('/')[0];
                int month = int.Parse(value.Split('/')[1]) - 1;
                string year = value.Split('/')[2];

                DateDayDrdwn.SelectByText(day);
                DateMonthDrdwn.SelectByIndex(month);
                DateYearDrdwn.SelectByText(year);
            }
        }

        public int FinancialYear
        {
            //Int x = FinancialYear
            get
            {
                if (_driver.FindElement(By.XPath($"//td[contains(text(),'Financial year: *')]/ ..//input[@id='d365']")).Selected)
                    return 365;
                if (_driver.FindElement(By.XPath($"//td[contains(text(),'Financial year: *')]/ ..//input[@id='d360']")).Selected) 
                    return 360;
                return 0;
            }
            //FinancialYear = value
            set
            {
                if (value == 365)
                    _driver.FindElement(By.XPath($"//td[contains(text(),'Financial year: *')]/ ..//input[@id='d365']")).Click();
                else _driver.FindElement(By.XPath($"//td[contains(text(),'Financial year: *')]/ ..//input[@id='d360']")).Click();
            }
        }

        public IWebElement IncomeFld => _driver.FindElement(By.XPath($"//th[contains(text(),'Income:')]/ ..//input"));

        public IWebElement InterestFld => _driver.FindElement(By.XPath($"//th[contains(text(),'Interest earned:')]/ ..//input"));

        public string EndDate => _driver.FindElement(By.XPath($"//th[contains(text(),'End date:')]/ ..//input")).GetAttribute("value");

        public string GetLabelText(string actualText)
        {
            return _driver.FindElement(By.XPath($"//*[contains (text(),'{actualText}')]")).Text;
        }

        public void Calculate(string amount, string rate, string term, string financialYear)
        {
            DepAmountFld.SendKeys(amount);
            RateInterestFld.SendKeys(rate);
            InvestTermFld.SendKeys(term);
            FinancialYear = int.Parse(financialYear);
        }

        public IWebElement SettingsLink => _driver.FindElement(By.XPath($"//div[contains (text(),'Settings')]"));
        public void Settings()
        {
            SettingsLink.Click();
        }
    }
}
