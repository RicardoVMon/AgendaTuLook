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
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var Estadistica = connection.QuerySingleOrDefault<EstadisticasModel>("Estadisticas", new
                {
                    fechaInicio = fechaInicial,
                    fechaFinal = fechaFinal
                },
                commandType: CommandType.StoredProcedure);
                //Reviws no existe en el branch actual cuando exista descomentar esta parte


                /*
             var Reviews = connection.Query<ReviewsModel>("BuscarReviewsPorFecha", new
             {
                    InicioMesActual = fechaInicial,
                    FinMesActual = fechaFinal
             },
            commandType: CommandType.StoredProcedure);

            Estadistica.reviews = Reviews
                */
                if (Estadistica != null)
                {
                    return Ok(new RespuestaModel
                    {
                        Indicador = true,
                        Datos = Estadistica
                    });
                }

                return Ok(new RespuestaModel
                {
                    Indicador = false,
                    Mensaje = "No se encontró la estadistica."
                });
            }
        }
    }
    
}
