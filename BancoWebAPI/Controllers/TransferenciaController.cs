using Core.DTO;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Servicios;

namespace BancoWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransferenciaController : ControllerBase
    {
        private readonly ILogger<TransferenciaController> _logger;
        private readonly ITransferenciaServicio _transferenciaServicio;

        public TransferenciaController(ITransferenciaServicio transferenciaServicio, ILogger<TransferenciaController> logger)
        {
            _transferenciaServicio = transferenciaServicio;
            _logger = logger;
        }

        [HttpPost(Name = "PostTransferencias")]
        public async Task<ActionResult<RespuestaExterna<bool>>> Post(Transferencia transferencia, int nroCuentaOrigen, int nroCuentaDestino)
        {
            var respuesta = new RespuestaExterna<bool>();
            var respuestaInterna = await _transferenciaServicio.Post(transferencia, nroCuentaOrigen, nroCuentaDestino);
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

        [HttpGet(Name = "GetTransferencias")]
        public async Task<ActionResult<RespuestaExterna<List<Transferencia>>>> Get(int nroCuenta)
        {
            var respuesta = new RespuestaExterna<List<Transferencia>>();
            var respuestaInterna = await _transferenciaServicio.Get(nroCuenta);
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
    }
}
