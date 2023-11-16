using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class RespuestaInterna<T>
    {
        public T Datos { get; set; }
        public bool Exito { get; set; } = false;
        public string Mensaje { get; set; } = "";
    }
}
