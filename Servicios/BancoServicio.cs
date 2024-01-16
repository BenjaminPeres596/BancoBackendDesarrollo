using Core.DTO;
using Data.Models;
using Data.Data;
using Microsoft.EntityFrameworkCore;

namespace Servicios
{
    public class BancoServicio : IBancoServicio
    {
        private BancoDBContext _bancoDBContext;
        public BancoServicio()
        {
            _bancoDBContext = new BancoDBContext();
        }

        public async Task<RespuestaInterna<bool>> AgregarAsync(Banco banco)
        {
            var respuesta = new RespuestaInterna<bool>();
            var bancoExiste = await _bancoDBContext.Banco.FirstOrDefaultAsync(x => x.RazonSocial == banco.RazonSocial);
            if (bancoExiste != null)
            {
                respuesta.Datos = false;
                respuesta.Mensaje = "El banco ya existe";
                return respuesta;
            }
            try
            {
                respuesta.Datos = true;
                respuesta.Mensaje = "Banco agregado correctamente";
                respuesta.Exito = true;
                await _bancoDBContext.Banco.AddAsync(banco);
                await _bancoDBContext.SaveChangesAsync();
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Datos = false;
                respuesta.Mensaje = "No se pudo agregar el banco. Detalles: " + ex.Message;
                return respuesta;
            }
        }

        public async Task<RespuestaInterna<List<Banco>>> ObtenerAsync()
        {
            var respuesta = new RespuestaInterna<List<Banco>>();
            try
            {
                var bancos = await _bancoDBContext.Banco.ToListAsync();
                respuesta.Datos = bancos;
                respuesta.Exito = true;
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = "No se pudo recuperar los bancos. Detalles: " + ex.Message;
                return respuesta;
            }

        }

        public async Task<RespuestaInterna<bool>> EliminarAsync(string razonSocial)
        {
            var respuesta = new RespuestaInterna<bool>();
            var bancoExiste = await _bancoDBContext.Banco.FirstOrDefaultAsync(x => x.RazonSocial == razonSocial);
            if (bancoExiste == null)
            {
                respuesta.Datos = false;
                respuesta.Mensaje = "El banco no existe";
                return respuesta;
            }
            try
            {
                _bancoDBContext.Remove(bancoExiste);
                await _bancoDBContext.SaveChangesAsync();
                respuesta.Datos = true;
                respuesta.Exito = true;
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = "No se pudo eliminar el banco. Detalles: " + ex.Message;
                return respuesta;
            }
        }
    }
    
}