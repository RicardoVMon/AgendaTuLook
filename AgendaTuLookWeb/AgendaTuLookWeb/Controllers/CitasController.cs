using AgendaTuLookWeb.Models;
using AgendaTuLookWeb.Servicios;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net.Http.Headers;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace AgendaTuLookWeb.Controllers
{
	[FiltroSesion]
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public class CitasController : Controller
    {
		private readonly IHttpClientFactory _httpClient;
		private readonly IConfiguration _configuration;
		private readonly IPictures _pictures;
		public CitasController(IHttpClientFactory httpClient, IConfiguration configuration, IPictures pictures) { 
			_httpClient = httpClient;
			_configuration = configuration;
			_pictures = pictures;
		}

		[HttpGet]
		public async Task<IActionResult> HistorialCitas()
		{

			using (var http = _httpClient.CreateClient())
			{
				long usuarioId = Convert.ToInt64(HttpContext.Session.GetString("UsuarioId")!);
				http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
				var url = _configuration.GetSection("Variables:urlWebApi").Value + "Calendario/ConsultarCitasCalendario?Id=" + usuarioId + "&c=" + 1;

				var response = await http.GetAsync(url);

				if (response.IsSuccessStatusCode)
				{
					var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;

					if (result != null && result.Indicador)
					{
						var datosConfirmar = JsonSerializer.Deserialize<List<CitasModel>>((JsonElement)result.Datos!)!;
						return View(datosConfirmar);
					}
				}
				return View();
			}
		}

		public IActionResult CuponesCitas()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> SolicitarServiciosCita()
		{
			using (var http = _httpClient.CreateClient())
			{
				http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
				var url = _configuration.GetSection("Variables:urlWebApi").Value + "Servicios/GestionarServicios";
				var response = await http.GetAsync(url);
				if (response.IsSuccessStatusCode)
				{
					var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;
					if (result != null && result.Indicador)
					{
						var servicios = JsonSerializer.Deserialize<List<ServicioModel>>((JsonElement)result.Datos!)!;
						return View(servicios);
					}
				}
			}
				return View();
		}

		[HttpGet]
		public IActionResult SolicitarFechaHora()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> ObtenerDiasLaborables()
		{
			using (var http = _httpClient.CreateClient())
			{
				http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
				var url = _configuration.GetSection("Variables:urlWebApi").Value + "DiasTrabajo/GestionarDiasTrabajo";
				var response = await http.GetAsync(url);

				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadFromJsonAsync<RespuestaModel>();
					if (result != null && result.Indicador)
					{
						var diasTrabajo = JsonSerializer.Deserialize<List<DiasTrabajoModel>>((JsonElement)result.Datos!)!;

						var fullCalendarDays = diasTrabajo
						.Where(d => d.Activo == true)
						.GroupBy(d => new { d.HoraInicio, d.HoraFin }) // Agrupa por horario
						.Select(grupo => new
						{
							daysOfWeek = grupo.Select(d => MapDayNameToIndex(d.NombreDia)).ToList(), // Lista de días
							startTime = grupo.Key.HoraInicio.ToString(@"hh\:mm"),
							endTime = grupo.Key.HoraFin.ToString(@"hh\:mm")
						})
						.ToList();

						return Json(fullCalendarDays);
					}
				}
				return Json(new { success = false, message = "No se encontraron días laborables" });
			}
		}

		[HttpPost]
		public async Task<IActionResult> ConsultarHorasDisponibles(DateTime fecha)
		{
			using (var http = _httpClient.CreateClient())
			{

				CitasModel model = new CitasModel
				{
					Fecha = fecha
				};

				http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
				var url = _configuration.GetSection("Variables:urlWebApi").Value + "Citas/ConsultarHorasDisponibles";
				var response = await http.PostAsJsonAsync(url, model);

				if (response.IsSuccessStatusCode)
				{
					var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;

					if (result != null && result.Indicador)
					{
						var horasDisponibles = JsonSerializer.Deserialize<List<HoraModel>>((JsonElement)result.Datos!)!;
						return Json(horasDisponibles);
					}
				}
				return Json(new { success = false, message = "No fue posible obtener las horas disponibles del día" });
			}
		}

		[HttpGet]
		public async Task<IActionResult> ConfirmarCita(long s, DateTime f, TimeSpan h)
		{

			using (var http = _httpClient.CreateClient())
			{
				long usuarioId = Convert.ToInt64(HttpContext.Session.GetString("UsuarioId")!);
				
				CitasModel model = new CitasModel
				{
					Usuario = new UsuarioModel
					{
						UsuarioId = usuarioId
					},
					Servicio = new ServicioModel
					{
						ServicioId = s
					},
					Fecha = f,
					HoraInicio = h,
				};

				http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
				var url = _configuration.GetSection("Variables:urlWebApi").Value + "Citas/ConsultarDatosConfirmar";
				
				var response = await http.PostAsJsonAsync(url, model);

				if (response.IsSuccessStatusCode)
				{
					var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;

					if (result != null && result.Indicador)
					{
						var datosConfirmar = JsonSerializer.Deserialize<CitasModel>((JsonElement)result.Datos!)!;
						return View(datosConfirmar);
					}
				}
				return View();
			}
		}

		[HttpPost]
		public async Task<IActionResult> ConfirmarCita(CitasModel model, IFormFile Archivos)
		{
			using (var http = _httpClient.CreateClient())
			{

				if (Archivos != null)
				{
					string rutaImagen = await _pictures.GuardarImagen(Archivos, "Comprobantes");
					model.MetodoPago!.Comprobante = rutaImagen;
				}

				http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
				var url = _configuration.GetSection("Variables:urlWebApi").Value + "Citas/ConfirmarCita";

				var response = await http.PostAsJsonAsync(url, model);

				if (response.IsSuccessStatusCode)
				{
					var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;

					if (result != null && result.Indicador)
					{
						TempData["SuccessMessage"] = "Cita Creada con Éxito!";
						return RedirectToAction("Calendario", "Calendario");
					}
				}
				TempData["errorMessage"] = "Ocurrió un error al crear la cita, vuelva a intentarlo más tarde";
				return RedirectToAction("SolicitarServiciosCita", "Citas");
			}
		}

		[HttpGet]
		public async Task<IActionResult> GestionarCitas()
		{

			using (var http = _httpClient.CreateClient())
			{
				long usuarioId = Convert.ToInt64(HttpContext.Session.GetString("UsuarioId")!);
				http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
				var url = _configuration.GetSection("Variables:urlWebApi").Value + "Calendario/ConsultarCitasCalendario?Id=" + usuarioId + "&c=" + 2;

				var response = await http.GetAsync(url);

				if (response.IsSuccessStatusCode)
				{
					var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;

					if (result != null && result.Indicador)
					{
						var datosConfirmar = JsonSerializer.Deserialize<List<CitasModel>>((JsonElement)result.Datos!)!;
						return View(datosConfirmar);
					}
				}
				return View();
			}
		}

		[HttpPost]
		public async Task<IActionResult> GuardarCalificacion(CitasModel model)
		{
			using (var http = _httpClient.CreateClient())
			{

				http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
				var url = _configuration.GetSection("Variables:urlWebApi").Value + "Citas/GuardarCalificacion";

				var response = await http.PostAsJsonAsync(url, model);

				if (response.IsSuccessStatusCode)
				{
					var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;

					if (result != null && result.Indicador)
					{
						TempData["SuccessMessage"] = "Calificación Realizada Con Éxito";
						return RedirectToAction("HistorialCitas", "Citas");
					}
				}
				TempData["errorMessage"] = "Ocurrió un error al crear la cita, vuelva a intentarlo más tarde";
				return RedirectToAction("HistorialCitas", "Citas");
			}
		}

		private int MapDayNameToIndex(string? nombreDia)
		{
			return nombreDia?.ToLower() switch
			{
				"lunes" => 1,
				"martes" => 2,
				"miércoles" or "miercoles" => 3,
				"jueves" => 4,
				"viernes" => 5,
				"sábado" or "sabado" => 6,
				"domingo" => 0,
				_ => -1  // Valor inválido
			};
		}

        [HttpGet]
        public async Task<IActionResult> DetalleCita(long id)
        {
            using (var http = _httpClient.CreateClient())
            {
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));

                var url = _configuration.GetSection("Variables:urlWebApi").Value + $"Citas/DetalleCita?id={id}";

                var response = await http.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var resultado = await response.Content.ReadFromJsonAsync<RespuestaModel>();
                    if (resultado != null && resultado.Indicador)
                    {
                        var cita = JsonSerializer.Deserialize<CitaDetalleModel>((JsonElement)resultado.Datos!)!;
                        return View(cita);
                    }
                }
            }

            TempData["Error"] = "No se pudo cargar el detalle de la cita.";
            return RedirectToAction("Calendario", "Calendario");
        }

        [HttpPost]
        public async Task<IActionResult> CancelarCita([FromForm] long citaId)
        {
            using var http = _httpClient.CreateClient();
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));

            var url = _configuration.GetSection("Variables:urlWebApi").Value + "Citas/Cancelar";
            var response = await http.PostAsJsonAsync(url, citaId);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<RespuestaModel>();
                if (result != null)
                {
                    if (result.Indicador)
                    {
                        TempData["SuccessMessage"] = result.Mensaje ?? "Cita cancelada exitosamente.";
                        return RedirectToAction("GestionarCitas");
                    }
                    else
                    {
                        TempData["errorMessage"] = result.Mensaje ?? "Ocurrió un error al cancelar la cita.";
                        return RedirectToAction("GestionarCitas");
                    }
                }
            }

            TempData["errorMessage"] = "Ocurrió un error al comunicarse con el servidor.";
            return RedirectToAction("GestionarCitas");
        }

        //-----------------------------------------------

        [HttpGet]
        public async Task<IActionResult> EditarCitaServicio(long id)
        {
            using (var http = _httpClient.CreateClient())
            {
                // Verificar si la cita es editable
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
                var editableUrl = _configuration.GetSection("Variables:urlWebApi").Value + "Citas/VerificarCitaEditable";

                var editableResponse = await http.PostAsJsonAsync(editableUrl, new { CitaId = id });

                if (!editableResponse.IsSuccessStatusCode)
                {
                    TempData["errorMessage"] = "Se presentó un error al solicitar editar la cita.";
                    return RedirectToAction("GestionarCitas");
                }

                var editableResult = await editableResponse.Content.ReadFromJsonAsync<RespuestaModel>();
                if (editableResult == null || !editableResult.Indicador)
                {
                    TempData["errorMessage"] = "Lo sentimos. No se puede editar citas con menos de 24 horas de anticipación.";
                    return RedirectToAction("GestionarCitas");
                }

                // Obtener información de la cita
                var url = _configuration.GetSection("Variables:urlWebApi").Value + "Citas/ObtenerCitaParaEditar";
                var response = await http.PostAsJsonAsync(url, new { CitaId = id });

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<RespuestaModel>();
                    if (result != null && result.Indicador && result.Datos != null)
                    {
                        var cita = JsonSerializer.Deserialize<CitasModel>((JsonElement)result.Datos!);

                        if (cita == null || cita.Servicio == null)
                        {
                            TempData["errorMessage"] = "No se pudo cargar la información para editar la cita";
                            return RedirectToAction("GestionarCitas");
                        }

                        // Guardar el precio original
                        cita.PrecioOriginal = cita.Servicio.Precio;

                        // Obtener lista de servicios
                        var serviciosResponse = await http.GetAsync(_configuration.GetSection("Variables:urlWebApi").Value + "Servicios/GestionarServicios");
                        if (serviciosResponse.IsSuccessStatusCode)
                        {
                            var serviciosResult = await serviciosResponse.Content.ReadFromJsonAsync<RespuestaModel>();
                            if (serviciosResult != null && serviciosResult.Indicador && serviciosResult.Datos != null)
                            {
                                var servicios = JsonSerializer.Deserialize<List<ServicioModel>>((JsonElement)serviciosResult.Datos!);
                                ViewBag.Servicios = servicios ?? new List<ServicioModel>();
                                return View(cita);
                            }
                        }
                    }
                }

                TempData["errorMessage"] = "No se pudo cargar la información para editar la cita";
                return RedirectToAction("GestionarCitas");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditarFechaHora(long id, long sn, long sa)
        {
            using (var http = _httpClient.CreateClient())
            {
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));

                // Obtener información de la cita
                var citaResponse = await http.PostAsJsonAsync(
                    _configuration.GetSection("Variables:urlWebApi").Value + "Citas/ObtenerCitaParaEditar",
                    new { CitaId = id });

                if (citaResponse.IsSuccessStatusCode)
                {
                    var result = await citaResponse.Content.ReadFromJsonAsync<RespuestaModel>();
                    if (result != null && result.Indicador)
                    {
                        var cita = JsonSerializer.Deserialize<CitasModel>((JsonElement)result.Datos!);

                        // Obtener el servicio seleccionado
                        var servicioResponse = await http.GetAsync(
                            _configuration.GetSection("Variables:urlWebApi").Value + $"Servicios/EditarServicio?id={sn}");

                        if (servicioResponse.IsSuccessStatusCode)
                        {
                            var servicioResult = await servicioResponse.Content.ReadFromJsonAsync<RespuestaModel>();
                            if (servicioResult != null && servicioResult.Indicador)
                            {
                                var servicioActual = JsonSerializer.Deserialize<ServicioModel>((JsonElement)servicioResult.Datos!);

                                // Guardar el precio original del servicio antes de cualquier cambio
                                if (cita.PrecioOriginal == 0)
                                {
                                    cita.PrecioOriginal = cita.Servicio.Precio;
                                }

                                // Actualizar el servicio en la cita
                                cita.Servicio = servicioActual;
                            }
                        }

                        return View(cita);
                    }
                }
                TempData["errorMessage"] = "No se pudo cargar la información para editar la cita";
                return RedirectToAction("GestionarCitas");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmarEdicionCita(long id, long sa, long sn, DateTime f, TimeSpan h)
        {
			using (var http = _httpClient.CreateClient())
			{
				long usuarioId = Convert.ToInt64(HttpContext.Session.GetString("UsuarioId")!);

				CitasModel model = new CitasModel
				{
					CitaId = id,
					Usuario = new UsuarioModel
					{
						UsuarioId = usuarioId
					},
					Servicio = new ServicioModel
					{
						ServicioId = sn,
						CambioServicio = sa != sn
					},
					Fecha = f,
					HoraInicio = h
				};

				http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
				var url = _configuration.GetSection("Variables:urlWebApi").Value + "Citas/ConsultarDatosConfirmar";

				var response = await http.PostAsJsonAsync(url, model);

				if (response.IsSuccessStatusCode)
				{
					var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;

					if (result != null && result.Indicador)
					{
						var datosConfirmar = JsonSerializer.Deserialize<CitasModel>((JsonElement)result.Datos!)!;
						return View(datosConfirmar);
					}
				}
				return View();
			}
		}


        [HttpPost]
        public async Task<IActionResult> ConfirmarEdicionCita(CitasModel model, IFormFile Archivos)
        {
            using (var http = _httpClient.CreateClient())
            {

				// Manejo de la imagen primero
				if (Archivos != null && Archivos.Length > 0)
				{
					// Eliminar imagen anterior si existe
					if (!string.IsNullOrEmpty(model.MetodoPago.Comprobante)) // Esto no va a llegar porque no lo hemos traido, por si ya tiene
					{
						_pictures.EliminarImagen(model.MetodoPago.Comprobante);
					}
					model.MetodoPago.Comprobante = await _pictures.GuardarImagen(Archivos, "Comprobantes");
				}

				http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
                var url = _configuration.GetSection("Variables:urlWebApi").Value + "Citas/ActualizarCita";

                var response = await http.PostAsJsonAsync(url, model);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<RespuestaModel>();
                    if (result != null && result.Indicador)
                    {
                        TempData["SuccessMessage"] = model.Servicio.CambioServicio ?
                            "Cita actualizada con éxito. Se ha generado una nueva factura." :
                            "Cita actualizada con éxito.";
                        return RedirectToAction("GestionarCitas");
                    }
                }

                TempData["errorMessage"] = "Ocurrió un error al actualizar la cita";
                return RedirectToAction("GestionarCitas");
            }
        }


    }

}
