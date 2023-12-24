using Core.DTO;
using Data.Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class CuentaServicio : ICuentaServicio
    {
        private BancoDBContext _bancoDBContext;
        public CuentaServicio()
        {
            _bancoDBContext = new BancoDBContext();
        }

        public async Task<RespuestaInterna<bool>> CrearCuentaAsync(Cuenta cuenta, int cuit)
        {
            var respuesta = new RespuestaInterna<bool>();
            try
            {
                var clienteexiste = await _bancoDBContext.Cliente.FirstOrDefaultAsync(c => c.Cuit == cuit);
                var tipocuentaexiste = await _bancoDBContext.TipoCuenta.FirstOrDefaultAsync(tc => tc.Id == cuenta.TipoCuentaId);
                if (tipocuentaexiste == null)
                {
                    respuesta.Datos = false;
                    respuesta.Exito = false;
                    respuesta.Mensaje = "No existe el tipo de cuenta";
                    return respuesta;
                }
                if (clienteexiste != null)
                {
                    respuesta.Datos = true;
                    respuesta.Exito = true;
                    var cantcuentas = await _bancoDBContext.Cuenta.CountAsync(c => c.Cliente.Cuit == cuit);
                    cuenta.FechaAlta = DateTime.Today.ToString("yyyy-MM-dd");
                    if (cantcuentas == 0)
                    {
                        cuenta.Saldo = 100000;
                    }
                    await _bancoDBContext.Cuenta.AddAsync(cuenta);
                    await _bancoDBContext.SaveChangesAsync();
                    return respuesta;
                }
                else
                {
                    respuesta.Mensaje = "El cliente no existe";
                    respuesta.Datos = false;
                    respuesta.Exito = false;
                    return respuesta;
                }
            }
            catch (Exception ex)
            {
                respuesta.Exito = false;
                respuesta.Datos = false;
                respuesta.Mensaje = ex.Message;
                return respuesta;
            }
        }
    }
}
