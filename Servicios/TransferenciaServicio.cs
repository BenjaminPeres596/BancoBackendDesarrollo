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
    public class TransferenciaServicio : ITransferenciaServicio
    {
        private BancoDBContext _bancoDBContext;
        public TransferenciaServicio()
        {
            _bancoDBContext = new BancoDBContext();
        }

        public async Task<RespuestaInterna<bool>> Post(Transferencia transferencia, string CbuOrigen, string CbuDestino)
        {
            var respuesta = new RespuestaInterna<bool>();
            try
            {
                var cuentaOrigenExiste = await _bancoDBContext.Cuenta.FirstOrDefaultAsync(c => c.Cbu == CbuOrigen);
                var cuentaDestinoExiste = await _bancoDBContext.Cuenta.FirstOrDefaultAsync(c => c.Cbu == CbuDestino);
                if (cuentaOrigenExiste == null)
                {
                    respuesta.Datos = false;
                    respuesta.Mensaje = "No existe la cuenta origen";
                    return respuesta;
                }
                else if (cuentaDestinoExiste == null)
                {
                    respuesta.Datos = false;
                    respuesta.Mensaje = "No existe la cuenta destino";
                    return respuesta;
                }
                else if (cuentaOrigenExiste != null && (cuentaOrigenExiste.Saldo - transferencia.Monto) < 0)
                {
                    respuesta.Datos = false;
                    respuesta.Mensaje = "El monto de la cuenta no es suficiente para realizar la transferencia";
                    return respuesta;
                }
                else
                {
                    respuesta.Datos = true;
                    respuesta.Exito = true;
                    transferencia.CuentaDestinoId = cuentaDestinoExiste.Id;
                    transferencia.CuentaOrigenId = cuentaOrigenExiste.Id;
                    respuesta.Mensaje = "Transferencia exitosa";
                    transferencia.CuentaDestino = cuentaDestinoExiste;
                    transferencia.CuentaOrigen = cuentaOrigenExiste;
                    cuentaOrigenExiste.Saldo = cuentaOrigenExiste.Saldo - transferencia.Monto;
                    cuentaDestinoExiste.Saldo = cuentaDestinoExiste.Saldo + transferencia.Monto;
                    await _bancoDBContext.Transferencia.AddAsync(transferencia);
                    await _bancoDBContext.SaveChangesAsync();
                    return respuesta;
                }
            }
            catch (Exception ex)
            {
                respuesta.Datos = false;
                respuesta.Exito = false;
                respuesta.Mensaje = "No se pudo realizar la transferencia. Detalles: " + ex.Message;
                return respuesta;
            }
        }

        public async Task<RespuestaInterna<List<Transferencia>>> Get(int id)
        {
            var respuesta = new RespuestaInterna<List<Transferencia>>();
            try
            {
                var cuentaExiste = await _bancoDBContext.Cuenta.FirstOrDefaultAsync(x => x.Id == id);
                if (cuentaExiste == null)
                {
                    respuesta.Mensaje = "No existe la cuenta.";
                    return respuesta;
                }
                else
                {
                    var transferencias = await _bancoDBContext.Transferencia.Include(x => x.CuentaOrigen).Include(x => x.CuentaDestino).Where(x => x.CuentaOrigen.Id == id || x.CuentaDestino.Id == id).ToListAsync();
                    respuesta.Datos = transferencias;
                    respuesta.Exito = true;
                    respuesta.Mensaje = "Transferencias recuperadas correctamente";
                    return respuesta;
                }
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = "No se pudo recuperar las transferencias. Detalles: " + ex.Message;
                return respuesta;
            }
        }
    }
}
