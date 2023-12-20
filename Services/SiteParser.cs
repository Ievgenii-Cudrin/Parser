using HtmlAgilityPack;
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
            Thread.Sleep(10000);

            return (ChromeDriver)driver;
        }

        public static List<string> GetSecretNamesPsb(ChromeDriver driver, string phoneNumber, bool shouldReturn)
        {
            phoneNumber = phoneNumber.Split(" ")[0];
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
                PressElemByTagName(driver, "a", 17, 2500);

            // press Send By phone btn
            PressElemByTagName(driver, "a", 25, 2500);

            phoneNumber = phoneNumber[0].ToString() == "+" ? phoneNumber.Remove(0, 2) : phoneNumber.Remove(0, 1); 
            // put phone number
            AddDataToInputByTagName(driver, "input", 0, 2500, phoneNumber);

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


        public static List<string> GetSecretNamesTg(ChromeDriver driver, string phoneNumber, bool shouldReturn)
        {
            var spans = driver.FindElements(By.ClassName("input-message-input"));
            spans[0].Click();
            spans[0].Clear();
            spans[0].SendKeys(phoneNumber);
            spans[0].SendKeys(Keys.Enter);
            Thread.Sleep(5000);
            var info = driver.FindElements(By.ClassName("spoilers-container"));
            var details = info[info.Count - 1].Text.Replace(Environment.NewLine, ",");
            var listWithIfo = new List<string>();

            if (details.Contains("результатов по запросу"))
            {
                var htmlWithInfo = driver.FindElements(By.ClassName("document-container"));
                htmlWithInfo[htmlWithInfo.Count - 1].Click();
                string directloc = @"C:\Users\ievge\Downloads";
                var files = Directory.EnumerateFiles(directloc, "*", SearchOption.AllDirectories).ToList();
                var htmlDoc = new HtmlDocument();
                details = File.ReadAllText(files[files.Count - 1]);
                htmlDoc.LoadHtml(details);

                var nodes = htmlDoc.DocumentNode.SelectNodes("//div");

                foreach (var node in nodes)
                    listWithIfo.Add(node.InnerText.Replace(" ", "").Replace("\n", ""));

                details = String.Join(", ", listWithIfo.ToArray());

            }

            if (details.Length > 5000)
                details = details.Substring(0, 5000);


            return new List<string>() { details.Replace("├", "") };
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
