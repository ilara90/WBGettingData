using System.Net;
using System.Text.Json;
using System.Web;
using ExcelLibrary.SpreadSheet;
using Microsoft.Extensions.Configuration;
using WBGettingData.Models;
using Models = WBGettingData.Models.WBSearchModel;

var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false);

IConfiguration config = builder.Build();

var configurationSettings = config.GetSection("ConfigurationSettings").Get<ConfigurationSettings>();

List<string> keys = new List<string>(); 

using (StreamReader reader = new StreamReader(configurationSettings.KeysPath))
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
    var urlDecode = HttpUtility.UrlDecode(string.Format(configurationSettings.SearchWBURL, key));
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
    workbook.Save(configurationSettings.ResultPath);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

Console.ReadLine();
