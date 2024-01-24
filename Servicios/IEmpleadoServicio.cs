using Core.DTO;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public interface IEmpleadoServicio
    {
        Task<RespuestaInterna<bool>> AgregarEmpleado(Empleado empleado);
        Task<RespuestaInterna<List<Empleado>>> ObtenerAsync();
        Task<RespuestaInterna<Empleado>> ObtenerPorLegajoAsync(int legajo);

        Task<RespuestaInterna<bool>> EliminarAsync(int legajo);
    }
}
