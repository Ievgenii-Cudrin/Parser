using Bt = Bytescout.Spreadsheet;
using Spire.Xls;

namespace Parser.Services
{
    public static class ExcelService
    {
        public static void RunExcel()
        {            
            string directloc = @"D:\test";
            var files = Directory.EnumerateFiles(directloc, "*", SearchOption.AllDirectories).ToList<string>();
            var driverAfterLogin = SiteParser.LoginToSite();
            var shouldReturn = false;

            for (int k = 0; k < files.Count; k++)
            {
                var document = new Bt.Spreadsheet();
                document.LoadFromFile(files[k]);
                var worksheet = document.Workbook.Worksheets.ByName("Лист1");
                var workbook = new Workbook();
                workbook.Worksheets.Clear();
                var ws = workbook.Worksheets.Add("WriteToCell");

                for (int i = 165; i <= 180; i++)
                {
                    for (int j = 0; j < 1; j++)
                    {
                        var phoneNumber = worksheet.Cell(i, j).ToString();
                        try
                        {
                            var secretNames = SiteParser.GetSecretNames(driverAfterLogin, phoneNumber, shouldReturn);
                            ws.Range[i, 1].Value = phoneNumber;
                            ws.Range[i, 2].Value = String.Join(", ", secretNames.ToArray());
                            shouldReturn = true;
                            Console.WriteLine(i + ". " + phoneNumber + " - " + secretNames.ToArray());
                        }
                        catch(Exception ex)
                        {
                            SaveFile(ws, document);
                            driverAfterLogin.Quit();
                        }
                    }
                }

                SaveFile(ws, document);
            }

            driverAfterLogin.Quit();
        }

        private static void SaveFile(Worksheet ws, Bt.Spreadsheet document)
        {
            DateTime d = DateTime.Now;
            document.Close();
            ws.AllocatedRange.AutoFitColumns();
            ws.SaveToFile("copy" + d.ToString().Replace(" ", "").Replace(".", "").Replace(":", "") + ".xls", " ");

        }
    }
}
