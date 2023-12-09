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

        public static string GetFileTypeFromUser()
        {
            var userChoice = "";
            var isUserChoiseIsCorrect = false;

            do
            {
                Console.WriteLine("Данные с какого файла читаем? - excel или txt(файл с Генчика):");
                Console.WriteLine("1. .xlsx");
                Console.WriteLine("2. .txt - файл с Генчика");
                Console.WriteLine("Введите 1 или 2 и нажмите Enter: ");
                userChoice = Console.ReadLine();

                isUserChoiseIsCorrect = userChoice == "1" || userChoice == "2";
                if (!isUserChoiseIsCorrect)
                    Console.WriteLine("\nВведено неверное значение, повторите Ваш выбор.\n");
            } while (!isUserChoiseIsCorrect);

            return userChoice;
        }
    }
}
