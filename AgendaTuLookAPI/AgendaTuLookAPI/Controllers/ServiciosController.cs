using AgendaTuLookAPI.Models;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Reflection;

namespace AgendaTuLookAPI.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class ServiciosController : Controller
	{

		private readonly IConfiguration _configuration;
        public ServiciosController(IConfiguration configuration)
		{
			_configuration = configuration;
        }

		[HttpGet]
		[Route("GestionarServicios")]
		public IActionResult GestionarServicios()
		{
			using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value))
			{
				var respuesta = new RespuestaModel();
				// ServicioId es 0 para que traiga todos
				var servicios = context.Query<ServicioModel>("ConsultarServicios", new { ServicioId = 0 });

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

		[HttpPost]
		[Route("CrearServicio")]
		public IActionResult CrearServcio(ServicioModel model)
		{

			using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value))
			{
				var respuesta = new RespuestaModel();

				var resultServicioId = context.QueryFirstOrDefault<long>("CrearServicio", new { model.NombreServicio, model.Descripcion, model.Precio, model.Duracion, model.Imagen });

				if (resultServicioId > 0)
				{
					respuesta.Indicador = true;
					return Ok(respuesta);
				}

				respuesta.Indicador = false;
				respuesta.Mensaje = "No se pudo crear el servicio";
				return Ok(respuesta);
			}

		}

		[HttpGet]
		[Route("EditarServicio")]
		public IActionResult EditarServicio(long id)
		{
			using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value))
			{
				var respuesta = new RespuestaModel();
				var servicios = context.QueryFirstOrDefault<ServicioModel>("ConsultarServicios", new { ServicioId = id });
				if (servicios != null)
				{
					respuesta.Indicador = true;
					respuesta.Datos = servicios;
					return Ok(respuesta);
				}
				respuesta.Indicador = false;
				respuesta.Mensaje = "No se pudieron obtener los datos del servicio";
				return Ok(respuesta);
			}
		}

		[HttpPut]
		[Route("EditarServicio")]
		public IActionResult EditarServicio(ServicioModel model)
		{
			using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value))
			{
				var respuesta = new RespuestaModel();
				var servicios = context.Execute("ActualizarServicio", new { model.ServicioId, model.NombreServicio, model.Descripcion, model.Precio, model.Duracion, model.Imagen });
				if (servicios > 0)
				{
					respuesta.Indicador = true;
					return Ok(respuesta);
				}
				respuesta.Indicador = false;
				respuesta.Mensaje = "No se pudo actualizar el servicio correctamente";
				return Ok(respuesta);
			}
		}

		[HttpGet]
		[Route("CambiarEstadoServicio")]
		public IActionResult CambiarEstadoServicio(long id)
		{
			using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value))
			{
				var respuesta = new RespuestaModel();
				// ServicioId es 0 para que traiga todos
				var resultado = context.Execute("CambiarEstadoServicio", new { ServicioId = id });

				if (resultado > 0)
				{
					respuesta.Indicador = true;
					return Ok(respuesta);
				}
				respuesta.Indicador = false;
				respuesta.Mensaje = "No se pudo cambiar el estado del servicio";
				return Ok(respuesta);
			}
		}

        [HttpGet]
        [Route("ObtenerServicioPorId")]
        public IActionResult ObtenerServicioPorId(long servicioId)
        {
            using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                var respuesta = new RespuestaModel();
                var servicio = context.QueryFirstOrDefault<ServicioModel>("ConsultarServicios", new { ServicioId = servicioId });

                if (servicio != null)
                {
                    respuesta.Indicador = true;
                    respuesta.Datos = servicio;
                    return Ok(respuesta);
                }

                respuesta.Indicador = false;
                respuesta.Mensaje = "No se pudo obtener el servicio";
                return Ok(respuesta);
            }
        }


    }
}
