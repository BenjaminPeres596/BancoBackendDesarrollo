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
        public async Task<ActionResult<RespuestaExterna<bool>>> Post(Cuenta cuenta, int cuit)
        {
            var respuesta = new RespuestaExterna<bool>();
            try
            {
                var respuestaInterna = await _cuentaServicio.CrearCuentaAsync(cuenta, cuit);
                respuesta.MensajePublico = respuestaInterna.Mensaje;
                respuesta.Datos = respuestaInterna.Datos;
                respuesta.Exito = respuestaInterna.Exito;
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.MensajePublico = "Ocurrio un error al agregar la cuenta.";
                return StatusCode(StatusCodes.Status500InternalServerError, respuesta);
            }
        }
    }
}
