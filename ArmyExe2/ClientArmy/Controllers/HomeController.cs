using ArmyExe2.Models;
using ClientArmy.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ArmyExe2.Controllers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Diagnostics.Metrics;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;

namespace ClientArmy.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        string BaseUrl = "http://localhost:39605";
        public async Task<IActionResult> Index()
        {
            DataTable dt = new DataTable();
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync("http://localhost:39605/api/CoronaDetail/GetAllCoronaDetails");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var coronaDetails = System.Text.Json.JsonSerializer.Deserialize<Response>(responseContent, options);
                    return View(coronaDetails);
                }
            }

            return View();
        }
        public async Task<IActionResult> GetAllCoronaDetailsHC()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync("http://localhost:39605/api/CoronaDetail/GetAllCoronaDetails");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var coronaDetails = System.Text.Json.JsonSerializer.Deserialize<Response>(responseContent, options);
                    return View(coronaDetails);
                }
            }

            return View();
        }
        public async Task<IActionResult> GetAllUsersHC()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync("http://localhost:39605/api/Username/GetAllUsers");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var username = System.Text.Json.JsonSerializer.Deserialize<Response>(responseContent, options);
                    return View(username);
                }
            }
            return View();
        }
        public async Task<IActionResult> GetSickByDays()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync("http://localhost:39605/api/Username/GetSickByDays");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var days = JsonSerializer.Deserialize<Response>(responseContent, options);
                    return View(days);
                }
            }
            return View();
        }
        public async Task<IActionResult> GetNotVaccinatedCount()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync("http://localhost:39605/api/CoronaDetail/GetNotVaccinatedCount");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var NotVaccinatedCount = JsonSerializer.Deserialize<Response>(responseContent, options);
                    return View(NotVaccinatedCount);
                }
            }
            return View();
        }
        private MediaTypeWithQualityHeaderValue MediaTypeWithQualityHeaderValue(string v)
        {
            throw new NotImplementedException();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}