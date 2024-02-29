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
        Task<RespuestaInterna<bool>> CrearCuentaAsync(Cuenta cuenta, long cuil);
        Task<RespuestaInterna<List<Cuenta>>> ObtenerAsync();
        Task<RespuestaInterna<List<Cuenta>>> ObtenerPorCuilAsync(long cuil);
        Task<RespuestaInterna<Cuenta>> ObtenerPorCbuAsync(string cbu);
        Task<RespuestaInterna<bool>> EliminarAsync(int id, long cuil);
        Task<RespuestaInterna<Cuenta>> CrearCuentaExterna(string cbuCuenta);
    }
}
