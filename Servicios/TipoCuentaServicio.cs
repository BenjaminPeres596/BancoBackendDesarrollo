using Core.DTO;
using Data.Data;
using Microsoft.EntityFrameworkCore;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class TipoCuentaServicio : ITipoCuentaServicio
    {
        private BancoDBContext _bancoDBContext;
        public TipoCuentaServicio()
        {
            _bancoDBContext = new BancoDBContext();
        }

        public async Task<RespuestaInterna<bool>> Post(TipoCuenta tipocuenta)
        {
            var respuesta = new RespuestaInterna<bool>();
            try
            {
                var tipocuentaexiste = await _bancoDBContext.TipoCuenta.FirstOrDefaultAsync(tc => tc.Nombre == tipocuenta.Nombre);
                if (tipocuentaexiste != null)
                {
                    respuesta.Datos = false;
                    respuesta.Mensaje = "Ya existe el tipo de cuenta";
                    return respuesta;
                }
                else
                {
                    respuesta.Datos = true;
                    respuesta.Exito = true;
                    respuesta.Mensaje = "Tipo cuenta creado exitosamente";
                    await _bancoDBContext.TipoCuenta.AddAsync(tipocuenta);
                    await _bancoDBContext.SaveChangesAsync();
                    return respuesta;
                }
            }
            catch (Exception ex)
            {
                respuesta.Datos = false;
                respuesta.Exito = false;
                respuesta.Mensaje = ex.Message;
                return respuesta;
            }
        }

        public async Task<RespuestaInterna<bool>> Delete(int id)
        {
            var respuesta = new RespuestaInterna<bool>();
            try
            {
                var tipocuentaexiste = await _bancoDBContext.TipoCuenta.FirstOrDefaultAsync(t => t.Id == id);
                if (tipocuentaexiste == null)
                {
                    respuesta.Datos = false;
                    respuesta.Mensaje = "No existe el tipo de cuenta";
                    return respuesta;
                }
                else
                {
                    respuesta.Datos = true;
                    respuesta.Exito = true;
                    respuesta.Mensaje = "Tipo cuenta eliminado correctamente";
                    _bancoDBContext.TipoCuenta.Remove(tipocuentaexiste);
                    await _bancoDBContext.SaveChangesAsync();
                    return respuesta;
                }
            }
            catch (Exception ex)
            {
                respuesta.Datos = false;
                respuesta.Mensaje = "No se pudo eliminar al cliente. Detalles: " + ex.Message;
                return respuesta;
            }
        }

        public async Task<RespuestaInterna<List<TipoCuenta>>> Get()
        {
            var respuesta = new RespuestaInterna<List<TipoCuenta>>();
            try
            {
                var tipoCuentas = await _bancoDBContext.TipoCuenta.ToListAsync();
                respuesta.Datos = tipoCuentas;
                respuesta.Exito = true;
                respuesta.Mensaje = "Tipo cuentas recuperados correctamente";
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = "No se pudo recuperar a los tipos cuentas. Detalles: " + ex.Message;
                return respuesta;
            }
        }
    }
}
