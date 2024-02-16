using Core.DTO;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Servicios;

namespace BancoWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TipoCuentaController : ControllerBase
    {
        private readonly ILogger<TipoCuentaController> _logger;
        private readonly ITipoCuentaServicio _tipoCuentaServicio;

        public TipoCuentaController(ITipoCuentaServicio tipoCuentaServicio, ILogger<TipoCuentaController> logger)
        {
            _tipoCuentaServicio = tipoCuentaServicio;
            _logger = logger;
        }

        [HttpPost("Post")]
        public async Task<ActionResult<RespuestaExterna<bool>>> Post(TipoCuenta tipocuenta)
        {
            var respuesta = new RespuestaExterna<bool>();
            var respuestaInterna = await _tipoCuentaServicio.Post(tipocuenta);
            try
            {
                if (respuestaInterna.Exito == true)
                {
                    respuesta.MensajePublico = respuestaInterna.Mensaje;
                    respuesta.Exito = true;
                    respuesta.Datos = true;
                    return Ok(respuesta);
                }
                else
                {
                    respuesta.MensajePublico = respuestaInterna.Mensaje;
                    respuesta.Datos = true;
                    return BadRequest(respuesta);
                }
            }
            catch
            {
                respuesta.MensajePublico = respuestaInterna.Mensaje;
                respuesta.Datos = respuestaInterna.Datos;
                return StatusCode(StatusCodes.Status500InternalServerError, respuesta);
            }
        }

        [HttpGet("Get")]
        public async Task<ActionResult<RespuestaExterna<List<TipoCuenta>>>> Get()
        {
            var respuesta = new RespuestaExterna<List<TipoCuenta>>();
            var respuestaInterna = await _tipoCuentaServicio.Get();
            try
            {
                if (respuestaInterna.Exito == true)
                {
                    respuesta.Exito = true;
                    respuesta.Datos = respuestaInterna.Datos;
                    respuesta.MensajePublico = respuestaInterna.Mensaje;
                    return Ok(respuesta);
                }
                else
                {
                    respuesta.Datos = respuestaInterna.Datos;
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

        [HttpDelete("Delete={idCuenta}")]
        public async Task<ActionResult<RespuestaExterna<bool>>> Delete(int idCuenta)
        {
            var respuesta = new RespuestaExterna<bool>();
            var respuestaInterna = await _tipoCuentaServicio.Delete(idCuenta);
            try
            {
                if (respuestaInterna.Exito == true)
                {
                    respuesta.Exito = true;
                    respuesta.Datos = true;
                    respuesta.MensajePublico = respuestaInterna.Mensaje;
                    return Ok(respuesta);
                }
                else
                {
                    respuesta.Datos = false;
                    respuesta.MensajePublico = respuestaInterna.Mensaje;
                    return BadRequest(respuesta);
                }
            }
            catch
            {
                respuesta.Datos = false;
                return StatusCode(StatusCodes.Status500InternalServerError, respuesta);
            }
        }
    }
}
