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
                


                
             var Reviews = connection.Query<ReviewsModel>("ConsultarReviewsPorFecha", new
             {
                    fechaInicio = fechaInicial,
                    fechaFinal = fechaFinal
             },
            commandType: CommandType.StoredProcedure);
                
                
               
                if (Estadistica != null)
                {
                    if(Reviews != null) 
                    {
                        
                        Estadistica.Reviews = Reviews.ToList();
                    }
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
