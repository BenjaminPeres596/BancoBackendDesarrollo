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
            try
            {
                var respuestaInterna = await _clienteServicio.AgregarAsync(cliente);
                if (respuestaInterna.Exito)
                {
                    respuesta.Exito = true;
                    respuesta.Datos = true;
                    return StatusCode(StatusCodes.Status201Created, respuesta);
                }
                else
                {
                    respuesta.MensajePublico = "Intente colocar otro CUIT.";
                    return BadRequest(respuesta);
                }
            }
            catch (Exception ex)
            {
                respuesta.MensajePublico = "Ocurrio un error al agregar al Cliente.";
                return StatusCode(StatusCodes.Status500InternalServerError, respuesta);
            }
        }

        [HttpGet(Name = "GetClientes")]
        public async Task<ActionResult<RespuestaExterna<List<Cliente>>>> Get()
        {
            var respuesta = new RespuestaExterna<List<Cliente>>();
            try
            {
                var respuestaInterna = await _clienteServicio.ObtenerAsync();
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
            catch (Exception ex)
            {
                respuesta.MensajePublico = "Hubo un error al recuperar los clientes";
                return StatusCode(StatusCodes.Status500InternalServerError, respuesta);
            }
        }

        [HttpGet(Name = "GetClientePorCuit")]
        public async Task<ActionResult<RespuestaExterna<Cliente>>> Get(int cuit)
        {
            var respuesta = new RespuestaExterna<Cliente>();
            try
            {
                if (RespuestaInterna.Exito)
            }
            
        }

        [HttpDelete("{cuit}", Name = "EliminarCliente")]
        public async Task<ActionResult<RespuestaExterna<bool>>> Delete(int cuit)
        {
            var respuesta = new RespuestaExterna<bool>();
            try
            {
                var respuestaInterna = await _clienteServicio.EliminarAsync(cuit);
                if (respuestaInterna.Exito)
                {
                    respuesta.Datos = true;
                    respuesta.Exito = true;
                    return StatusCode(StatusCodes.Status204NoContent, respuesta);
                }
                else
                {
                    respuesta.MensajePublico = "Intente con otro CUIT.";
                    return BadRequest(respuesta);
                }
            }
            catch (Exception ex)
            {
                respuesta.MensajePublico = "Ocurrio un error al eliminar el cliente";
                return StatusCode(StatusCodes.Status500InternalServerError, respuesta);
            }
        }
    }
}