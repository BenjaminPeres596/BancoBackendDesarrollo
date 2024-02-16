using Core.DTO;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Servicios;

namespace BancoWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MotivoController : ControllerBase
    {
        private readonly ILogger<MotivoController> _logger;
        private readonly IMotivoServicio _motivoServicio;

        public MotivoController(IMotivoServicio motivoServicio, ILogger<MotivoController> logger)
        {
            _motivoServicio = motivoServicio;
            _logger = logger;
        }

        [HttpPost("Post")]
        public async Task<ActionResult<RespuestaExterna<bool>>> Post(TipoMotivo motivo)
        {
            var respuesta = new RespuestaExterna<bool>();
            var respuestaInterna = await _motivoServicio.Post(motivo);
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
        public async Task<ActionResult<RespuestaExterna<List<TipoMotivo>>>> Get()
        {
            var respuesta = new RespuestaExterna<List<TipoMotivo>>();
            var respuestaInterna = await _motivoServicio.Get();
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

        [HttpDelete("Delete={idMotivo}")]
        public async Task<ActionResult<RespuestaExterna<bool>>> Delete(int idMotivo)
        {
            var respuesta = new RespuestaExterna<bool>();
            var respuestaInterna = await _motivoServicio.Delete(idMotivo);
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
