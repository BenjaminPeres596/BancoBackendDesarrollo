using Data.Models;
using Core.DTO;

namespace Servicios
{
    public interface IBancoServicio
    {
        Task<RespuestaInterna<bool>> AgregarAsync(Banco banco);
        Task<RespuestaInterna<List<Banco>>> ObtenerAsync();
        Task<RespuestaInterna<bool>> EliminarAsync(string razonSocial);
    }
}
