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
        Task<RespuestaInterna<bool>> Post(Transferencia transferencia, string cbuOrigen, string cbuDestino, float monto, string motivoNombre);
        Task<RespuestaInterna<List<Transferencia>>> Get(int id);
        Task<RespuestaInterna<Transferencia>> PostTransferenciaExterna(TransferenciaJSON transferencia);
    }

    public class TransferenciaJSON
    {
        public string origin_cbu { get; set; }
        public float amount { get; set; }
        public string destination_cbu { get; set; }
        public string motive { get; set; }
        public int origin_cuil { get; set; }
        public int destination_cuil { get; set; }

    }

    public class ConfirmacionJSON
    {
        public int NumeroTrans { get; set; }
        public string Estado { get; set; }
    }
}
