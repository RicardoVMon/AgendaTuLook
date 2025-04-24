using AgendaTuLookAPI.Models;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Reflection;

namespace AgendaTuLookAPI.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class UsuariosController : ControllerBase
	{
		private readonly IConfiguration _configuration;
		public UsuariosController(IConfiguration configuration)
		{
			_configuration = configuration;
		}

        [HttpGet]
        [Route("PerfilUsuario")]
        public IActionResult PerfilUsuario(long Id)
        {
            using (var context = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
				// El id del usuario también lo podemos sacar del JWT
                var result = context.QueryFirstOrDefault<UsuarioModel>("ConsultarUsuario", new { UsuarioId = Id });
				var respuesta = new RespuestaModel();

				if (result != null)
				{
					respuesta.Indicador = true;
					respuesta.Datos = result;
				}
				else
				{
					respuesta.Indicador = false;
					respuesta.Mensaje = "No se pudo obtener el usuario";
				}
				return Ok(respuesta);
            }
        }

		[HttpPut]
		[Route("EditarPerfilUsuario")]
		public IActionResult EditarPerfilUsuario(UsuarioModel model)
		{
			using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value))
			{ 
				var respuesta = new RespuestaModel();
				var result = context.Query<String>("ActualizarUsuario",
				new
				{
                    model.UsuarioId,
                    model.Nombre,
                    model.Telefono,
                    model.Correo,
					model.Imagen
                }).FirstOrDefault();

				if (result == "1")
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

		[HttpPut]
		[Route("EditarContrasenniaUsuario")]
		public IActionResult EditarContrasenniaUsuario(UsuarioModel model)
		{
			using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value))
			{
				var respuesta = new RespuestaModel();
				
				if (model.NuevaContrasennia != model.ConfirmarContrasennia)
				{
					respuesta.Indicador = false;
					respuesta.Mensaje = "Las contraseñas no coinciden";
					return Ok(respuesta);
				}

				var resultValidacion = context.QueryFirstOrDefault<int>("ValidarContrasenniaActual",
					new
					{
						model.UsuarioId,
						ContrasenniaEnviada = model.Contrasennia,
					});

				if (resultValidacion > 0)
				{
					var result = context.Execute("CambiarContrasenna",
											new
											{
												model.Correo,
												model.NuevaContrasennia
											});
					respuesta.Indicador = true;
					respuesta.Mensaje = "Contraseña actualizada correctamente";
				}
				else
				{
					respuesta.Indicador = false;
					respuesta.Mensaje = "La contraseña actual no es correcta";
				}

				return Ok(respuesta);
			}
		}


	}
}


