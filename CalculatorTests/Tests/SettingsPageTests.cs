using CalculatorTests.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        [TestCase("DD/MM/YYYY")]
        public void SettingsSavedTextAllert(string dateFormat)
        {
            settingsPage = new SettingsPage(Driver);
            settingsPage.DateFormat = dateFormat;
            settingsPage.SaveBtn.Click();
            string alertText = Driver.SwitchTo().Alert().Text;

            Assert.AreEqual($"Changes are saved!", alertText);
        }

        [Test]
        public void SettingsCancel()
        {
            settingsPage = new SettingsPage(Driver);
            string defaultDateFormat = settingsPage.DateFormatValue;
            settingsPage.DateFormat = "DD/MM/YYYY";
            settingsPage.CancelBtn.Click();
            calculatorPage.Settings();

            Assert.AreEqual(defaultDateFormat, settingsPage.DateFormatValue);
        }

        [TestCase("DD/MM/YYYY", "123,456,789.00", "$ - US Dollar")]
        [TestCase("DD-MM-YYYY", "123.456.789,00", "€ - Euro")]
        [TestCase("MM/DD/YYYY", "123 456 789.00", "£ - Great Britain Pound")]
        [TestCase("MMM DD YYYY", "123 456 789,00", "£ - Great Britain Pound")]

        public void SettingsSaved(string dateFormat, string numberFormat, string defaultCurrency)
        {
            settingsPage = new SettingsPage(Driver);
            settingsPage.DateFormat = dateFormat;
            settingsPage.NumberFormat = numberFormat;
            settingsPage.CurrencyFormat = defaultCurrency;
            //settingsPage.SaveBtn.Click();
            //Driver.SwitchTo().Alert().Accept();
            settingsPage.SettingsSave();
            //Driver.SwitchTo().Alert().Accept();
            calculatorPage.Settings();

            Assert.Multiple(() =>
            {
                Assert.AreEqual(dateFormat, settingsPage.DateFormatValue);
                Assert.AreEqual(defaultCurrency, settingsPage.DefaultCurrencyValue);
                Assert.AreEqual(numberFormat, settingsPage.NumberFormatValue); ///////////////              
            });

            //string alertText = Driver.SwitchTo().Alert().Text;

            // string defaultValue = calculatorPage.StartDate; // StartDate = get

            //Assert.AreEqual($"Changes are saved!", alertText);
            //Assert.AreEqual(DateTime.Today.ToString("d/M/yyyy"), defaultValue);


            //// Act
            //LoginPage loginPage = new LoginPage(Driver);
            //loginPage.OpenRemindPasswordView();
            //loginPage.RemindPass("test@test.com");
            //string alertText = Driver.SwitchTo().Alert().Text;

            //// Assert
            //Assert.AreEqual($"Email with instructions was sent to test@test.com", alertText);
            //Driver.SwitchTo().Alert().Accept();

            {
                // Act
                //    calculatorPage = new CalculatorPage(Driver);
                //    calculatorPage.StartDate = date;
                //    string interest = calculatorPage.InterestFld.GetAttribute("value");

                //    // Assert          
                //    Assert.AreEqual("0.00", interest);
            }
        }

        //[TestCase(currentDate)]
        //[TestCase("90236")]
        //[TestCase(Euro)]
        public void SettingsChanged()
        {



        }

        [TestCase("$ - US dollar", "$")]
        [TestCase("€ - Euro", "€")]
        [TestCase("£ - Great Britain Pound", "£")]
        public void CheckSignDefaultCurrencyChanged( string defaultCurrency, string result)
        {
            settingsPage = new SettingsPage(Driver);
            settingsPage.CurrencyFormat = defaultCurrency;
            settingsPage.SettingsSave();

            Assert.AreEqual(result, calculatorPage.DepAmountSign.Text);

        }
    }
}