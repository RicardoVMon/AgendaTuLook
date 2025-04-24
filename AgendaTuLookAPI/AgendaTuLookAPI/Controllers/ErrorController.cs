using AgendaTuLookAPI.Models;
using Dapper;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace AgendaTuLookAPI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorController : Controller
	{
        private readonly IConfiguration _configuration;
        public ErrorController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("CapturarError")]
		public IActionResult CapturarError()
		{
            var ex = HttpContext.Features.Get<IExceptionHandlerFeature>();

            using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value)) 
            {
                var UsuarioId = ObtenerUsuarioFromToken();
                var Mensaje = ex!.Error.Message;
                var Origen = ex.Path;

			    var respuesta = new RespuestaModel();
                var resultado = context.Execute("RegistrarError", new  { UsuarioId, Mensaje,Origen});

                if (resultado > 0)
                {
					respuesta.Indicador = true;
					return Ok(respuesta);
                }

                respuesta.Indicador = false;
			    respuesta.Mensaje = "Se presentó un problema en el sistema.";
                return Ok(respuesta);
			}
        }

        public long ObtenerUsuarioFromToken() {

            if (User.Claims.Any())
            {
                var IdUsuario = User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;
                return long.Parse(IdUsuario!);
            }
            return 0;
        
        }
	}
}
