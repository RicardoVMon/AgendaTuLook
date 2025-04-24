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
using iText.Commons.Actions.Contexts;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

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
		public IActionResult ConsultarDatosConfirmar([FromBody] CitaModel model, [FromQuery] int e)
		{

			using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value))
			{
				var respuesta = new RespuestaModel();
				var informacionConfirmar = context.Query("ConsultarDatosConfirmar", new { model.Usuario!.UsuarioId, model.Servicio!.ServicioId, model.Fecha });

				if (informacionConfirmar.Any())
				{
					// Proceso para obtener los metodos de pago
					var metodosPago = context.Query<MetodoPagoModel>("ObtenerMetodosPago");

					var comprobante = "";
					if (e == 1)
					{
						comprobante = context.QueryFirstOrDefault<string>("ObtenerFacturaCita", new { model.CitaId });
					}

					// Proceso para redondear siguiente hora
					var firstInfo = informacionConfirmar.First();
					var horaInicio = TimeSpan.Parse(model.HoraInicio.ToString());
					var duracion = TimeSpan.FromMinutes(firstInfo.Duracion);
					var horaFin = RedondearHora(horaInicio + duracion);

					CitaModel confirmarCita = new CitaModel
					{
                        CitaId = model.CitaId,
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
							Duracion = firstInfo.Duracion,
                            CambioServicio = model.Servicio.CambioServicio,
						},
						DiaTrabajoId = firstInfo.DiaTrabajoId,
						MetodosPago = metodosPago.ToList(),
						MetodoPago = new MetodoPagoModel
						{
							Comprobante = comprobante
						},
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
				bool respuestaCorreo = _correos.EnviarCorreoFacturaCita(model.Usuario.Correo!, model.Usuario.Nombre!, model.Servicio.NombreServicio!, model.Servicio.Precio!, model.MetodoPago.Nombre!, model.Fecha.ToString("dd/MM/yyyy"), model.HoraInicio.ToString(@"hh\:mm"), model.HoraFin.ToString(@"hh\:mm"));

				if (resultado > 0 && respuestaCorreo)
				{
					respuesta.Indicador = true;
					return Ok(respuesta);
				}

				respuesta.Indicador = false;
				respuesta.Mensaje = "Ha ocurrido un error en el proceso de confirmación de cita, por favor contácte al equipo de soporte si sigue ocurriendo";
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

		[HttpGet]
		[Route("GenerarFacturaDescargableCita")]
		public IActionResult GenerarFacturaDescargableCita(long id)
		{
			using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				var cita = connection.QueryFirstOrDefault<CitaDetalleModel>("ObtenerDetalleCita", new { CitaId = id }, commandType: CommandType.StoredProcedure);

				if (cita == null)
				{
					return NotFound("No se encontró la cita.");
				}

				var pdfStream = _correos.GenerarFacturaPDF(
					cita.NombreUsuario,
					cita.NombreServicio,
					(double)cita.Precio,
					cita.NombreMetodoPago != null ? cita.NombreMetodoPago : "No disponible (Cita Cancelada)",
					cita.Fecha.ToString("dd/MM/yyyy"),
					DateTime.Today.Add(cita.HoraInicio).ToString("hh:mm tt"),
					DateTime.Today.Add(cita.HoraFin).ToString("hh:mm tt")
				);

				pdfStream.Position = 0;
				return File(pdfStream, "application/pdf", $"Factura_{id}.pdf");
			}
		}


		[HttpGet]
        [Route("Cancelar")]
        public IActionResult CancelarCita(long id)
        {
            using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value))
            {

                var datosCita = context.QueryFirstOrDefault("ObtenerCitaParaEditar", new { CitaId = id });
                var aplicaReembolso = datosCita!.Fecha.AddDays(-1) >= DateTime.Now;

                var resultado = context.Execute("CancelarCita", new { CitaId = id });

				bool resultadoCorreo = _correos.EnviarCorreoCancelacion(datosCita.UsuarioCorreo, datosCita.UsuarioNombre, datosCita.NombreServicio,
												datosCita.Fecha.ToString("dd/MM/yyyy"),
												((TimeSpan)datosCita.HoraInicio).ToString(@"hh\:mm"),
												((TimeSpan)datosCita.HoraFin).ToString(@"hh\:mm"),
												datosCita.MetodoPagoNombre ?? "No especificado",
												Convert.ToDecimal(datosCita.Precio),
												aplicaReembolso);


				if (resultado > 0 && resultadoCorreo)
				{
					return Ok(new RespuestaModel
					{
						Indicador = true,
						Mensaje = "Cita cancelada exitosamente"
					});
				}

				return Ok(new RespuestaModel
                {
                    Indicador = false,
                    Mensaje = "Ha ocurrido un error al cancelar la cita"
                });
            }
        }

        [HttpPost]
        [Route("ObtenerCitaParaEditar")]
        public IActionResult ObtenerCitaParaEditar(CitaModel model)
        {
            using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                var respuesta = new RespuestaModel();

                var citaData = context.QueryFirstOrDefault<dynamic>("ObtenerCitaParaEditar", new { model.CitaId });

                if (citaData != null)
                {
                    var cita = new CitaModel
                    {
                        CitaId = citaData.CitaId,
                        Usuario = new UsuarioModel { UsuarioId = citaData.UsuarioId },
                        Servicio = new ServicioModel
                        {
                            ServicioId = citaData.ServicioId,
                            NombreServicio = citaData.NombreServicio,
                            Precio = Convert.ToDouble(citaData.Precio),
                            Duracion = citaData.Duracion,
                            Imagen = citaData.Imagen,
                            Descripcion = citaData.Descripcion
                        },
                        Fecha = citaData.Fecha,
                        HoraInicio = citaData.HoraInicio,
                        HoraFin = citaData.HoraFin,
                        Estado = citaData.Estado,
                        DiaTrabajoId = citaData.DiaTrabajoId,
                        MetodoPago = new MetodoPagoModel
                        {
                            MetodoPagoId = citaData.MetodoPagoId,
                            Nombre = citaData.MetodoPagoNombre
                        },
                        PrecioOriginal = Convert.ToDouble(citaData.Precio) 
                    };

                    // Obtener métodos de pago
                    var metodosPago = context.Query<MetodoPagoModel>("ObtenerMetodosPago");
                    cita.MetodosPago = metodosPago.ToList();

                    respuesta.Indicador = true;
                    respuesta.Datos = cita;
                    return Ok(respuesta);
                }

                respuesta.Indicador = false;
                respuesta.Mensaje = "No se pudo obtener la información de la cita";
                return Ok(respuesta);
            }
        }

        [HttpPost]
        [Route("VerificarCitaEditable")]
        public IActionResult VerificarCitaEditable(CitaModel model)
        {
            using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                var respuesta = new RespuestaModel();
                var resultado = context.QueryFirstOrDefault<dynamic>("VerificarCitaEditable", new { model.CitaId });

                respuesta.Indicador = resultado?.EsEditable == 1;
                respuesta.Mensaje = respuesta.Indicador ? "La cita es editable" : "La cita no se puede editar (tiene menos de 24 horas de anticipación)";
                return Ok(respuesta);
            }
        }

        [HttpPost]
        [Route("ActualizarCita")]
        public IActionResult ActualizarCita(CitaModel model)
        {
            using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                var respuesta = new RespuestaModel();

                // Actualizar la cita
                var resultado = context.Execute("ActualizarCita",
                    new
                    {
                        model.CitaId,
                        model.Servicio.ServicioId,
                        model.Fecha,
                        model.HoraInicio,
                        model.HoraFin,
                        model.DiaTrabajoId,
                        model.MetodoPago.MetodoPagoId,
                        model.MetodoPago.Comprobante,
						ServicioCambiado = model.Servicio.CambioServicio ? 1 : 0
                    });

				if (resultado > 0)
                {
					// Enviar correo de confirmación
					_correos.EnviarCorreoFacturaCitaEdicion(
						model.Usuario.Correo,
						model.Usuario.Nombre,
						model.Servicio.NombreServicio,
						model.Servicio.Precio,
						model.MetodoPago.Nombre,
						model.Fecha.ToString("dd/MM/yyyy"),
						model.HoraInicio.ToString(@"hh\:mm"),
						model.HoraFin.ToString(@"hh\:mm"),
						model.Servicio.CambioServicio);

					respuesta.Indicador = true;
                    return Ok(respuesta);
                }

                respuesta.Indicador = false;
                respuesta.Mensaje = "No se pudo actualizar la cita";
                return Ok(respuesta);
            }
        }

        [HttpGet]
        [Route("ObtenerMetodosPago")]
        public IActionResult ObtenerMetodosPago()
        {
            using (var context = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var respuesta = new RespuestaModel();
                var metodos = context.Query<MetodoPagoModel>("ObtenerMetodosPago");

                respuesta.Indicador = true;
                respuesta.Datos = metodos;
                return Ok(respuesta);
            }
        }


    }
}

