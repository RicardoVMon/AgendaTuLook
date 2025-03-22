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
        /*
         * fecha: 22/3/2025
         * Accion: muestra los horarios segun el estado
        */
        public IActionResult Index(Boolean Estado)
        {
            using (var http = _httpClient.CreateClient())
            {
                //url de la api para mostrar los horarios segun el estado
                var url = _configuration.GetSection("Variables:urlWebApi").Value + "Horarios/MostrarHorarios?Estado=" + Estado;
                var response = http.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;
                    if(result != null && result.Indicador)
                    {
                        //se transforma el json del api a una lista de horarios model
                        var datosHorarios = JsonSerializer.Deserialize<List<HorariosModel>>((JsonElement)result.Datos!);
                        //se muestra en la vista
                        return View(datosHorarios); 
                    }
                   
                }
                return View();

            }
        }

        /*
         * fecha: 22/3/2025
         * Accion: muestra un horario dependiendo del id
        */
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
        /*
         * fecha: 22/3/2025
         * Accion: muestra la vista con el formulario para crear el horario
        */

        public IActionResult CrearHorario()
        {
            return View();
        }

        /*
         * fecha: 22/3/2025
         * Accion: registra en la base de datos el horario creado en el metodo anterior llamando al api
        */
        public IActionResult RegistrarHorario(HorariosModel model)
        {
            using (var http = _httpClient.CreateClient())
            {
                var url = _configuration.GetSection("Variables:urlWebApi").Value + "Horarios/RegistrarHorario";
                var response = http.PostAsJsonAsync(url, model).Result;

                return RedirectToAction("Index", new { Estado = true });

            }
        }

        /*
         * fecha: 22/3/2025
         * Accion: Edita en base de datos el horario pasado por parametro
        */
        public IActionResult EditarHorario(HorariosModel model)
        {
            using (var http = _httpClient.CreateClient())
            {
                var url = _configuration.GetSection("Variables:urlWebApi").Value + "Horarios/ActualizarHorario";
                var response = http.PutAsJsonAsync(url, model).Result;

                return RedirectToAction("Index", new { Estado = true });

            }
        }
        /*
         * fecha: 22/3/2025
         * Accion: no elimina literalmente un horario solo cambia el estado del horario 
        */

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
