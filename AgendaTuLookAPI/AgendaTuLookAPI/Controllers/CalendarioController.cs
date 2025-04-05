using AgendaTuLookAPI.Models;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace AgendaTuLookAPI.Controllers
{
	[Route("api/[controller]")]
	[Authorize]
	[ApiController]
	public class CalendarioController : Controller
	{

		private readonly IConfiguration _configuration;
		public CalendarioController(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		[HttpGet]
		[Route("ConsultarCitasCalendario")]
		public IActionResult ConsultarCitasCalendario(long Id, long c)
		{
			using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value))
			{
				var respuesta = new RespuestaModel();

				var resultado = context.Query<CitaModel>("ConsultarCitasCalendario",
				new
				{
					UsuarioId = Id,
					Completadas = c
				});

				if (resultado.Any())
				{
					respuesta.Indicador = true;
					respuesta.Datos = resultado;
					return Ok(respuesta);
				}

				respuesta.Indicador = false;
				respuesta.Mensaje = "No se pudieron obtener las citas del usuario";
				return Ok(respuesta);
			}

		}

	}
}
