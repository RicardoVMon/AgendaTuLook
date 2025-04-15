using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AgendaTuLookWeb.Models;
using System.Text.Json;
using System.Net.Http.Headers;

namespace AgendaTuLookWeb.Controllers
{
    [FiltroSesion]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HomeController(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _httpClient = clientFactory.CreateClient();
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            using (var http = _httpClient)
            {
                http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));

                var url = _configuration.GetSection("Variables:urlWebApi").Value + "Estadisticas/Reviews";

                var response = await http.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var resultado = await response.Content.ReadFromJsonAsync<RespuestaModel>();
                    if (resultado != null && resultado.Indicador)
                    {
                        var reviews = JsonSerializer.Deserialize<List<ReviewDestacadoModel>>(
                            (JsonElement)resultado.Datos!)!;
                        return View(reviews);
                    }
                }
            }

            return View(new List<ReviewDestacadoModel>());
        }
    }
}