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
        public int Monto { get; set; }
        public DateOnly Fecha { get; set; }
        [ForeignKey("CuentaOrigen")]
        public int CuentaOrigenId { get; set; }
        public Cuenta CuentaOrigen { get; set; }
        [ForeignKey("CuentaDestino")]
        public int CuentaDestinoId { get; set; }
        public Cuenta CuentaDestino { get; set; }
    }
}

