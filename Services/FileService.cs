using Bt = Bytescout.Spreadsheet;
using Spire.Xls;

namespace Parser.Services
{
    public static class FileService
    {
        private const string psbBaseUrl = "https://ib.psbank.ru/lk/products/summary";
        private const string tinBaseUrl = "https://www.tinkoff.ru/mybank/";
        private const string tgBaseUrl = "https://web.telegram.org/k/#@Userbox_searching_bot"; //Smart_SearchBot

        public static void RunParser()
        {
            var siteId = Settings.Settings.GetInfoTypeFromUser("ПСБ", "Тиня", "Телега");
            var filesWithNumbers = Settings.Settings.GetInfoTypeFromUser("excel.xlsx", ".txt - файл с Генчика", "Выбирай 1 или 2") == "2" ?
                GetDataFromTxtFile(): GetDataFromExcelFile();
            var driverAfterLogin = siteId == "1" ? SiteParser.LoginToSite(psbBaseUrl) : siteId == "2" ? SiteParser.LoginToSite(tinBaseUrl) : SiteParser.LoginToSite(tgBaseUrl);
            //var driverAfterLogin = SiteParser.LoginToSite(tgBaseUrl);

            var shouldReturn = false;

            for (int k = 0; k < filesWithNumbers.Count; k++)
            {
                var workbook = new Workbook();
                workbook.Worksheets.Clear();
                var ws = workbook.Worksheets.Add("WriteToCell");

                for (int i = 0; i <= 5; i++)
                //for (int i = 0; i <= filesWithNumbers[k].Count - 1; i++)
                {
                    try
                    {
                        var index = i + 1;
                        var secretNames = siteId == "1" ? SiteParser.GetSecretNamesPsb(driverAfterLogin, filesWithNumbers[k][i], shouldReturn) :
                             siteId == "2" ? SiteParser.GetSecretNamesTin(driverAfterLogin, filesWithNumbers[k][i], shouldReturn) : 
                             SiteParser.GetSecretNamesTg(driverAfterLogin, filesWithNumbers[k][i], shouldReturn);
                        ws.Range[index, 1].Value = filesWithNumbers[k][i];
                        ws.Range[index, 2].Value = String.Join(",", secretNames.ToArray());
                        shouldReturn = true;
                        Console.WriteLine(index.ToString() + ". " + filesWithNumbers[k][i] + " - " + String.Join(", ", secretNames.ToArray()));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        SaveFile(ws);
                        driverAfterLogin.Quit();
                    }
                }

                SaveFile(ws);
            }

            driverAfterLogin.Quit();
        }

        private static List<List<string>> GetDataFromTxtFile()
        {
            var listsWithPhones = new List<List<string>>();

            string directloc = @"D:\Kudrin\Projects\numgen\results";
            var files = Directory.EnumerateFiles(directloc, "*", SearchOption.AllDirectories).ToList();
            foreach (var file in files)
                listsWithPhones.Add(File.ReadAllText(file).Split(System.Environment.NewLine).ToList());

            return listsWithPhones;
        }

        private static List<List<string>> GetDataFromExcelFile()
        {
            var listsWithPhones = new List<List<string>>();

            string directloc = @"D:\test";
            var files = Directory.EnumerateFiles(directloc, "*", SearchOption.AllDirectories).ToList<string>();
            foreach (var file in files)
            {
                var document = new Bt.Spreadsheet();
                document.LoadFromFile(file);
                var worksheet = document.Workbook.Worksheets.ByName("Лист1");
                var listWithNumebrsOnOneFile = new List<string>();

                for (int i = 0; i <= worksheet.NotEmptyRowMax - 1; i++)
                    listWithNumebrsOnOneFile.Add(worksheet.Cell(i, 0).ToString());

                listsWithPhones.Add(listWithNumebrsOnOneFile);
                document.Close();
            }

            return listsWithPhones;

        }

        private static void SaveFile(Worksheet ws)
        {
            DateTime d = DateTime.Now;
            ws.AllocatedRange.AutoFitColumns();
            ws.SaveToFile("copy" + d.ToString().Replace(" ", "").Replace(".", "").Replace(":", "") + ".xls", " ");
        }
    }
}
