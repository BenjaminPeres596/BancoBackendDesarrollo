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
            if (clienteExiste != null)
            {
                respuesta.Mensaje = "El cliente ya existe";
                return respuesta;
            }

            try
            {
                await _bancoDBContext.Cliente.AddAsync(cliente);
                await _bancoDBContext.SaveChangesAsync();
                respuesta.Exito = true;
                respuesta.Datos = true;
                return respuesta;
            }
            catch
            {
                return respuesta;
            }
        }

        public async Task<RespuestaInterna<List<Cliente>>> ObtenerAsync()
        {
            var respuesta = new RespuestaInterna<List<Cliente>>();
            try
            {
                var clientes = await _bancoDBContext.Cliente.ToListAsync();
                respuesta.Datos = clientes;
                respuesta.Exito = true;
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
                return respuesta;
            }

        }

    }
}
