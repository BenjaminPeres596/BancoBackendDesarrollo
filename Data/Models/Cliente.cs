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
        public string Usuario {  get; set; }
        public string Clave {  get; set; }
        public int Cuit { get; set; }
        public string Mail { get; set; }
        [ForeignKey("Banco")]
        public int BancoId { get; set; }
        public Banco Banco { get; set; }
    }
}
