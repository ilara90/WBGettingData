
using System.Text.Json;
using System.Web;
using Microsoft.Office.Interop.Excel;
using _excel = Microsoft.Office.Interop.Excel;
using Model = WBGettingData.Model;

string[] keys = new[] { "Игрушки", "Настолки" };

_Application excel = new _excel.Application();
Workbook workbook = excel.Workbooks.Add();

foreach (var key in keys)
{
    HttpClient client = new HttpClient();
    var urlDecode = HttpUtility.UrlDecode(
        $"https://search.wb.ru/exactmatch/ru/common/v4/search?dest=-1029256,-102269,-2162196,-1257786&query={key}&resultset=catalog&sort=popular");
    var response = await client.GetAsync(urlDecode);
    var result = await response.Content.ReadAsStringAsync();
    var deserializedResult = JsonSerializer.Deserialize<Model>(result);

    var worksheet = (Worksheet)workbook.Worksheets[Array.IndexOf(keys, key) + 1];
    worksheet.Name = key;
    worksheet.Cells[1, "A"] = "Title";
    foreach (var product in deserializedResult.Data.Products)
    {
        var index = Array.IndexOf(deserializedResult.Data.Products, product) + 2;
        worksheet.Cells[index, "A"] = product.Name;
    }
    
}

workbook.SaveAs("C:\\example_workbook.xlsx");