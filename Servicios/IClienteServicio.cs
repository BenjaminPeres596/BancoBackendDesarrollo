using Data.Models;
using Core.DTO;

namespace Servicios
{
    public interface IClienteServicio
    {
        Task<RespuestaInterna<Cliente>> AgregarAsync(Cliente cliente);
        Task<RespuestaInterna<List<Cliente>>> ObtenerAsync();
        Task<RespuestaInterna<Cliente>> ObtenerPorDniAsync(long cuil);
        Task<RespuestaInterna<bool>> EliminarAsync(long cuil);
        Task<RespuestaInterna<Cliente>> LoginAuth(long cuil ,string usuario, string contraseña, string authCode);
        Task<RespuestaInterna<ClienteData>> AuthRenaper(string authCode);
    }

    public class ClienteData
    {
        public string Nombre { get; set; }
        public string Rol { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Cuil { get; set; }
        public bool Estado { get; set; }
        public bool EstadoCrediticio { get; set; }
    }
    public class ClienteJSON
    {
        public string clientId { get; set; }
        public string clientSecret { get; set; }
        public string authorizationCode { get; set; }

    }
}
