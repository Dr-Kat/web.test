using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.Extensions;

namespace CalculatorTests
{
    public class BaseTest
    {
        protected IWebDriver Driver;
        protected string BaseUrl => ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location).AppSettings.Settings["BaseUrl"].Value;

        protected IWebDriver GetDriver()
        {
            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;
            chromeDriverService.SuppressInitialDiagnosticInformation = true;

            var options = new ChromeOptions
            {
                UnhandledPromptBehavior = UnhandledPromptBehavior.Ignore,
                AcceptInsecureCertificates = true
            };
            options.AddArgument("--silent");
            options.AddArgument("log-level=3");

            IWebDriver driver = new ChromeDriver(chromeDriverService, options);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(60);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);

            return driver;
        }

        [TearDown]
        public void Close()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                var test = TestContext.CurrentContext.Test;
                var folder = $"{Environment.CurrentDirectory}/ScreenShots";
                var fileName = $"{folder}/{test.ClassName}_{test.MethodName}_{DateTime.Now:mmss}.png";

                Directory.CreateDirectory(folder);
                Driver.TakeScreenshot().SaveAsFile(fileName, ScreenshotImageFormat.Png);
                TestContext.AddTestAttachment(fileName);
            }

            Driver.Quit();
        }
    }
}
