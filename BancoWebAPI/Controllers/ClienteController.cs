using Microsoft.AspNetCore.Mvc;
using Data.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Data.Data;
using Microsoft.EntityFrameworkCore;
using Servicios;
using Core.DTO;

namespace TP3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly ILogger<ClienteController> _logger;
        private readonly IClienteServicio _clienteServicio;

        public ClienteController(IClienteServicio clienteServicio, ILogger<ClienteController> logger)
        {
            _clienteServicio = clienteServicio;
            _logger = logger;
        }

        [HttpPost(Name = "PostCliente")]
        public async Task<ActionResult<RespuestaExterna<bool>>> Post(Cliente cliente)
        {
            var respuesta = new RespuestaExterna<bool>();
            var respuestaInterna = await _clienteServicio.AgregarAsync(cliente);
            try
            {
                if (respuestaInterna.Exito)
                {
                    respuesta.Exito = true;
                    respuesta.Datos = true;
                    return StatusCode(StatusCodes.Status201Created, respuesta);
                }
                else
                {
                    if (respuestaInterna.Mensaje.Contains("cliente"))
                    {
                        respuesta.MensajePublico = "Intente colocar otro CUIT.";
                        return BadRequest(respuesta);
                    }
                    else
                    {
                        respuesta.MensajePublico = "Intente colocar otro Banco.";
                        return BadRequest(respuesta);
                    }
                }
            }
            catch
            {
                respuesta.MensajePublico = respuestaInterna.Mensaje;
                return StatusCode(StatusCodes.Status500InternalServerError, respuesta);
            }
        }

        [HttpGet(Name = "GetClientes")]
        public async Task<ActionResult<RespuestaExterna<List<Cliente>>>> Get()
        {
            var respuesta = new RespuestaExterna<List<Cliente>>();
            var respuestaInterna = await _clienteServicio.ObtenerAsync();
            try
            {
                if (respuestaInterna.Exito)
                {
                    respuesta.Exito = true;
                    respuesta.MensajePublico = "Clientes recuperados correctamente";
                    respuesta.Datos = respuestaInterna.Datos;
                    return respuesta;
                }
                else
                {
                    respuesta.MensajePublico = "Hubo un error al recuperar los clientes";
                    return BadRequest(respuesta);
                }
            }
            catch
            {
                respuesta.MensajePublico = respuestaInterna.Mensaje;
                return StatusCode(StatusCodes.Status500InternalServerError, respuesta);
            }
        }

        [HttpGet("{cuit}", Name = "GetClientePorCuit")]
        public async Task<ActionResult<RespuestaExterna<Cliente>>> Get(int cuit)
        {
            var respuesta = new RespuestaExterna<Cliente>();
            var respuestaInterna = await _clienteServicio.ObtenerPorCuitAsync(cuit);
            try
            {
                if (respuestaInterna.Exito)
                {
                    respuesta.Exito = true;
                    respuesta.Datos = respuestaInterna.Datos;
                    respuesta.MensajePublico = "Cliente recuperado correctamente";
                    return respuesta;
                }
                else
                {
                    respuesta.MensajePublico = "CUIT incorrecto";
                    return BadRequest(respuesta);
                }
            }
            catch
            {
                respuesta.MensajePublico = respuestaInterna.Mensaje;
                return StatusCode(StatusCodes.Status500InternalServerError, respuesta);
            }
        }

        [HttpDelete("{cuit}", Name = "EliminarCliente")]
        public async Task<ActionResult<RespuestaExterna<bool>>> Delete(int cuit)
        {
            var respuesta = new RespuestaExterna<bool>();
            var respuestaInterna = await _clienteServicio.EliminarAsync(cuit);
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
                    respuesta.MensajePublico = "Intente con otro CUIT.";
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