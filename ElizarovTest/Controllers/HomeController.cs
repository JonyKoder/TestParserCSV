using ElizarovTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;
using System.Diagnostics;

namespace ElizarovTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        IWebHostEnvironment _appEnvironment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment appEnvironment)
        {
            _logger = logger;
            _appEnvironment = appEnvironment;
        }

        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile uploadedFile, string znak)
        {
            FileModel fileModel = new FileModel();
            
            //ПРОПИСАЛ СОХРАНЕНИЕ НА СЕРВЕРЕ + 2 ВИДА ЧТЕНИЯ ИЗ ФАЙЛА...
            //МОГ БЫ СДЕЛТЬ ЕЩЕ С БАЗОЙ ДАННЫХ
                // путь к папке Files
                string path = "/Files/" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                FileModel file = new FileModel { Name = uploadedFile.FileName, Path = path };
                TextFieldParser tfp = new TextFieldParser("wwwroot/Files/" + uploadedFile.FileName);
                
                    string res = "";
                    tfp.TextFieldType = FieldType.Delimited;
                    if (znak.Contains("Точка")) res = ".";
                    if (znak.Contains("Зап")) res = ",";
                    tfp.SetDelimiters(res);

            string[] fields = tfp.ReadFields();
            string whole_file = System.IO.File.ReadAllText("wwwroot/Files/" + uploadedFile.FileName);
            whole_file = whole_file.Replace('\n', '\r');
            string[] lines = whole_file.Split(new char[] { '\r' },
            StringSplitOptions.RemoveEmptyEntries);
            int num_rows = lines.Length;
            int num_cols = lines[0].Split(res).Length;

            ViewBag.Test = fields;
            ViewBag.Rows = num_rows;
            ViewBag.Cols = num_cols;


            return View();


        }

        public IActionResult Index()
        {
          
            IEnumerable<FileModel> fileModels = new List<FileModel>();
           
            



            return View(fileModels);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}