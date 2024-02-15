using Core.DTO;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public interface ITransferenciaServicio
    {
        Task<RespuestaInterna<bool>> Post(Transferencia transferencia, string cbuOrigen, string cbuDestino, float monto, int MotivoId);
        Task<RespuestaInterna<List<Transferencia>>> Get(int id);
    }
}
