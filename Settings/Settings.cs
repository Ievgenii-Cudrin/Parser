using OpenQA.Selenium.Chrome;

namespace Parser.Settings
{
    public static class Settings
    {
        public static ChromeOptions GetChromeOptions()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument(@"--user-data-dir=C:\Users\user\AppData\Local\Google\Chrome\User Data\");
            options.AddArgument("--profile-directory=Default");
            options.AddArgument("--start-maximized");

            return options;
        }

        public static string GetInfoTypeFromUser(string firtsType, string secondType, string thirdType)
        {
            var userChoice = "";
            var isUserChoiseIsCorrect = false;

            do
            {
                Console.WriteLine($"Данные с какого файла читаем? - {firtsType}, {secondType}, {thirdType}:");
                Console.WriteLine($"1. {firtsType}");
                Console.WriteLine($"2. {secondType}");
                Console.WriteLine($"3. {thirdType}");
                Console.WriteLine("Введите 1 или 2 или 3 и нажмите Enter: ");
                userChoice = Console.ReadLine();

                isUserChoiseIsCorrect = userChoice == "1" || userChoice == "2" || userChoice == "3";
                if (!isUserChoiseIsCorrect)
                    Console.WriteLine("\nВведено неверное значение, повторите Ваш выбор.\n");
            } while (!isUserChoiseIsCorrect);

            return userChoice;
        }
    }
}
