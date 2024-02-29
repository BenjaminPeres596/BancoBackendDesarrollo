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
        private readonly ICuentaServicio _cuentaServicio;

        public TransferenciaServicio(ICuentaServicio cuentaServicio)
        {
            _cuentaServicio = cuentaServicio;
            _bancoDBContext = new BancoDBContext();
        }

        public async Task<RespuestaInterna<bool>> Post(Transferencia transferencia, string CbuOrigen, string CbuDestino, float monto, string motivoNombre)
        {
            var estado = "No realizada";
            var respuesta = new RespuestaInterna<bool>();
            try
            {
                //if (CbuOrigen.Substring(0, 10) == "0000000003")
                //{
                //    var options = new RestClientOptions("https://colosal.duckdns.org:15001/WebApi/Transaccion");
                //    var client = new RestClient();
                //    var JSONTransferencia = new TransferenciaJSON
                //    {
                //        origin_cbu = CbuOrigen,
                //        amount = monto,
                //        destination_cbu = CbuDestino,
                //        motive = motivoNombre, //Hay que buscar forma de comparar con nuestra BD, el id con el nombre del motivo para ponerlo aca
                //        origin_cuil = (int)Convert.ToInt32(transferencia.CuentaOrigen.Cliente.Cuil),
                //        destination_cuil = (int)Convert.ToInt32(transferencia.CuentaDestino.Cliente.Cuil)
                //    };
                //    var request = new RestRequest("https://colosal.duckdns.org:15001/WebApi/Transaccion").AddJsonBody(JSONTransferencia);
                //    var responseBroker = await client.PostAsync<RespuestaInterna<string>>(request);

                //    if (responseBroker.estado == "Validada")
                //    {
                //        if (CbuDestino.Substring(0, 10) == "0000000003")
                //        {
                            var cuentaOrigenExiste = await _bancoDBContext.Cuenta.FirstOrDefaultAsync(c => c.Cbu == CbuOrigen);
                            var cuentaDestinoExiste = await _bancoDBContext.Cuenta.FirstOrDefaultAsync(c => c.Cbu == CbuDestino);
                            var motivoExiste = await _bancoDBContext.TipoMotivo.FirstOrDefaultAsync(m => m.Nombre == motivoNombre);
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
                            else if (cuentaOrigenExiste != null && (cuentaOrigenExiste.Saldo - monto) < 0)
                            {
                                respuesta.Datos = false;
                                respuesta.Mensaje = "El monto de la cuenta no es suficiente para realizar la transferencia";
                                return respuesta;
                            }
                            else if (motivoExiste == null)
                            {
                                respuesta.Datos = false;
                                respuesta.Mensaje = "No existe el motivo";
                                return respuesta;
                            }
                            else if (monto <= 0)
                            {
                                respuesta.Datos = false;
                                respuesta.Mensaje = "No se puede tener un monto negativo";
                                return respuesta;
                            }
                            else
                            {
                                var ultimatransferencia = await _bancoDBContext.Transferencia.OrderByDescending(t => t.Id).FirstOrDefaultAsync();
                            
                                int numeroTransferencia;
                                if (ultimatransferencia != null)
                                {
                                numeroTransferencia = ultimatransferencia.NumeroTrans + 1;
                                }
                                else
                                {
                                numeroTransferencia = 1;
                                }
                                transferencia.NumeroTrans = numeroTransferencia;
                                transferencia.CuentaDestinoId = cuentaDestinoExiste.Id;
                                transferencia.CuentaOrigenId = cuentaOrigenExiste.Id;
                                transferencia.CuentaDestino = cuentaDestinoExiste;
                                transferencia.CuentaOrigen = cuentaOrigenExiste;
                                transferencia.TipoMotivo = motivoExiste;
                                transferencia.Monto = long.Parse(monto.ToString());
                                cuentaOrigenExiste.Saldo = cuentaOrigenExiste.Saldo - monto;
                                cuentaDestinoExiste.Saldo = cuentaDestinoExiste.Saldo + monto;
                                respuesta.Datos = true;
                                respuesta.Exito = true;
                                respuesta.Mensaje = "Transferencia exitosa";
                 //             estado = "Realizada";
                                await _bancoDBContext.Transferencia.AddAsync(transferencia);
                                await _bancoDBContext.SaveChangesAsync();
                                return respuesta;
                            }
                        }
                        //options = new RestClientOptions("https://colosal.duckdns.org:15001/WebApi/Transaccion/Confirmacion");
                        //client = new RestClient();
                        //var ConfirmacionJSON = new ConfirmacionJSON
                        //{
                        //    NumeroTrans = responseBroker.Numero,
                        //    Estado = estado
                        //};
                        //request = new RestRequest("https://colosal.duckdns.org:15001/WebApi/Transaccion/Confirmacion").AddJsonBody(estado);
                        //var reponseBrokerConfirmacion = await client.PostAsync<RespuestaInterna<string>>(request);
            //        }
            //    }
            //    else
            //    {

            //    }
            //}
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

        public async Task<RespuestaInterna<Transferencia>> PostTransferenciaExterna(TransferenciaJSON transferenciaExterna)
        {
            var respuesta = new RespuestaInterna<Transferencia>();
            try
            {
                if (transferenciaExterna.amount <= 0)
                {
                    respuesta.Mensaje = "Monto inválido, intente nuevamente.";
                    return respuesta;
                }

                var cuentaBancoExternoResponse = await _cuentaServicio.ObtenerPorCbuAsync(transferenciaExterna.origin_cbu);
                if (!cuentaBancoExternoResponse.Exito)
                {
                    respuesta.Mensaje = cuentaBancoExternoResponse.Mensaje;
                    return respuesta;
                }

                var cuentaBancoExterno = cuentaBancoExternoResponse.Datos;
                if (cuentaBancoExterno == null)
                {
                    var respuestaCrearCuenta = await _cuentaServicio.CrearCuentaExterna(transferenciaExterna.origin_cbu);
                    if (!respuestaCrearCuenta.Exito)
                    {
                        respuesta.Mensaje = respuestaCrearCuenta.Mensaje;
                        return respuesta;
                    }
                    cuentaBancoExterno = respuestaCrearCuenta.Datos;
                }

                var cuentaDestinoResponse = await _cuentaServicio.ObtenerPorCbuAsync(transferenciaExterna.destination_cbu);
                if (!cuentaDestinoResponse.Exito)
                {
                    respuesta.Mensaje = cuentaDestinoResponse.Mensaje;
                    return respuesta;
                }

                var cuentaDestino = cuentaDestinoResponse.Datos;
                if (cuentaDestino == null)
                {
                    respuesta.Mensaje = "La cuenta destino no existe";
                    return respuesta;
                }

                var motivo = await _bancoDBContext.TipoMotivo.FirstOrDefaultAsync(tm => tm.Nombre == transferenciaExterna.motive);
                if (motivo == null)
                {
                    respuesta.Mensaje = "El motivo no existe";
                    return respuesta;
                }

                var ultimaTransferencia = await _bancoDBContext.Transferencia.OrderByDescending(t => t.Id).FirstOrDefaultAsync();
                int numeroTransferencia = (ultimaTransferencia != null) ? ultimaTransferencia.NumeroTrans + 1 : 1;

                var transferencia = new Transferencia
                {
                    Monto = (long)transferenciaExterna.amount,
                    Fecha = DateTime.UtcNow,
                    CuentaOrigenId = cuentaBancoExterno.Id,
                    CuentaDestinoId = cuentaDestino.Id,
                    TipoMotivoId = motivo.Id,
                    NumeroTrans = numeroTransferencia
                };

                cuentaDestino.Saldo = cuentaDestino.Saldo + transferencia.Monto;

                respuesta.Datos = transferencia;
                respuesta.Exito = true;
                respuesta.Mensaje = "Transferencia exitosa";

                await _bancoDBContext.Transferencia.AddAsync(transferencia);
                await _bancoDBContext.SaveChangesAsync();

                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = "Error al realizar la transferencia. Detalles: " + ex.Message;
                respuesta.Exito = false;

                // Imprimir información adicional sobre la excepción interna
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Excepción interna: " + ex.InnerException.Message);
                }
                else
                {
                    Console.WriteLine("No hay excepción interna.");
                }

                return respuesta;
            }
        }
    }
}
