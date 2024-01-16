﻿using Microsoft.AspNetCore.Mvc;
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
    public class BancoController : ControllerBase
    {
        private readonly ILogger<BancoController> _logger;
        private readonly IBancoServicio _bancoServicio;

        public BancoController(IBancoServicio bancoServicio, ILogger<BancoController> logger)
        {
            _bancoServicio = bancoServicio;
            _logger = logger;
        }

        [HttpPost(Name = "PostBanco")]
        public async Task<ActionResult<RespuestaExterna<bool>>> Post(Banco banco)
        {
            var respuesta = new RespuestaExterna<bool>();
            var respuestaInterna = await _bancoServicio.AgregarAsync(banco);
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
                        respuesta.MensajePublico = "Intente colocar otra RazonSocial.";
                        return BadRequest(respuesta);
                }
            }
            catch
            {
                respuesta.MensajePublico = respuestaInterna.Mensaje;
                return StatusCode(StatusCodes.Status500InternalServerError, respuesta);
            }
        }

        [HttpGet(Name = "GetBancos")]
        public async Task<ActionResult<RespuestaExterna<List<Banco>>>> Get()
        {
            var respuesta = new RespuestaExterna<List<Banco>>();
            var respuestaInterna = await _bancoServicio.ObtenerAsync();
            try
            {
                if (respuestaInterna.Exito)
                {
                    respuesta.Exito = true;
                    respuesta.MensajePublico = "Bancos recuperados correctamente";
                    respuesta.Datos = respuestaInterna.Datos;
                    return respuesta;
                }
                else
                {
                    respuesta.MensajePublico = "Hubo un error al recuperar los bancos";
                    return BadRequest(respuesta);
                }
            }
            catch
            {
                respuesta.MensajePublico = respuestaInterna.Mensaje;
                return StatusCode(StatusCodes.Status500InternalServerError, respuesta);
            }
        }

        [HttpDelete("{razonSocial}", Name = "EliminarBanco")]
        public async Task<ActionResult<RespuestaExterna<bool>>> Delete(string razonSocial)
        {
            var respuesta = new RespuestaExterna<bool>();
            var respuestaInterna = await _bancoServicio.EliminarAsync(razonSocial);
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
                    respuesta.MensajePublico = "Intente con otra razon social.";
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
