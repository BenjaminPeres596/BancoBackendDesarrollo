using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        //Como un Cliente se logea al banco?
        public int Cuit { get; set; }
        public string Mail { get; set; }
        public string Calle { get; set; }
        public int Numero { get; set; }
    }
}
