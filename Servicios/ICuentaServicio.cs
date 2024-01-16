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
        Task<RespuestaInterna<bool>> CrearCuentaAsync(Cuenta cuenta, int cuit);
        Task<RespuestaInterna<List<Cuenta>>> ObtenerAsync();
        Task<RespuestaInterna<List<Cuenta>>> ObtenerPorCuitAsync(int cuit);

        Task<RespuestaInterna<bool>> EliminarAsync(int nroCuenta, int cuit);
    }
}
