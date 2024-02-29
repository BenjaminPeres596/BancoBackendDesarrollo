using Core.DTO;
using Data.Data;
using Microsoft.EntityFrameworkCore;
using Data.Models;
using RestSharp;
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

        //public async Task<RespuestaInterna<bool>> Post(Transferencia transferencia, string CbuOrigen, string CbuDestino, float monto, string motivoNombre)
        //{
        //    var estado = "No realizada";
        //    var respuesta = new RespuestaInterna<bool>();
        //    try
        //    {
        //        if (CbuOrigen.Substring(0, 10) == "0000000003") { 
        //            var options = new RestClientOptions("https://colosal.duckdns.org:15001/WebApi/Transaccion");
        //            var client = new RestClient();
        //            var JSONTransferencia = new TransferenciaJSON
        //            {
        //                origin_cbu = CbuOrigen,
        //                amount = monto,
        //                destination_cbu = CbuDestino,
        //                motive = motivoNombre, //Hay que buscar forma de comparar con nuestra BD, el id con el nombre del motivo para ponerlo aca
        //                origin_cuil = (int)Convert.ToInt32(transferencia.CuentaOrigen.Cliente.Cuil),
        //                destination_cuil = (int)Convert.ToInt32(transferencia.CuentaDestino.Cliente.Cuil)
        //            };
        //            var request = new RestRequest("https://colosal.duckdns.org:15001/WebApi/Transaccion").AddJsonBody(JSONTransferencia);
        //            var responseBroker = await client.PostAsync<RespuestaInterna<string>>(request);

        //            if (responseBroker.estado == "Validada")
        //            {
        //                if (CbuDestino.Substring(0, 10) == "0000000003")
        //                {
        //                    var cuentaOrigenExiste = await _bancoDBContext.Cuenta.FirstOrDefaultAsync(c => c.Cbu == CbuOrigen);
        //                    var cuentaDestinoExiste = await _bancoDBContext.Cuenta.FirstOrDefaultAsync(c => c.Cbu == CbuDestino);
        //                    var motivoExiste = await _bancoDBContext.TipoMotivo.FirstOrDefaultAsync(m => m.Nombre == motivoNombre); //revisacion de esto
        //                    if (cuentaOrigenExiste == null)
        //                    {
        //                        respuesta.Datos = false;
        //                        respuesta.Mensaje = "No existe la cuenta origen";
        //                        return respuesta;
        //                    }
        //                    else if (cuentaDestinoExiste == null)
        //                    {
        //                        respuesta.Datos = false;
        //                        respuesta.Mensaje = "No existe la cuenta destino";
        //                        return respuesta;
        //                    }
        //                    else if (cuentaOrigenExiste != null && (cuentaOrigenExiste.Saldo - monto) < 0)
        //                    {
        //                        respuesta.Datos = false;
        //                        respuesta.Mensaje = "El monto de la cuenta no es suficiente para realizar la transferencia";
        //                        return respuesta;
        //                    }
        //                    else if (motivoExiste == null)
        //                    {
        //                        respuesta.Datos = false;
        //                        respuesta.Mensaje = "No existe el motivo";
        //                        return respuesta;
        //                    }
        //                    else if (monto <= 0)
        //                    {
        //                        respuesta.Datos = false;
        //                        respuesta.Mensaje = "No se puede tener un monto negativo";
        //                        return respuesta;
        //                    }
        //                    else
        //                    {
        //                        transferencia.CuentaDestinoId = cuentaDestinoExiste.Id;
        //                        transferencia.CuentaOrigenId = cuentaOrigenExiste.Id;
        //                        transferencia.CuentaDestino = cuentaDestinoExiste;
        //                        transferencia.CuentaOrigen = cuentaOrigenExiste;
        //                        transferencia.TipoMotivo = motivoExiste;
        //                        transferencia.Monto = long.Parse(monto.ToString());
        //                        cuentaOrigenExiste.Saldo = cuentaOrigenExiste.Saldo - monto;
        //                        cuentaDestinoExiste.Saldo = cuentaDestinoExiste.Saldo + monto;
        //                        respuesta.Datos = true;
        //                        respuesta.Exito = true;
        //                        respuesta.Mensaje = "Transferencia exitosa";
        //                        estado = "Realizada";
        //                        await _bancoDBContext.Transferencia.AddAsync(transferencia);
        //                        await _bancoDBContext.SaveChangesAsync();
        //                        return respuesta;
        //                    }
        //                }
        //                options = new RestClientOptions("https://colosal.duckdns.org:15001/WebApi/Transaccion/Confirmacion");
        //                client = new RestClient();
        //                var ConfirmacionJSON = new ConfirmacionJSON
        //                {
        //                    NumeroTrans = responseBroker.Numero,
        //                    Estado = estado
        //                };
        //                request = new RestRequest("https://colosal.duckdns.org:15001/WebApi/Transaccion/Confirmacion").AddJsonBody(estado);
        //                var reponseBrokerConfirmacion = await client.PostAsync<RespuestaInterna<string>>(request);
        //            }
        //        }
        //        else
        //        {

        //        }
        //    }                                           
        //    catch (Exception ex)
        //    {
        //        respuesta.Datos = false;
        //        respuesta.Exito = false;
        //        respuesta.Mensaje = "No se pudo realizar la transferencia. Detalles: " + ex.Message;
        //        return respuesta;
        //    }
        //}

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
