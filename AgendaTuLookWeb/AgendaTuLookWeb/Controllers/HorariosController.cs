using System.Text.Json;
using AgendaTuLookWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace AgendaTuLookWeb.Controllers
{
    public class HorariosController : Controller
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly IConfiguration _configuration;

        public HorariosController(IHttpClientFactory httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public IActionResult Index(Boolean Estado)
        {
            Console.WriteLine("Este es el estado " + Estado);
            using (var http = _httpClient.CreateClient())
            {
                var url = _configuration.GetSection("Variables:urlWebApi").Value + "Horarios/MostrarHorarios?Estado=" + Estado;
                var response = http.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;
                    if(result != null && result.Indicador)
                    {
                        Console.WriteLine(result.Datos);
                        var datosHorarios = JsonSerializer.Deserialize<List<HorariosModel>>((JsonElement)result.Datos!);
                        
                        return View(datosHorarios); 
                    }
                   
                }
                return View();

            }
        }


        public IActionResult VerHorario(long id)
        {
            using (var http = _httpClient.CreateClient())
            {
                var url = _configuration.GetSection("Variables:urlWebApi").Value + "Horarios/MostrarHorario?HorariosId=" + id;
                var response = http.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;
                    if (result != null && result.Indicador)
                    {
                        Console.WriteLine(result.Datos);
                        var datosHorarios = JsonSerializer.Deserialize<HorariosModel>((JsonElement)result.Datos!);
                        return View(datosHorarios);
                    }

                }
                return View();

            }
        }


        public IActionResult EditarHorario(HorariosModel model)
        {
            using (var http = _httpClient.CreateClient())
            {
                var url = _configuration.GetSection("Variables:urlWebApi").Value + "Horarios/ActualizarHorario";
                var response = http.PutAsJsonAsync(url, model).Result;

                return RedirectToAction("Index", new { Estado = true });

            }
        }

        public IActionResult EliminarHorario(long id)
        {
            using (var http = _httpClient.CreateClient())
            {
                var url = _configuration.GetSection("Variables:urlWebApi").Value + "Horarios/EliminarHorario?HorariosId=" + id;
                var response = http.DeleteAsync(url).Result;
                
                return RedirectToAction("Index", new {Estado = true});

            }
        }
    }
}
