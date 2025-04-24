using AgendaTuLookAPI.Models;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace AgendaTuLookAPI.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class DiasTrabajoController : Controller
	{

		private readonly IConfiguration _configuration;
		public DiasTrabajoController(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		[HttpGet]
		[Route("GestionarDiasTrabajo")]
		public IActionResult GestionarDiasTrabajo()
		{
			using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value))
			{
				var respuesta = new RespuestaModel();
				var servicios = context.Query<DiasTrabajoModel>("ConsultarDiasTrabajo", new { DiaTrabajoId = 0 });

				if (servicios.Any())
				{
					respuesta.Indicador = true;
					respuesta.Datos = servicios;
					return Ok(respuesta);
				}
				respuesta.Indicador = false;
				respuesta.Mensaje = "No se pudieron obtener los servicios";
				return Ok(respuesta);
			}
		}
		[HttpGet]
		[Route("CambiarEstadoDiaTrabajo")]
		public IActionResult CambiarEstadoDiaTrabajo(long id)
		{
			using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value))
			{
				var respuesta = new RespuestaModel();
				var resultado = context.Execute("CambiarEstadoDiaTrabajo", new { DiaTrabajoId = id });

				if (resultado > 0)
				{
					respuesta.Indicador = true;
					return Ok(respuesta);
				}
				respuesta.Indicador = false;
				respuesta.Mensaje = "No se pudo cambiar el estado del día de trabajo";
				return Ok(respuesta);
			}
		}

		[HttpPost]
		[Route("ActualizarHorasDiaTrabajo")]
		public IActionResult ActualizarHorasDiaTrabajo(DiasTrabajoModel model)
		{
			using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value))
			{
				var respuesta = new RespuestaModel();
				var resultado = context.Execute("ActualizarHorasDiaTrabajo", new { model.DiaTrabajoId, model.HoraInicio, model.HoraFin });

				if (resultado > 0)
				{
					respuesta.Indicador = true;
					return Ok(respuesta);
				}
				respuesta.Indicador = false;
				respuesta.Mensaje = "No se pudo cambiar el estado del día de trabajo";
				return Ok(respuesta);
			}
		}
	}
}
