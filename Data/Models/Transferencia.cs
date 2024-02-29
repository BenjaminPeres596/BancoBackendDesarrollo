using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Transferencia
    {
        [Key]
        public int Id { get; set; }
        public int NroTransferencia { get; set; }
        public long Monto { get; set; }
        public DateTime Fecha { get; set; }
        [ForeignKey("CuentaOrigen")]
        public int CuentaOrigenId { get; set; }
        public Cuenta CuentaOrigen { get; set; }
        [ForeignKey("CuentaDestino")]
        public int CuentaDestinoId { get; set; }
        public Cuenta CuentaDestino { get; set; }
        [ForeignKey("TipoMotivo")]
        public int TipoMotivoId { get; set; }
        public TipoMotivo TipoMotivo { get; set; }
    }
}

