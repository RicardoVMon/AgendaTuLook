using AgendaTuLookWeb.Models;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace AgendaTuLookWeb.Controllers
{
    public class EstadisticasController : Controller
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly IConfiguration _configuration;


        public EstadisticasController(IHttpClientFactory httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task<IActionResult> Estadisticas(DateTime fechaInicio, DateTime fechaFinal)
        {
            if (fechaInicio > fechaFinal) {
                TempData["Mensaje"] = "coloque una fecha valida";
                var estadisticaVacia = new EstadisticasModel();
                estadisticaVacia.IngresosTotales = 0;
                return View(estadisticaVacia);
            }
            var parsedFechaInicio = fechaInicio.ToString("yyyy-MM-dd");
            var parsedFechaFinal = fechaFinal.ToString("yyyy-MM-dd");


            using (var http = _httpClient.CreateClient())
            {
                    
                    http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
                    var url = $"{_configuration.GetSection("Variables:urlWebApi").Value}Estadisticas/ConsultarEstadisticas?fechaInicial={parsedFechaInicio}&fechaFinal={parsedFechaFinal}";
                    var response = await http.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;

                        if (result != null && result.Indicador)
                        {
                            var Estadisticas = JsonSerializer.Deserialize<EstadisticasModel>((JsonElement)result.Datos!)!;
                            ViewBag.FechaInicio = parsedFechaInicio;
                            ViewBag.FechaFinal = parsedFechaFinal;
                            return View(Estadisticas);
                        }
                    }



                var estadisticaVacia = new EstadisticasModel();
                estadisticaVacia.IngresosTotales = 0;
                TempData["Mensaje"] = "coloque una fecha valida";
                return View(estadisticaVacia);
            }
        }
    }
}
