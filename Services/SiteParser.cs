using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Parser.Services
{
    public static class SiteParser
    {
        public static ChromeDriver LoginToSite(string baseUrl)
        {
            ChromeOptions options = Settings.Settings.GetChromeOptions();
            IWebDriver driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl(baseUrl);
            Thread.Sleep(3000);

            return (ChromeDriver)driver;
        }

        public static List<string> GetSecretNamesPsb(ChromeDriver driver, string phoneNumber, bool shouldReturn)
        {
            phoneNumber = phoneNumber.Split(" ")[0].Split(".")[1];
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
        public static List<string> GetSecretNamesTin(ChromeDriver driver, string phoneNumber, bool shouldReturn)
        {
            if (shouldReturn)
                PressElemByTagName(driver, "a", 17, 1500);

            // press Send By phone btn
            PressElemByTagName(driver, "a", 25, 1500);

            phoneNumber = phoneNumber[0].ToString() == "+" ? phoneNumber.Remove(0, 2) : phoneNumber.Remove(0, 1); 
            // put phone number
            AddDataToInputByTagName(driver, "input", 0, 1500, phoneNumber);
            //AddDataToInputByTagName(driver, "input", 0, 2000, "9143735917");

            // press on element to load results
            PressElemByTagName(driver, "h2", 2, 2000);

            var results = driver.FindElements(By.TagName("button"));
            var correctList = new List<string>();

            for (int i = 3; i < results.Count - 3; i++)
            {
                var info = results[i].Text.Split(Environment.NewLine);
                if (correctList.Count == 0)
                    correctList.Add(info[1]);

                correctList.Add(info[0]);
            }

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
            var searchString = driver.FindElement(By.Name(tagName));
            searchString.SendKeys(phoneNumber);
            Thread.Sleep(sleep);
        }

        private static void AddDataToInputByTagName(ChromeDriver driver, string tagName, int elemntPosition, int sleep, string phoneNumber)
        {
            var inputs = driver.FindElements(By.TagName(tagName));
            inputs[elemntPosition].SendKeys(phoneNumber);
            Thread.Sleep(sleep);
        }
    }
}
