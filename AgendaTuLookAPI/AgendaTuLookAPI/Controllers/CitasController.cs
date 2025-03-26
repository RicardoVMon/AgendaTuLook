using AgendaTuLookAPI.Models;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace AgendaTuLookAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CitasController : Controller
	{

		private readonly IConfiguration _configuration;
		public CitasController(IConfiguration configuration)
		{
			_configuration = configuration;
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
	}
}
