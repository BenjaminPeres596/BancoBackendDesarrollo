using Core.DTO;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public interface ITipoCuentaServicio
    {
        Task<RespuestaInterna<bool>> Post(TipoCuenta tipocuenta);
        Task<RespuestaInterna<bool>> Delete(int id);
        Task<RespuestaInterna<List<TipoCuenta>>> Get();
    }
}
