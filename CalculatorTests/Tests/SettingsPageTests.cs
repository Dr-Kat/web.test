using CalculatorTests.Pages;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Globalization;

namespace CalculatorTests
{
    class SettingsPageTests : BaseTest
    {
        private LoginPage loginPage;
        private CalculatorPage calculatorPage;
        private SettingsPage settingsPage;

        [SetUp]
        public void OpenLoginPage()
        {
            Driver = GetDriver();
            Driver.Url = BaseUrl;

            loginPage = new LoginPage(Driver);
            loginPage.Login("test", "newyork1");

            calculatorPage = new CalculatorPage(Driver);
            calculatorPage.Settings();
        }

        [TestCase("dd/MM/yyyy")]
        public void SettingsSavedTextAllert(string dateFormat)
        {
            settingsPage = new SettingsPage(Driver);
            settingsPage.DateFormat = dateFormat;
            settingsPage.SaveBtn.Click();
            new WebDriverWait(Driver, TimeSpan.FromSeconds(2))
                 .Until(ExpectedConditions.AlertIsPresent());
            string alertText = Driver.SwitchTo().Alert().Text;

            Assert.AreEqual($"Changes are saved!", alertText);
        }

        [Test]
        public void SettingsCancel()
        {
            settingsPage = new SettingsPage(Driver);
            string defaultDateFormat = settingsPage.DateFormatValue;
            settingsPage.DateFormat = "dd/MM/yyyy";
            settingsPage.CancelBtn.Click();
            calculatorPage.Settings();

            Assert.AreEqual(defaultDateFormat, settingsPage.DateFormatValue);
        }

        [TestCase("dd/MM/yyyy", "123,456,789.00", "$ - US Dollar")]
        [TestCase("dd-MM-yyyy", "123.456.789,00", "€ - Euro")]
        [TestCase("MM/dd/yyyy", "123 456 789.00", "£ - Great Britain Pound")]
        [TestCase("MM dd yyyy", "123 456 789,00", "₴ - Ukrainian Hryvnia")]

        public void SettingsSaved(string dateFormat, string numberFormat, string defaultCurrency)
        {
            settingsPage = new SettingsPage(Driver);
            settingsPage.DateFormat = dateFormat;
            settingsPage.NumberFormat = numberFormat;
            settingsPage.CurrencyFormat = defaultCurrency;;
            settingsPage.SettingsSave();
            calculatorPage.Settings();

            Assert.Multiple(() =>
            {
                Assert.AreEqual(dateFormat, settingsPage.DateFormatValue);
                Assert.AreEqual(defaultCurrency, settingsPage.DefaultCurrencyValue);
                Assert.AreEqual(numberFormat, settingsPage.NumberFormatValue);             
            });
        }

        [TestCase("$ - US Dollar", "$")]
        [TestCase("€ - Euro", "€")]
        [TestCase("£ - Great Britain Pound", "£")]
        [TestCase("₴ - Ukrainian Hryvnia", "₴")]
        public void CheckSignDefaultCurrencyChanged( string defaultCurrency, string result)
        {
            settingsPage = new SettingsPage(Driver);
            settingsPage.CurrencyFormat = defaultCurrency;
            settingsPage.SettingsSave();

            Assert.AreEqual(result, calculatorPage.DepAmountSign.Text);

        }

        [TestCase("dd/MM/yyyy")]
        [TestCase("dd-MM-yyyy")]
        [TestCase("MM/dd/yyyy")]
        [TestCase("MM dd yyyy")]
        public void CheckCalculatorEndDateChangedFormat(string dateFormat)
        {
            settingsPage = new SettingsPage(Driver);
            settingsPage.DateFormat = dateFormat;
            settingsPage.SettingsSave();
            calculatorPage.Calculate("100000","100","365","365");
            string endDate = calculatorPage.EndDate;
            //var actual = calculatorPage.EndDate;
            //var expected = DateTime.Today.AddMonths(12).ToString(dateFormat);
            //var numberFormatGenral = string.Format();


            Assert.Multiple(() =>
            {
                Assert.AreEqual(DateTime.Today.AddMonths(12).Date, DateTime.ParseExact(endDate, dateFormat, CultureInfo.InvariantCulture).Date);
                //Assert.AreEqual(expected, actual);
            });     
        }
        [TestCase("123,456,789.00", "100,000.00", "200,000.00")]
        [TestCase("123.456.789,00", "100.000,00", "200.000,00")]
        [TestCase("123 456 789.00", "100 000.00", "200 000.00")]
        [TestCase("123 456 789,00", "100 000,00", "200 000,00")]
        public void CheckCalculatorValuesFieldsChangedFormat(string numberFormat, string interestFormatExpected, string incomeFormatExpected)
        {
            settingsPage = new SettingsPage(Driver);
            settingsPage.NumberFormat = numberFormat;
            settingsPage.SettingsSave();
            calculatorPage.Calculate("100000", "100", "365", "365");
            string interestFormat = calculatorPage.InterestFld.GetAttribute("value");
            string incomeFormat = calculatorPage.IncomeFld.GetAttribute("value");

            Assert.Multiple(() =>
            {
                Assert.AreEqual(interestFormatExpected, interestFormat);
                Assert.AreEqual(incomeFormatExpected, incomeFormat);
            });
        }
    }

}