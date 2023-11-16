using Data.Models;
using Core.DTO;

namespace Servicios
{
    public interface IClienteServicio
    {
        Task<RespuestaInterna<bool>> AgregarAsync(Cliente cliente);
        Task<RespuestaInterna<List<Cliente>>> ObtenerAsync();
    }
}
