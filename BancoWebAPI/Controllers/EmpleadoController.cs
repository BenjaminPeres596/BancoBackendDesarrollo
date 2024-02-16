using Microsoft.AspNetCore.Mvc;
using Data.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Data.Data;
using Microsoft.EntityFrameworkCore;
using Servicios;
using Core.DTO;

namespace BancoWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmpleadoController : ControllerBase
    {
        private readonly ILogger<EmpleadoController> _logger;
        private readonly IEmpleadoServicio _empleadoServicio;

        public EmpleadoController(IEmpleadoServicio empleadoServicio, ILogger<EmpleadoController> logger)
        {
            _empleadoServicio = empleadoServicio;
            _logger = logger;
        }

        [HttpPost("Post")]
        public async Task<ActionResult<RespuestaExterna<bool>>> Post(Empleado empleado)
        {
            var respuesta = new RespuestaExterna<bool>();
            var respuestaInterna = await _empleadoServicio.AgregarEmpleado(empleado);
            try
            {
                respuesta.MensajePublico = respuestaInterna.Mensaje;
                respuesta.Datos = respuestaInterna.Datos;
                respuesta.Exito = respuestaInterna.Exito;
                return respuesta;
            }
            catch
            {
                respuesta.MensajePublico = respuestaInterna.Mensaje;
                return StatusCode(StatusCodes.Status500InternalServerError, respuesta);
            }
        }

        [HttpGet("Get")]
        public async Task<ActionResult<RespuestaExterna<List<Empleado>>>> Get()
        {
            var respuesta = new RespuestaExterna<List<Empleado>>();
            var respuestaInterna = await _empleadoServicio.ObtenerAsync();
            try
            {
                if (respuestaInterna.Exito)
                {
                    respuesta.Exito = true;
                    respuesta.MensajePublico = "Empleados recuperados correctamente";
                    respuesta.Datos = respuestaInterna.Datos;
                    return respuesta;
                }
                else
                {
                    respuesta.MensajePublico = respuestaInterna.Mensaje;
                    return BadRequest(respuesta);
                }
            }
            catch
            {
                respuesta.MensajePublico = respuestaInterna.Mensaje;
                return StatusCode(StatusCodes.Status500InternalServerError, respuesta);
            }
        }

        [HttpGet("GetPorLegajo={legajo}")]
        public async Task<ActionResult<RespuestaExterna<Empleado>>> Get(int legajo)
        {
            var respuesta = new RespuestaExterna<Empleado>();
            var respuestaInterna = await _empleadoServicio.ObtenerPorLegajoAsync(legajo);
            try
            {
                if (respuestaInterna.Exito)
                {
                    respuesta.Exito = true;
                    respuesta.Datos = respuestaInterna.Datos;
                    respuesta.MensajePublico = "Empleado recuperado correctamente";
                    return respuesta;
                }
                else
                {
                    respuesta.MensajePublico = respuestaInterna.Mensaje;
                    return BadRequest(respuesta);
                }
            }
            catch
            {
                respuesta.MensajePublico = respuestaInterna.Mensaje;
                return StatusCode(StatusCodes.Status500InternalServerError, respuesta);
            }
        }

        [HttpDelete("Delete={legajo}")]
        public async Task<ActionResult<RespuestaExterna<bool>>> Delete(int legajo)
        {
            var respuesta = new RespuestaExterna<bool>();
            var respuestaInterna = await _empleadoServicio.EliminarAsync(legajo);
            try
            {
                if (respuestaInterna.Exito)
                {
                    respuesta.Datos = true;
                    respuesta.Exito = true;
                    return StatusCode(StatusCodes.Status200OK, respuesta);
                }
                else
                {
                    respuesta.MensajePublico = respuestaInterna.Mensaje;
                    return BadRequest(respuesta);
                }
            }
            catch
            {
                respuesta.MensajePublico = respuestaInterna.Mensaje;
                return StatusCode(StatusCodes.Status500InternalServerError, respuesta);
            }
        }
    }
}
