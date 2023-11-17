using Data.Models;
using Core.DTO;

namespace Servicios
{
    public interface IClienteServicio
    {
        Task<RespuestaInterna<bool>> AgregarAsync(Cliente cliente);
        Task<RespuestaInterna<List<Cliente>>> ObtenerAsync();
        Task<RespuestaInterna<Cliente>> ObtenerPorCuitAsync(int cuit);
        Task<RespuestaInterna<bool>> EliminarAsync(int cuit);
    }
}
