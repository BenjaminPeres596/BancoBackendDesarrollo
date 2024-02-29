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

        [HttpPost("Post")]
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

        [HttpPost("LoginAuth={cuil},{usuario},{contraseña},{authCode}")]
        public async Task<ActionResult<RespuestaExterna<Cliente>>> PostLogin(long cuil, string usuario, string contraseña, string authCode)
        {
            var respuesta = new RespuestaExterna<Cliente>();
            var respuestaInterna = await _clienteServicio.LoginAuth(cuil, usuario, contraseña, authCode);
            try
            {
                if (respuestaInterna.Exito)
                {
                    respuesta.Exito = true;
                    respuesta.MensajePublico = respuestaInterna.Mensaje;
                    respuesta.Datos = respuestaInterna.Datos;
                    return respuesta;
                }
                else
                {
                    respuesta.MensajePublico = respuestaInterna.Mensaje;
                    return respuesta;
                }
            }
            catch (Exception ex)
            {
                respuesta.MensajePublico = respuestaInterna.Mensaje;
                return StatusCode(StatusCodes.Status500InternalServerError, respuesta);
            }
        }

        [HttpGet("Get")]
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

        [HttpGet("GetPorCuil={cuil}")]
        public async Task<ActionResult<RespuestaExterna<Cliente>>> Get(long cuil)
        {
            var respuesta = new RespuestaExterna<Cliente>();
            var respuestaInterna = await _clienteServicio.ObtenerPorCuilAsync(cuil);
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

        [HttpDelete("DeleteCliente={cuil}")]
        public async Task<ActionResult<RespuestaExterna<bool>>> Delete(long cuil)
        {
            var respuesta = new RespuestaExterna<bool>();
            var respuestaInterna = await _clienteServicio.EliminarAsync(cuil);
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

        [HttpPost("AuthRenaper={authCode}")]
        public async Task<ActionResult<RespuestaExterna<ClienteData>>> AuthRenaper(string authCode)
        {
            var respuesta = new RespuestaExterna<ClienteData>();
            var respuestaInterna = await _clienteServicio.AuthRenaper(authCode);
            try
            {
                if (respuestaInterna.Exito)
                {
                    respuesta.Exito = true;
                    respuesta.MensajePublico = respuestaInterna.Mensaje;
                    respuesta.Datos = respuestaInterna.Datos;
                    return respuesta;
                }
                else
                {
                    respuesta.MensajePublico = respuestaInterna.Mensaje;
                    return respuesta;
                }
            }
            catch (Exception ex)
            {
                respuesta.MensajePublico = respuestaInterna.Mensaje;
                return StatusCode(StatusCodes.Status500InternalServerError, respuesta);
            }
        }
    }
}