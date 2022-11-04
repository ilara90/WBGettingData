using System.Net;
using System.Text.Json;
using System.Web;
using ExcelLibrary.SpreadSheet;
using Models = WBGettingData.Models.WBSearchModel;

string path = "/WBGettingData/WBGettingData/WBGettingData/Keys.txt";
List<string> keys = new List<string>(); 

using (StreamReader reader = new StreamReader(path))
{
    string? line;
    while ((line = await reader.ReadLineAsync()) != null)
    {
        keys.Add(line);
    }
}

Workbook workbook = new Workbook();

foreach (var key in keys)
{
    Worksheet worksheet = new Worksheet(key);
    worksheet.Cells[0, 0] = new Cell("Title");
    worksheet.Cells[0, 1] = new Cell("Brand");
    worksheet.Cells[0, 2] = new Cell("Id");
    worksheet.Cells[0, 3] = new Cell("Feddbacks");
    worksheet.Cells[0, 4] = new Cell("Price");

    HttpClient client = new HttpClient();
    var urlDecode = HttpUtility.UrlDecode(
        $"https://search.wb.ru/exactmatch/ru/common/v4/search?dest=-1029256,-102269,-2162196,-1257786&query={key}&resultset=catalog&sort=popular");
    var response = await client.GetAsync(urlDecode);

    if (response.StatusCode == HttpStatusCode.OK)
    {
        var result = await response.Content.ReadAsStringAsync();
        var deserializedResult = JsonSerializer.Deserialize<Models>(result);

        if (deserializedResult?.Data?.Products != null)
        {
            foreach (var product in deserializedResult.Data.Products)
            {
                var index = Array.IndexOf(deserializedResult.Data.Products, product) + 1;
                worksheet.Cells[index, 0] = new Cell(product.Name);
                worksheet.Cells[index, 1] = new Cell(product.Brand);
                worksheet.Cells[index, 2] = new Cell(product.Id);
                worksheet.Cells[index, 3] = new Cell(product.Feddbacks);
                worksheet.Cells[index, 4] = new Cell(product.Price / 100);
            }
        }

        workbook.Worksheets.Add(worksheet);
    }
    else
    {
        Console.WriteLine(response.StatusCode);
    }
}

try
{
    workbook.Save("D:\\example_workbook.xls");
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

Console.ReadLine();
