using System;
using CalculatorTests.Pages;
using NUnit.Framework;

namespace CalculatorTests
{
    class CalculatorPageTests : BaseTest
    {
        private LoginPage loginPage;
        private CalculatorPage calculatorPage;

        [SetUp]
        public void OpenLoginPage()
        {
            Driver = GetDriver();
            Driver.Url = BaseUrl;

            loginPage = new LoginPage(Driver);
            loginPage.Login("test", "newyork1");
        }

        [TestCase ("1000", "25", "365", "365", "1 250,00", "250,00")]
        [TestCase("1000", "25", "360", "365", "1 246,58", "246,58")]
        [TestCase("1000", "25", "25", "365", "1 017,12", "17,12")]
        [TestCase("1000", "25", "360", "360", "1 250,00", "250,00")]
        [TestCase("1000", "25", "1", "360", "1 000,69", "0,69")]
        public void CalculateTest(string amount, string rate, string term, string financialYear, string expectedIncom, string expectedInterest)
        {
            // Arrange
            // Income = Amount/100*Rate * Term/FinYear

            // Act
            calculatorPage = new CalculatorPage(Driver);
            calculatorPage.Calculate(amount,rate,term,financialYear);
            string income = calculatorPage.IncomeFld.GetAttribute("value");
            string interest = calculatorPage.InterestFld.GetAttribute("value");

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedIncom, income);
                Assert.AreEqual(expectedInterest, interest);
            });
        }

        [TestCase("* - mandatory fields", "mandatory")]
        public void Deposit_Texts(string expectedText, string actualText)
        {
            // Arrange
            calculatorPage = new CalculatorPage(Driver);
            Driver.Url = $"{BaseUrl}/Calculator";

            // Assert

            Assert.AreEqual(expectedText, calculatorPage.GetLabelText(actualText));
        }

        [Test]
        public void LogoutLink()
        {
            calculatorPage = new CalculatorPage(Driver);
            calculatorPage.LogoutLink.Click();

            // Assert
            string currentURL = Driver.Url;
            Assert.AreEqual(BaseUrl+'/', currentURL);
        }
        // After Logout button is removed from Settings to $"{BaseUrl}/Deposit", Test need to be update

        //[Test]
        //public void Logout_pageback()
        //{
        //    // Arrange
        //    Driver.Url = $"{BaseUrl}/Login";

        //    // Act
        //    Driver.FindElement(By.LinkText("Settings")).Click();
        //    Driver.FindElement(By.LinkText("Logout")).Click();

        //    // Assert
        //    string currentURL = Driver.Url;
        //    Assert.AreEqual($"{BaseUrl}/Deposit", currentURL);
        //}

        [TestCase("", "25", "365", "365")]
        [TestCase("1000","","365", "365")]
        [TestCase("1000", "25", "", "365")]
        public void MandatoryFields(string depamount, string rateinterest, string investTerm, string financialYear)
        {
            // Act
            calculatorPage = new CalculatorPage(Driver);
            calculatorPage.DepAmountFld.SendKeys(depamount);
            calculatorPage.RateInterestFld.SendKeys(rateinterest);
            calculatorPage.InvestTermFld.SendKeys(investTerm);
            calculatorPage.FinancialYear = int.Parse(financialYear);
            string interest = calculatorPage.InterestFld.GetAttribute("value");

            // Assert          
            Assert.AreEqual("0,00", interest);
        }

        [TestCase("1/1/2022", "365")]
        [TestCase("1/1/2022", "360")]
        public void InterestCalculationDependingFromFinancialYear(string date, string financialYear)
        {
            // Act
            calculatorPage = new CalculatorPage(Driver);
            calculatorPage.StartDate = date;
            string interest = calculatorPage.InterestFld.GetAttribute("value");

            // Assert          
            Assert.AreEqual("0,00", interest);
        }

        [TestCase("1/1/2024", "60", "01/03/2024")]
        [TestCase("1/1/2023", "60", "02/03/2023")]
        [TestCase("10/12/2023", "120", "08/04/2024")]
        public void EndDateCalculation(string date, string term, string result)
        {
            // Act
            calculatorPage = new CalculatorPage(Driver);
            calculatorPage.StartDate = date;
            calculatorPage.Calculate("100", "1", term, "365");

            // Assert
            Assert.AreEqual(result, calculatorPage.EndDate);          
        }

        [Test]

        public void StartDateDefaultValue()
        {
            calculatorPage = new CalculatorPage(Driver);
            string defaultValue = calculatorPage.StartDate; // StartDate = get

            Assert.AreEqual(DateTime.Today.ToString("d/M/yyyy"),defaultValue);
        }

        [Test]

        public void StartDateEarlierThenToday()
        {
            calculatorPage = new CalculatorPage(Driver);
            var startDate = DateTime.Today.AddDays(-1).ToString("d/MM/yyyy"); /// .Date doesn't work
            calculatorPage.StartDate = startDate;
           
           Assert.AreEqual(DateTime.Today.ToString("d/M/yyyy"), calculatorPage.StartDate);
        }

        [Test]
        public void MandatoryFieldFinanceYear()
        {
            // Act
            calculatorPage = new CalculatorPage(Driver);
            int defaultFinancialYear = calculatorPage.FinancialYear;

            // Assert          
            Assert.AreEqual(365, defaultFinancialYear);
        }

        [Test]
        public void MaxValueValidation()
        {
            // Act
            calculatorPage = new CalculatorPage(Driver);
            calculatorPage.DepAmountFld.SendKeys("100001");
            calculatorPage.RateInterestFld.SendKeys("101");
            calculatorPage.InvestTermFld.SendKeys("366");

            // Assert
            Assert.Multiple(() =>
                {
                    Assert.AreEqual("100000", calculatorPage.DepAmountFld.GetAttribute("value"));
                    Assert.AreEqual("100", calculatorPage.RateInterestFld.GetAttribute("value"));
                    Assert.AreEqual("365", calculatorPage.InvestTermFld.GetAttribute("value"));
                }
           );
        }

        [Test]
        public void MaxTerm_equal_FinancialYear360()
        {
            // Act
            calculatorPage = new CalculatorPage(Driver);
            calculatorPage.DepAmountFld.SendKeys("1000");
            calculatorPage.RateInterestFld.SendKeys("25");
            calculatorPage.InvestTermFld.SendKeys("365");
            calculatorPage.FinancialYear = 360;
            string finalInvestTerm = calculatorPage.InvestTermFld.GetAttribute("value");

            // Assert
            Assert.AreEqual("360", finalInvestTerm);
        }

        [Test]
        public void MaxTerm_equal_FinancialYear365()
        {
            // Act
            calculatorPage = new CalculatorPage(Driver);
            calculatorPage.DepAmountFld.SendKeys("1000");
            calculatorPage.RateInterestFld.SendKeys("25");
            calculatorPage.InvestTermFld.SendKeys("366");
            calculatorPage.FinancialYear = 365;
            string finalInvestTerm = calculatorPage.InvestTermFld.GetAttribute("value");

            // Assert
            Assert.AreEqual("365", finalInvestTerm);
        }
    }
}
