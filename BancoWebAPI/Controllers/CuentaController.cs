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

        [HttpPost("Post={dniCliente}")]
        public async Task<ActionResult<RespuestaExterna<bool>>> Post(Cuenta cuenta, int dniCliente)
        {
            var respuesta = new RespuestaExterna<bool>();
            var respuestaInterna = await _cuentaServicio.CrearCuentaAsync(cuenta, dniCliente);
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

        [HttpGet("GetCuentasPorDni={dni}")]
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

        [HttpGet("GetCuentasPorCbu={cbu}")]
        public async Task<ActionResult<RespuestaExterna<List<Cuenta>>>> Get(string cbu)
        {
            var respuesta = new RespuestaExterna<List<Cuenta>>();
            var respuestaInterna = await _cuentaServicio.ObtenerPorCbuAsync(cbu);
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

        [HttpDelete("Delete={id},{dni}")]
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
