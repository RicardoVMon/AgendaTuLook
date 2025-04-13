using AgendaTuLookAPI.Models;
using AgendaTuLookAPI.Servicios;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Data;

namespace AgendaTuLookAPI.Controllers
{
	[Route("api/[controller]")]
	[Authorize]
	[ApiController]
	public class CitasController : Controller
	{
		private readonly IHttpClientFactory _httpClient;
		private readonly IConfiguration _configuration;
		private readonly ICorreos _correos;
		public CitasController(IConfiguration configuration, ICorreos correos, IHttpClientFactory httpClient)
		{
			_httpClient = httpClient;
			_configuration = configuration;
			_correos = correos;
		}

		[HttpPost]
		[Route("ConsultarHorasDisponibles")]
		public IActionResult ConsultarHorasDisponibles(CitaModel model)
		{

			using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value))
			{
				var respuesta = new RespuestaModel();
				var horasDisponibles = context.Query<HoraModel>("ConsultarHorasDisponibles", new { model.Fecha });

				if (horasDisponibles.Any())
				{
					respuesta.Indicador = true;
					respuesta.Datos = horasDisponibles;
					return Ok(respuesta);
				}
				respuesta.Indicador = false;
				respuesta.Mensaje = "No se pudieron obtener los servicios";
				return Ok(respuesta);
			}

		}

		[HttpPost]
		[Route("ConsultarDatosConfirmar")]
		public IActionResult ConsultarDatosConfirmar(CitaModel model)
		{

			using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value))
			{
				var respuesta = new RespuestaModel();
				var informacionConfirmar = context.Query("ConsultarDatosConfirmar", new { model.Usuario!.UsuarioId, model.Servicio!.ServicioId, model.Fecha });

				if (informacionConfirmar.Any())
				{
					// Proceso para obtener los metodos de pago
					var metodosPago = context.Query<MetodoPagoModel>("ObtenerMetodosPago");

					// Proceso para redondear siguiente hora
					var firstInfo = informacionConfirmar.First();
					var horaInicio = TimeSpan.Parse(model.HoraInicio.ToString());
					var duracion = TimeSpan.FromMinutes(firstInfo.Duracion);
					var horaFin = RedondearHora(horaInicio + duracion);

					CitaModel confirmarCita = new CitaModel
					{
						Fecha = model.Fecha,
						HoraInicio = horaInicio,
						HoraFin = horaFin,

						Usuario = new UsuarioModel
						{
							UsuarioId = model.Usuario.UsuarioId,
							Nombre = firstInfo.Nombre,
							Correo = firstInfo.Correo,
							Telefono = firstInfo.Telefono
						},
						Servicio = new ServicioModel
						{
							ServicioId = model.Servicio.ServicioId,
							NombreServicio = firstInfo.NombreServicio,
							Precio = (double)firstInfo.Precio,
							Duracion = firstInfo.Duracion
						},
						DiaTrabajoId = firstInfo.DiaTrabajoId,
						MetodosPago = metodosPago.ToList(),
					};

					respuesta.Indicador = true;
					respuesta.Datos = confirmarCita;
					return Ok(respuesta);
				}
				respuesta.Indicador = false;
				respuesta.Mensaje = "No se pudieron obtener los datos de confirmación";
				return Ok(respuesta);
			}

		}

		[HttpPost]
		[Route("ConfirmarCita")]
		public IActionResult ConfirmarCita(CitaModel model)
		{

			using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value))
			{
				var respuesta = new RespuestaModel();
				var resultado = context.Execute("CrearCita",
					new
					{
						model.Usuario!.UsuarioId,
						model.Servicio!.ServicioId,
						model.Fecha,
						model.HoraInicio,
						model.HoraFin,
						model.DiaTrabajoId,
						model.MetodoPago!.MetodoPagoId,
						model.MetodoPago.Comprobante
					});

				// Enviar correo
				_correos.EnviarCorreoFacturaCita(model.Usuario.Correo!, model.Usuario.Nombre!, model.Servicio.NombreServicio!, model.Servicio.Precio!, model.MetodoPago.Nombre!, model.Fecha.ToString("dd/MM/yyyy"), model.HoraInicio.ToString(@"hh\:mm"), model.HoraFin.ToString(@"hh\:mm"));

				if (resultado > 0)
				{
					respuesta.Indicador = true;
					return Ok(respuesta);
				}

				respuesta.Indicador = false;
				respuesta.Mensaje = "No se pudieron obtener los datos de confirmación";
				return Ok(respuesta);
			}

		}

		[HttpPost]
		[Route("GuardarCalificacion")]
		public IActionResult GuardarCalificacion(CitaModel model)
		{
			using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value))
			{

				var respuesta = new RespuestaModel();
				var resultado = context.Execute("GuardarCalificacionCita",
					new
					{
						model.CitaId,
						model.CalificacionReview,
						model.ComentarioReview,
					});

				if (resultado > 0)
				{
					respuesta.Indicador = true;
					return Ok(respuesta);
				}

				respuesta.Indicador = false;
				respuesta.Mensaje = "No se pudo calificar la cita correctamente.";
				return Ok(respuesta);
			}

		}
		private TimeSpan RedondearHora(TimeSpan hora)
		{
			return new TimeSpan(hora.Hours + (hora.Minutes > 0 ? 1 : 0), 0, 0);
		}

        [HttpGet]
        [Route("DetalleCita")]
        public IActionResult DetalleCita(long id)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var cita = connection.QueryFirstOrDefault<CitaDetalleModel>("ObtenerDetalleCita", new { CitaId = id }, commandType: CommandType.StoredProcedure);

                if (cita != null)
                {
                    return Ok(new RespuestaModel
                    {
                        Indicador = true,
                        Datos = cita
                    });
                }

                return Ok(new RespuestaModel
                {
                    Indicador = false,
                    Mensaje = "No se encontró la cita."
                });
            }
        }



    }
}

