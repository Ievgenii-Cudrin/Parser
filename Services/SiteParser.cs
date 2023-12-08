using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

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
                // go back after phone number checking
                PressElemByTagName(driver, "span", 3, 2000);

                // go to phone number entering
                PressElemByTagName(driver, "rui-icon", 5, 1000);
            } 
            else
            {
                // click on find by phone button
                PressElemByTagName(driver, "button", 4, 2000);
            }

            if(!shouldReturn)
            {
                //Add phone number to input
                AddDataToInputByName(driver, phoneNumber, "searchString", 3000);

                // click on phone number to search
                PressElemByTagName(driver, "h4", 1, 2000);
            }
            else
            {
                AddDataToInputByName(driver, phoneNumber, "phone-number", 1000);
                PressElemByTagName(driver, "span", 3, 2000);
            }

            var results = driver.FindElements(By.TagName("h5"));
            var invalidList = new List<string>() { "ПСБ", "Система быстрых платежей", "Другой банк" };
            var correctList = new List<string>();

            foreach(var result in results)
                if (!invalidList.Contains(result.Text))
                    correctList.Add(result.Text);

            return correctList;
        }

        private static void PressElemByTagName(ChromeDriver driver, string tagName, int elemntPosition, int sleep)
        {
            var spans = driver.FindElements(By.TagName(tagName));
            spans[elemntPosition].Click();
            Thread.Sleep(sleep);
        }

        private static void AddDataToInputByName(ChromeDriver driver, string phoneNumber, string tagName, int sleep)
        {
            var searchString = driver.FindElement(By.Name("searchString"));
            searchString.SendKeys(phoneNumber);
            Thread.Sleep(sleep);
        }
    }
}
