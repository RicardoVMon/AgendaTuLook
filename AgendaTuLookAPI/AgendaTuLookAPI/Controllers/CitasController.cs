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

        [HttpPost]
        [Route("Cancelar")]
        //public IActionResult CancelarCita(long citaId)
        public IActionResult CancelarCita([FromBody] long citaId)
        {
            using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                var datosCita = context.QueryFirstOrDefault(@"SELECT u.Nombre, u.Correo, c.Fecha, c.HoraInicio, c.HoraFin, 
                                      s.NombreServicio, s.Precio, m.Nombre AS MetodoPago, c.Estado
                                      FROM tCitas c
                                      JOIN tUsuarios u ON c.UsuarioId = u.UsuarioId
                                      JOIN tServicios s ON c.ServicioId = s.ServicioId
                                      LEFT JOIN tPagos p ON p.CitaId = c.CitaId
                                      LEFT JOIN tMetodosPago m ON m.MetodoPagoId = p.MetodoPagoId
                                      WHERE c.CitaId = @CitaId", new { CitaId = citaId });

                var fechaCita = Convert.ToDateTime(datosCita.Fecha);
                var aplicaReembolso = fechaCita.AddDays(-1) >= DateTime.Now;

                var resultado = context.Execute("CancelarCita", new { CitaId = citaId });

                var estadoActualizado = context.QueryFirstOrDefault<string>(
                    "SELECT Estado FROM tCitas WHERE CitaId = @CitaId",
                    new { CitaId = citaId });

                if (estadoActualizado == "Cancelada")
                {
                    _correos.EnviarCorreoCancelacion(datosCita.Correo, datosCita.Nombre, datosCita.NombreServicio,
                                                 fechaCita.ToString("dd/MM/yyyy"),
                                                 ((TimeSpan)datosCita.HoraInicio).ToString(@"hh\:mm"),
                                                 ((TimeSpan)datosCita.HoraFin).ToString(@"hh\:mm"),
                                                 datosCita.MetodoPago ?? "No especificado",
                                                 Convert.ToDecimal(datosCita.Precio),
                                                 aplicaReembolso);
                    return Ok(new RespuestaModel
                    {
                        Indicador = true,
                        Mensaje = "Cita cancelada exitosamente"
                    });
                }

                return Ok(new RespuestaModel
                {
                    Indicador = false,
                    Mensaje = "No se pudo cancelar la cita"
                });
            }
        }

        //------------------------------------------------

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

                // Verificar si el servicio cambió
                var servicioOriginal = context.QueryFirstOrDefault<long>(
                    "SELECT ServicioId FROM tCitas WHERE CitaId = @CitaId",
                    new { model.CitaId });

                bool servicioCambiado = servicioOriginal != model.Servicio.ServicioId;

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
                        NuevoMetodoPagoId = servicioCambiado ? model.MetodoPago?.MetodoPagoId : (long?)null,
                        Comprobante = servicioCambiado ? model.MetodoPago?.Comprobante : null,
                        ServicioCambiado = servicioCambiado ? 1 : 0
                    });

                if (resultado > 0)
                {
                    // Obtener datos completos de la cita para el correo
                    var citaActualizada = context.QueryFirstOrDefault<dynamic>(
                        @"SELECT c.*, u.Nombre AS NombreCliente, u.Correo, s.NombreServicio, s.Precio, 
                         mp.Nombre AS MetodoPagoNombre
                          FROM tCitas c
                          JOIN tUsuarios u ON c.UsuarioId = u.UsuarioId
                          JOIN tServicios s ON c.ServicioId = s.ServicioId
                          LEFT JOIN tPagos p ON p.CitaId = c.CitaId
                          LEFT JOIN tMetodosPago mp ON mp.MetodoPagoId = p.MetodoPagoId
                          WHERE c.CitaId = @CitaId",
                        new { model.CitaId });

                    // Enviar correo de confirmación
                    _correos.EnviarCorreoFacturaCitaEdicion(
                        citaActualizada.Correo,
                        citaActualizada.NombreCliente,
                        citaActualizada.NombreServicio,
                        (double)citaActualizada.Precio,
                        citaActualizada.MetodoPagoNombre ?? "No especificado",
                        model.Fecha.ToString("dd/MM/yyyy"),
                        model.HoraInicio.ToString(@"hh\:mm"),
                        model.HoraFin.ToString(@"hh\:mm"),
                        servicioCambiado);

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

