using CalculatorTests.Pages;
using NUnit.Framework;
using OpenQA.Selenium;

namespace CalculatorTests
{
    public class LoginPageTests : BaseTest
    {
        [SetUp]
        public void OpenLoginPage()
        {
            Driver = GetDriver();
            Driver.Url = BaseUrl;
        }

        [Test]
        public void Login_Label_Text()
        {
            // Assert
            LoginPage loginPage = new LoginPage(Driver);
            Assert.AreEqual("User:", loginPage.UserFieldText);
            Assert.AreEqual("Password:", loginPage.PasswordFieldText);
        }

        [TestCase("test", "password", "Incorrect login or password!")]
        [TestCase("", "", "User name and password cannot be empty!")]
        [TestCase("", "newyork1", "User name and password cannot be empty!")]
        [TestCase("test", "", "User name and password cannot be empty!")]
        [TestCase("test1", "newyork", "Incorrect login or password!")] //Login_Not_Existing_Account
        public void Failed_Login_Error_Texts(string login, string password, string expectedError)
        {
            // Act
            LoginPage loginPage = new LoginPage(Driver);
            loginPage.Login(login, password);
            // Assert
            Assert.AreEqual(expectedError, loginPage.Error);
        }

        //[Test]
        //public void Failed_Login_Attempts_Error()
        //{
        //    // Act
        //    driver.FindElement(By.Id("login")).SendKeys("test");
        //    driver.FindElement(By.Id("password")).SendKeys("password");
        //    for (int i = 0; i < 5; i++)
        //    {
        //        driver.FindElements(By.Id("login"))[1].Click();
        //    }
        //    // Assert
        //    IWebElement error = driver.FindElement(By.Id("errorMessage"));
        //    Assert.AreEqual("Failed logins more then 5 times. Please wait 15 min for next try.", error.Text);

        //    driver.Close();
        //    // it may be necessary to add a check that the user has not logged in
        //}

        [TestCase("test", "newyork1")]//success login
        [TestCase("Test", "newyork1")]//Login_Not_Case_Sensitive
        public void Success_Login(string login, string password)
        {
            LoginPage loginPage = new LoginPage(Driver);
            // Act
            loginPage.Login(login, password);

            // Assert
            Assert.AreEqual($"{BaseUrl}/Deposit", Driver.Url);
        }

        [Test]
        public void Login_Buttons_Exist()
        {         
            LoginPage loginPage = new LoginPage(Driver);

            bool loginBtnIsShown = loginPage.LoginBtn.Displayed;
            bool remindBtnIsShown = loginPage.RemindBtn.Displayed;

            // Assert
            Assert.AreEqual(true, remindBtnIsShown, "button is not displayed");  
            Assert.IsTrue(loginBtnIsShown); // = Assert.AreEqual(true, loginBtn);
        }

        [Test]
        public void Remind_Password_Success()
        {
            // Act
            LoginPage loginPage = new LoginPage(Driver);
            loginPage.OpenRemindPasswordView();
            loginPage.RemindPass("test@test.com");
            string alertText = Driver.SwitchTo().Alert().Text;

            // Assert
            Assert.AreEqual($"Email with instructions was sent to test@test.com", alertText);
            Driver.SwitchTo().Alert().Accept();
        }

        [Test]
        public void Remind_Password_Validation_Email()
        {
            LoginPage loginPage = new LoginPage(Driver);
            loginPage.OpenRemindPasswordView();
            var result = loginPage.RemindPass2("test@testcom");

            // Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual("Invalid email", result.Text);
        } 

        [Test]
        public void Remind_Password_Email_Not_Exist()
        {
            // Act
            Driver.FindElement(By.Id("remindBtn")).Click();
            Driver.SwitchTo().Frame("remindPasswordView");
            Driver.FindElement(By.Id("email")).SendKeys("test12@test.com");
            Driver.FindElement(By.XPath("//button[contains(text(),'Send')]")).Click();
            string errorText = Driver.FindElement(By.Id("message")).Text;

            // Assert
            Assert.AreEqual("No user was found", errorText);
        }

        [Test]
        public void Remind_Password_Empty_Form()
        {
            // Act
            Driver.FindElement(By.Id("remindBtn")).Click();
            Driver.SwitchTo().Frame("remindPasswordView");
            Driver.FindElement(By.Id("email")).SendKeys("test@test.com");
            Driver.FindElement(By.XPath("//button[contains(text(),'Send')]")).Click();
            Driver.SwitchTo().Alert().Accept();
            Driver.SwitchTo().DefaultContent();
            Driver.FindElement(By.Id("remindBtn")).Click();
            Driver.SwitchTo().Frame("remindPasswordView");
            string email = Driver.FindElement(By.Id("email")).GetAttribute("value");

            // Assert
            Assert.AreEqual("", email);
        }
    }
}