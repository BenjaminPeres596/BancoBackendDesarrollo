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
    public class CuentaController : ControllerBase
    {
        private readonly ILogger<CuentaController> _logger;
        private readonly ICuentaServicio _cuentaServicio;

        public CuentaController(ICuentaServicio cuentaServicio, ILogger<CuentaController> logger)
        {
            _cuentaServicio = cuentaServicio;
            _logger = logger;
        }

        [HttpPost(Name = "PostCuenta")]
        public async Task<ActionResult<RespuestaExterna<bool>>> Post(Cuenta cuenta, int dni)
        {
            var respuesta = new RespuestaExterna<bool>();
            var respuestaInterna = await _cuentaServicio.CrearCuentaAsync(cuenta, dni);
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

        [HttpGet(Name = "GetCuentas")]
        public async Task<ActionResult<RespuestaExterna<List<Cuenta>>>> Get()
        {
            var respuesta = new RespuestaExterna<List<Cuenta>>();
            var respuestaInterna = await _cuentaServicio.ObtenerAsync();
            try
            {
                if (respuestaInterna.Exito)
                {
                    respuesta.Exito = true;
                    respuesta.MensajePublico = "Cuentas recuperados correctamente";
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

        [HttpGet("{dni}", Name = "GetCuentasPorDni")]
        public async Task<ActionResult<RespuestaExterna<List<Cuenta>>>> Get(int dni)
        {
            var respuesta = new RespuestaExterna<List<Cuenta>>();
            var respuestaInterna = await _cuentaServicio.ObtenerPorDniAsync(dni);
            try
            {
                if (respuestaInterna.Exito)
                {
                    respuesta.Exito = true;
                    respuesta.Datos = respuestaInterna.Datos;
                    respuesta.MensajePublico = "Cuenta recuperada correctamente";
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

        [HttpDelete("{id},{dni}", Name = "EliminarCuenta")]
        public async Task<ActionResult<RespuestaExterna<bool>>> Delete(int id, int dni)
        {
            var respuesta = new RespuestaExterna<bool>();
            var respuestaInterna = await _cuentaServicio.EliminarAsync(id,dni);
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
                    if (respuestaInterna.Mensaje.Contains("cuit")){
                        respuesta.MensajePublico = "Intente con otro DNI.";
                    }
                    if (respuestaInterna.Mensaje.Contains("cuenta"))
                    {
                        respuesta.MensajePublico = "Intente con otro numero de cuenta.";
                    }
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
