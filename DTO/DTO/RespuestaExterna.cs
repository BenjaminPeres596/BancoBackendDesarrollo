using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class RespuestaExterna<T>
    {
        public T Datos { get; set; }
        public bool Exito { get; set; } = false;
        public string MensajePublico { get; set; } = "";
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
