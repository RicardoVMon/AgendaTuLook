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
        public IActionResult ConsultarEstadisticas(string fechaInicial, string fechaFinal)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var Estadistica = connection.QuerySingleOrDefault<EstadisticasModel>("Estadisticas", new {
                    InicioMesActual = fechaInicial,
                    FinMesActual = fechaFinal
                },
                commandType: CommandType.StoredProcedure);

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
