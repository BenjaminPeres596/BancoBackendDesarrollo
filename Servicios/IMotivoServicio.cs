using Core.DTO;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public interface IMotivoServicio
    {
        Task<RespuestaInterna<bool>> Post(TipoMotivo motivo);
        Task<RespuestaInterna<bool>> Delete(int id);
        Task<RespuestaInterna<List<TipoMotivo>>> Get();
    }
}
