using AgendaTuLookAPI.Models;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace AgendaTuLookAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsuariosController : ControllerBase
	{
		private readonly IConfiguration _configuration;
		private readonly IHttpClientFactory _httpClient;
		public UsuariosController(IConfiguration configuration, IHttpClientFactory httpClient)
		{
			_httpClient = httpClient;
			_configuration = configuration;
		}

        [HttpGet]
        [Route("PerfilUsuario")]
        public IActionResult PerfilUsuario([FromQuery] long UsuarioId)
        {
            try
            {
                using (var context = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var parameters = new { UsuarioId };
                    var result = context.QueryFirstOrDefault<UsuarioModel>("ConsultarUsuario", parameters, commandType: CommandType.StoredProcedure);

                    if (result == null)
                    {
                        return NotFound(new { message = "Usuario no encontrado" });
                    }

                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error en el servidor", error = ex.Message });
            }
        }


		[HttpPost]
		[Route("EditarPerfilUsuario")]
		public IActionResult EditarPerfilUsuario(UsuarioModel model)
		{
			try
			{
				using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value))
				{
                   

                    // Ejecutar el procedimiento almacenado usando el modelo
                    var result = context.Execute("ActualizarUsuario",
						new
						{
                            model.UsuarioId,
                            model.Nombre,
                            model.Telefono,
                            model.Correo,
                            Contrasennia = model.NuevaContrasennia
                        });

					var respuesta = new RespuestaModel();

					if (result > 0)
					{
						respuesta.Indicador = true;
						respuesta.Mensaje = "Usuario actualizado correctamente";
					}
					else
					{
						respuesta.Indicador = false;
						respuesta.Mensaje = "No se pudo actualizar el usuario";
					}

					return Ok(respuesta);
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { Message = "Error en el servidor", Error = ex.Message });
			}
		}


	}
}


