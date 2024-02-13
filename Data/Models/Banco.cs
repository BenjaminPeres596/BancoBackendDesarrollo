using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Banco
    {
        [Key]
        public int Id { get; set; }
        public string RazonSocial { get; set; }
        public long Telefono { get; set; }
        public string Calle { get; set; }
        public long Numero { get; set; }
    }
}
