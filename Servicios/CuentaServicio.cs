﻿using Core.DTO;
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

        public async Task<RespuestaInterna<bool>> CrearCuentaAsync(Cuenta cuenta, int dni)
        {
            var respuesta = new RespuestaInterna<bool>();
            try
            {
                var clienteexiste = await _bancoDBContext.Cliente.FirstOrDefaultAsync(c => c.Cuil == dni);
                var tipocuentaexiste = await _bancoDBContext.TipoCuenta.FirstOrDefaultAsync(t => t.Id == cuenta.TipoCuentaId);
                if (tipocuentaexiste == null)
                {
                    respuesta.Datos = false;
                    respuesta.Mensaje = "No existe el tipo de cuenta";
                    return respuesta;
                }
                if (clienteexiste != null)
                {
                    respuesta.Datos = true;
                    respuesta.Exito = true;
                    var cantcuentas = await _bancoDBContext.Cuenta.CountAsync(c => c.Cliente.Cuil == dni);
                    var ultimacuenta = await _bancoDBContext.Cuenta.OrderByDescending(c => c.Cbu).FirstOrDefaultAsync();
                    if (ultimacuenta != null)
                    {
                        var nuevoNumeroCbu = long.Parse(ultimacuenta.Cbu.Substring(10)) + 1;
                        cuenta.Cbu = $"0000000003{nuevoNumeroCbu:D12}";

                    }
                    else
                    {
                        cuenta.Cbu = "0000000003000000000001";
                    }
                    cuenta.FechaAlta = DateTime.Today.ToString("yyyy-MM-dd");
                    if (cantcuentas == 0)
                    {
                        cuenta.Saldo = 100000;
                    }
                    cuenta.ClienteId = clienteexiste.Id;
                    cuenta.TipoCuenta = tipocuentaexiste;
                    cuenta.Cliente = clienteexiste;
                    await _bancoDBContext.Cuenta.AddAsync(cuenta);
                    await _bancoDBContext.SaveChangesAsync();
                    return respuesta;
                }
                else
                {
                    respuesta.Mensaje = "El cliente no existe";
                    respuesta.Datos = false;
                    return respuesta;
                }
            }
            catch (Exception ex)
            {
                respuesta.Datos = false;
                respuesta.Mensaje = "No se pudo crear la cuenta. Detalles: " + ex.Message;
                return respuesta;
            }
        }
        public async Task<RespuestaInterna<List<Cuenta>>> ObtenerAsync()
        {
            var respuesta = new RespuestaInterna<List<Cuenta>>();
            try
            {
                var cuentas = await _bancoDBContext.Cuenta.ToListAsync();
                respuesta.Datos = cuentas;
                respuesta.Exito = true;
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = "No se pudo recuperar las cuentas. Detalles: " + ex.Message;
                return respuesta;
            }

        }

        public async Task<RespuestaInterna<List<Cuenta>>> ObtenerPorDniAsync(int dni)
        {
            var respuesta = new RespuestaInterna<List<Cuenta>>();
            var clienteexiste = await _bancoDBContext.Cliente.FirstOrDefaultAsync(c => c.Cuil == dni);
            if (clienteexiste == null)
            {
                respuesta.Mensaje = "No existe el cliente, intente con otro CUIT.";
                return respuesta;
            }
            try
            {
                var cuentas = await _bancoDBContext.Cuenta.Include(x => x.TipoCuenta).Include(x => x.Cliente).Where(x => x.Cliente.Cuil == dni).ToListAsync();
                respuesta.Datos = cuentas;
                respuesta.Exito = true;
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = "No se pudo recuperar las cuentas del cliente. Detalles: " + ex.Message;
                return respuesta;
            }

        }

        public async Task<RespuestaInterna<List<Cuenta>>> ObtenerPorCbuAsync(string cbu)
        {
            var respuesta = new RespuestaInterna<List<Cuenta>>();
            var cuentaexiste = await _bancoDBContext.Cuenta.FirstOrDefaultAsync(c => c.Cbu == cbu);
            if (cuentaexiste == null)
            {
                respuesta.Mensaje = "No existe la cuenta, intente con otro CBU.";
                return respuesta;
            }
            try
            {
                var cuentas = await _bancoDBContext.Cuenta.Include(x => x.TipoCuenta).Include(x => x.Cliente).Where(x => x.Cbu == cbu).ToListAsync();
                respuesta.Datos = cuentas;
                respuesta.Exito = true;
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = "No se pudo recuperar las cuentas del cliente. Detalles: " + ex.Message;
                return respuesta;
            }

        }

        public async Task<RespuestaInterna<bool>> EliminarAsync(int id, int dni)
        {
            var respuesta = new RespuestaInterna<bool>();
            var clienteExiste = await _bancoDBContext.Cliente.FirstOrDefaultAsync(x => x.Cuil == dni);
            if (clienteExiste == null)
            {
                respuesta.Datos = false;
                respuesta.Mensaje = "El cliente no existe, intente con otro DNI.";
                return respuesta;
            }
            var cuentas = await _bancoDBContext.Cuenta.Where(x => x.Cliente.Cuil == dni).ToListAsync();
            var cuentaCliente = cuentas.FirstOrDefault(x => x.Id == id);
            if (cuentaCliente == null)
            {
                respuesta.Datos = false;
                respuesta.Mensaje = "La cuenta no existe";
                return respuesta;
            }
            if (cuentaCliente.Id != id)
            {
                respuesta.Datos = false;
                respuesta.Mensaje = "No se encontro la cuenta del cliente";
                return respuesta;
            }
            try
            {
                _bancoDBContext.Remove(cuentaCliente);
                await _bancoDBContext.SaveChangesAsync();
                respuesta.Datos = true;
                respuesta.Exito = true;
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = "No se pudo eliminar la cuenta. Detalles: " + ex.Message;
                return respuesta;
            }
        }
    }
}
