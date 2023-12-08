using OpenQA.Selenium.Chrome;

namespace Parser.Settings
{
    public static class Settings
    {
        public static ChromeOptions GetChromeOptions()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument(@"--user-data-dir=C:\Users\ievge\AppData\Local\Google\Chrome\User Data\");
            options.AddArgument("--profile-directory=Default");
            options.AddArgument("--start-maximized");

            return options;
        }
    }
}
