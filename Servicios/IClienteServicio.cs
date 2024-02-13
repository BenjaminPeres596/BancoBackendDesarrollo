using Data.Models;
using Core.DTO;

namespace Servicios
{
    public interface IClienteServicio
    {
        Task<RespuestaInterna<bool>> AgregarAsync(Cliente cliente);
        Task<RespuestaInterna<List<Cliente>>> ObtenerAsync();
        Task<RespuestaInterna<Cliente>> ObtenerPorDniAsync(int dni);
        Task<RespuestaInterna<bool>> EliminarAsync(int dni);
        Task<RespuestaInterna<Cliente>> LoginAuth(int dni ,string usuario, string contraseña);
    }
}
