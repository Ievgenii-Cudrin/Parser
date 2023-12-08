using Bytescout.Spreadsheet;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Parser.Services;
using Parser.Settings;

class Program
{
    static void Main()
    {
        ExcelService.RunExcel();

        Console.WriteLine();
    }
}
