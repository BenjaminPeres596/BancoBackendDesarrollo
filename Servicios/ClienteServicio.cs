using Core.DTO;
using Data.Models;
using Data.Data;
using Microsoft.EntityFrameworkCore;

namespace Servicios
{
    public class ClienteServicio : IClienteServicio
    {
        private BancoDBContext _bancoDBContext;
        public ClienteServicio()
        {
            _bancoDBContext = new BancoDBContext();
        }
        public async Task<RespuestaInterna<bool>> AgregarAsync(Cliente cliente)
        {
            var respuesta = new RespuestaInterna<bool>();
            var clienteExiste = await _bancoDBContext.Cliente.FirstOrDefaultAsync(x => x.Cuit == cliente.Cuit);
            var bancoExiste = await _bancoDBContext.Banco.FirstOrDefaultAsync(x => x.Id == cliente.BancoId);
            if (clienteExiste != null)
            {
                respuesta.Datos = false;
                respuesta.Mensaje = "El cliente ya existe";
                return respuesta;
            }
            if (bancoExiste == null)
            {
                respuesta.Datos = false;
                respuesta.Mensaje = "El banco no existe";
                return respuesta;
            }
            
            try
            {
                cliente.Banco = bancoExiste;
                await _bancoDBContext.Cliente.AddAsync(cliente);
                await _bancoDBContext.SaveChangesAsync();
                respuesta.Exito = true;
                respuesta.Datos = true;
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Datos = false;
                respuesta.Mensaje = "No se pudo agregar al cliente. Detalles: " + ex.Message;
                return respuesta;
            }
        }

        public async Task<RespuestaInterna<List<Cliente>>> ObtenerAsync()
        {
            var respuesta = new RespuestaInterna<List<Cliente>>();
            try
            {
                var clientes = await _bancoDBContext.Cliente.Include(x => x.Banco).ToListAsync();
                respuesta.Datos = clientes;
                respuesta.Exito = true;
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = "No se pudo recuperar los clientes. Detalles: " + ex.Message;
                return respuesta;
            }

        }

        public async Task<RespuestaInterna<Cliente>> ObtenerPorCuitAsync(int cuit)
        {
            var respuesta = new RespuestaInterna<Cliente>();
            var clienteExiste = await _bancoDBContext.Cliente.FirstOrDefaultAsync(x => x.Cuit == cuit);
            if (clienteExiste == null)
            {
                respuesta.Mensaje = "El cliente no existe";
                return respuesta;
            }
            try
            {
                respuesta.Datos = clienteExiste;
                respuesta.Exito = true;
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = "No se pudo obtener el cliente. Detalles: " + ex.Message;
                return respuesta;
            }
        }

        public async Task<RespuestaInterna<bool>> EliminarAsync(int cuit)
        {
            var respuesta = new RespuestaInterna<bool>();
            var clienteExiste = await _bancoDBContext.Cliente.FirstOrDefaultAsync(x => x.Cuit == cuit);
            if (clienteExiste == null)
            {
                respuesta.Datos = false;
                respuesta.Mensaje = "El cliente no existe";
                return respuesta;
            }
            try
            {
                _bancoDBContext.Remove(clienteExiste);
                await _bancoDBContext.SaveChangesAsync();
                respuesta.Datos = true;
                respuesta.Exito = true;
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = "No se pudo eliminar al cliente. Detalles: " + ex.Message;
                return respuesta;
            }
        }
    }
}
