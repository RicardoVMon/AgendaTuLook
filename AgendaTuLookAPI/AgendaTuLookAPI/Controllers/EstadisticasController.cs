using System.Data;
using AgendaTuLookAPI.Models;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace AgendaTuLookAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]

	public class EstadisticasController : Controller
	{
		private readonly IConfiguration _configuration;

		public EstadisticasController(IConfiguration configuration)
		{
			_configuration = configuration;

		}

		[HttpGet]
		[Route("ConsultarEstadisticas")]
		public IActionResult ConsultarEstadisticas(DateTime fechaInicial, DateTime fechaFinal)
		{
			Console.WriteLine("fecha inicial " + fechaInicial);
			Console.WriteLine("fecha final " + fechaFinal);
			using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				var Estadistica = connection.QuerySingleOrDefault<EstadisticasModel>("Estadisticas", new
				{
					fechaInicio = fechaInicial,
					fechaFinal = fechaFinal
				});

				var Reviews = connection.Query<ReviewsModel>("ConsultarReviewsPorFecha", new
				{
					fechaInicio = fechaInicial,
					fechaFinal = fechaFinal
				});

				using (var multi = connection.QueryMultiple("EstadisticasFinancieras", new
				{
					fechaInicio = fechaInicial,
					fechaFinal = fechaFinal
				},
				commandType: CommandType.StoredProcedure))
				{
					var ingresosPorServicio = multi.Read<ServicioModel>().ToList();
					var ingresoTotal = multi.ReadFirstOrDefault<decimal>();

					if (Estadistica != null)
					{
						Estadistica.Reviews = Reviews?.ToList();
						Estadistica.Servicios = ingresosPorServicio;
						Estadistica.IngresosTotales = ingresoTotal;

						return Ok(new RespuestaModel
						{
							Indicador = true,
							Datos = Estadistica
						});
					}
				}

				return Ok(new RespuestaModel
				{
					Indicador = false,
					Mensaje = "No se encontró la estadística."
				});
			}
		}

		[HttpGet("Reviews")]
		public IActionResult GetReviews()
		{
			using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				var reviews = connection.Query<ReviewDestacadoModel>(
					"ObtenerReviewsDestacados",
					commandType: CommandType.StoredProcedure
				).ToList();

				if (reviews.Any())
				{
					return Ok(new RespuestaModel
					{
						Indicador = true,
						Datos = reviews
					});
				}

				return Ok(new RespuestaModel
				{
					Indicador = false,
					Mensaje = "No se encontraron reseñas."
				});
			}
		}
	}
}




