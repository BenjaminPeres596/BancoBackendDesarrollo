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

        [HttpPost("{cbuOrigen},{cbuDestino},{monto}.{motivoId}",Name = "PostTransferencias")]
        public async Task<ActionResult<RespuestaExterna<bool>>> Post(Transferencia transferencia, string cbuOrigen, string cbuDestino, float monto, int motivoId)
        {
            var respuesta = new RespuestaExterna<bool>();
            var respuestaInterna = await _transferenciaServicio.Post(transferencia, cbuOrigen, cbuDestino, monto, motivoId);
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

        [HttpGet("{idCuenta}", Name = "GetTransferencias")]
        public async Task<ActionResult<RespuestaExterna<List<Transferencia>>>> Get(int idCuenta)
        {
            var respuesta = new RespuestaExterna<List<Transferencia>>();
            var respuestaInterna = await _transferenciaServicio.Get(idCuenta);
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
