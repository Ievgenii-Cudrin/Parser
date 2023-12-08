using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Services
{
    public static class SiteParser
    {
        public static ChromeDriver LoginToSite()
        {
            const string baseUrl = "https://ib.psbank.ru/lk/products/summary";
            ChromeOptions options = Settings.Settings.GetChromeOptions();
            IWebDriver driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl(baseUrl);
            Thread.Sleep(3000);

            return (ChromeDriver)driver;
        }

        public static List<string> GetSecretNames(ChromeDriver driver, string phoneNumber, bool shouldReturn)
        {
            if (shouldReturn)
            {
                // go back
                PressElemByTagName(driver, "span", 3, 2000);
                //var spans = driver.FindElements(By.TagName("span"));
                //spans[3].Click();
                //Thread.Sleep(2000);

                // go to phone number entering
                PressElemByTagName(driver, "rui-icon", 5, 1000);
                //var iconst = driver.FindElements(By.TagName("rui-icon"));
                //iconst[5].Click();
                //Thread.Sleep(1000);
            } 
            else
            {
                PressElemByTagName(driver, "button", 4, 2000);
                //var buttons = driver.FindElements(By.TagName("button"));
                //buttons[4].Click();
                //Thread.Sleep(2000);
            }

            if(!shouldReturn)
            {
                var searchString = driver.FindElement(By.Name("searchString"));
                searchString.SendKeys(phoneNumber);
                Thread.Sleep(3000);

                PressElemByTagName(driver, "h4", 1, 2000);
                //var classFromEnteredNumber = driver.FindElements(By.TagName("h4"));
                //classFromEnteredNumber[1].Click();
                //Thread.Sleep(2000);
            }
            else
            {
                var searchString = driver.FindElement(By.Name("phone-number"));
                searchString.SendKeys(phoneNumber);
                Thread.Sleep(1000);

                PressElemByTagName(driver, "span", 3, 2000);
                //var spansFromEnteredNumber = driver.FindElements(By.TagName("span"));
                //spansFromEnteredNumber[3].Click();
                //Thread.Sleep(2000);
            }

            var results = driver.FindElements(By.TagName("h5"));
            var badList = new List<string>() { "ПСБ", "Система быстрых платежей", "Другой банк" };
            var correctList = new List<string>();

            foreach(var result in results)
                if (!badList.Contains(result.Text))
                    correctList.Add(result.Text);

            return correctList;
        }

        public static void PressElemByTagName(ChromeDriver driver, string tagName, int elemntPosition, int sleep)
        {
            var spans = driver.FindElements(By.TagName(tagName));
            spans[elemntPosition].Click();
            Thread.Sleep(sleep);
        }
    }
}
