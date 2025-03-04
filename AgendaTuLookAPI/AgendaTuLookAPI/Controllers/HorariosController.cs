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
        public IActionResult MostrarHorarios(string Estado)
        {
            try
            {
                using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value))
                {
                    //Se llama al procediminto almacenado para mostrar los horarios activos
                    //Estado debe ser solo "Activo" o "Inactivo"
                    //que hacer: validaciones a estado
                    var result = context.Query<HorariosModel>("MostrarHorarios", new {Estado});

                    if (result != null)
                    {
                        //Devuelve el resultado del query como una lista de horarios
                        return Ok(result);
                    }
                    //en cualquier caso si falla la request devuelve nulo
                    return NotFound(null);

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
                    /*Se ejecuta el procedimiento Registrar Horario
                     *  que hacer: validar que el dia que se ponga sea valido (Lunes-domingo)
                     * validar que la hora siga un formado(HH: MM)
                     *
                     * 
                     */

                    var result = context.Execute("RegistrarHorario",
                    new { model.HoraEntrada, model.HoraSalida, model.dia});
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
                    /*que hacer: Validar estado que sea (Activo/Inactivo)
                     * validar que la hora siga un formado(HH: MM)
                     * validar que el dia que se ponga sea valido (Lunes-domingo)
                    */
                    var result = context.Execute("ActualizarHorario",
                    new {model.HorariosId, model.HoraEntrada, model.HoraSalida, model.dia, model.Estado});
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
                    /*que hacer: 
                    */
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
    }
}
