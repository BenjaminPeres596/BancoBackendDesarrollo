using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Cuenta
    {
        [Key]
        public int Id { get; set; }
        public int NroCuenta { get; set; }
        public string FechaAlta { get; set; }
        public float Saldo { get; set; }
        [ForeignKey("Cliente")]
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        [ForeignKey("TipoCuenta")]
        public int TipoCuentaId { get; set; }
        public TipoCuenta TipoCuenta { get; set; }
    }
}
