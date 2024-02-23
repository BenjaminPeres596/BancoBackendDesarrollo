using Core.DTO;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public interface ICuentaServicio
    {
        Task<RespuestaInterna<bool>> CrearCuentaAsync(Cuenta cuenta, int dni);
        Task<RespuestaInterna<List<Cuenta>>> ObtenerAsync();
        Task<RespuestaInterna<List<Cuenta>>> ObtenerPorDniAsync(int dni);
        Task<RespuestaInterna<List<Cuenta>>> ObtenerPorCbuAsync(string cbu);
        Task<RespuestaInterna<bool>> EliminarAsync(int id, int dni);
    }
}
