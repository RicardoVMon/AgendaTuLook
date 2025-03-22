using System.ComponentModel.DataAnnotations;
using AgendaTuLookAPI.Models;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace AgendaTuLookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HorariosController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public HorariosController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("MostrarHorarios")]
        public IActionResult MostrarHorarios(Boolean Estado)
        {
            try
            {
                using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value))
                {
                    var respuesta = new RespuestaModel();
                    //Se llama al procediminto almacenado para mostrar los horarios activos
                    var result = context.Query<HorariosModel>("MostrarHorarios", new {Estado});

                    if (result != null)
                    {
                        respuesta.Indicador = true;
                        respuesta.Datos = result;
                        return Ok(respuesta);
                    }
                    else
                    {
                        respuesta.Indicador = false;
                        respuesta.Mensaje = "Su información no se ha validado correctamente";
                    }

                    return Ok(respuesta);

                }
            }
            catch (SqlException ex)
            {
                return NotFound("Algo sucedio en el servidor: " + ex.Message );
            }
            catch (Exception ex) {
                return NotFound(ex.Message);
            }
            
            
        }

        [HttpPost]
        [Route("RegistrarHorario")]
        public IActionResult RegistrarHorarios(HorariosModel model)
        {
            try
            {
                using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value))
                {
                    //Se ejecuta el procedimiento Registrar Horario

                    var result = context.Execute("RegistrarHorario",
                    new { model.StartDate, model.EndDate});
                    var respuesta = new RespuestaModel();

                    if (result > 0)
                        respuesta.Indicador = true;
                    else
                    {
                        respuesta.Indicador = false;
                        respuesta.Mensaje = "Verifique los campos";
                    }

                    return Ok(respuesta);
                }
            }
            catch (SqlException ex)
            {
                return NotFound("Algo sucedio en el servidor: " + ex.Message);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }


        }

        [HttpPut]
        [Route("ActualizarHorario")]
        public IActionResult ActualizarHorario(HorariosModel model)
        {
            try
            {
                using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value))
                {
                    //Se ejecuta el procedimiento Actualizar Horario
                    var result = context.Execute("ActualizarHorario",
                    new {model.HorariosId, model.StartDate, model.EndDate, model.Estado});
                    var respuesta = new RespuestaModel();

                    if (result > 0)
                        respuesta.Indicador = true;
                    else
                    {
                        respuesta.Indicador = false;
                        respuesta.Mensaje = "Verifique los campos";
                    }

                    return Ok(respuesta);
                }
            }
            catch (SqlException ex)
            {
                return NotFound("Algo sucedio en el servidor: " + ex.Message);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }


        }

        [HttpDelete]
        [Route("EliminarHorario")]
        public IActionResult EliminarHorario(long HorariosId)
        {
            try
            {
                using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value))
                {
                    //Se ejecuta el procedimiento Elimnar Horario

                    var result = context.Execute("EliminarHorario",
                    new { HorariosId });
                    var respuesta = new RespuestaModel();

                    if (result > 0)
                        respuesta.Indicador = true;
                    else
                    {
                        respuesta.Indicador = false;
                        respuesta.Mensaje = "Verifique los campos";
                    }

                    return Ok(respuesta);
                }
            }
            catch (SqlException ex)
            {
                return NotFound("Algo sucedio en el servidor: " + ex.Message);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }


        }

        [HttpGet]
        [Route("MostrarHorario")]
        public IActionResult MostrarHorario(long HorariosId)
        {
            try
            {
                using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value))
                {
                    var respuesta = new RespuestaModel();
                    //Se llama al procediminto almacenado para mostrar los horarios activos
                    var result = context.QueryFirstOrDefault<HorariosModel>("MostrarHorario", new { HorariosId });
                    if (result != null)
                    {
                        respuesta.Indicador = true;
                        respuesta.Datos = result;
                        return Ok(respuesta);
                    }
                    else
                    {
                        respuesta.Indicador = false;
                        respuesta.Mensaje = "Su información no se ha validado correctamente";
                    }

                    return Ok(respuesta);

                }
            }
            catch (SqlException ex)
            {
                return NotFound("Algo sucedio en el servidor: " + ex.Message);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }


        }
    }
}
