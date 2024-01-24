using Core.DTO;
using Data.Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class EmpleadoServicio : IEmpleadoServicio
    {
        private BancoDBContext _bancoDBContext;
        public EmpleadoServicio()
        {
            _bancoDBContext = new BancoDBContext();
        }

        public async Task<RespuestaInterna<bool>> AgregarEmpleado(Empleado empleado)
        {
            var respuesta = new RespuestaInterna<bool>();
            try
            {
                var empleadoExiste = await _bancoDBContext.Empleado.FirstOrDefaultAsync(e => e.Legajo == empleado.Legajo);
                if (empleadoExiste == null)
                {
                    respuesta.Datos = true;
                    respuesta.Exito = true;
                    await _bancoDBContext.Empleado.AddAsync(empleado);
                    await _bancoDBContext.SaveChangesAsync();
                    return respuesta;
                }
                else
                {
                    respuesta.Mensaje = "El empleado ya existe";
                    respuesta.Datos = false;
                    return respuesta;
                }
            }
            catch (Exception ex)
            {
                respuesta.Datos = false;
                respuesta.Mensaje = "No se pudo agregar al empleado. Detalles: " + ex.Message;
                return respuesta;
            }
        }
        public async Task<RespuestaInterna<List<Empleado>>> ObtenerAsync()
        {
            var respuesta = new RespuestaInterna<List<Empleado>>();
            try
            {
                var empleados = await _bancoDBContext.Empleado.ToListAsync();
                respuesta.Datos = empleados;
                respuesta.Exito = true;
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = "No se pudo recuperar la lista de empleados. Detalles: " + ex.Message;
                return respuesta;
            }

        }

        public async Task<RespuestaInterna<Empleado>> ObtenerPorLegajoAsync(int legajo)
        {
            var respuesta = new RespuestaInterna<Empleado>();
            var empleadoExiste = await _bancoDBContext.Empleado.FirstOrDefaultAsync(e => e.Legajo == legajo);
            if (empleadoExiste == null)
            {
                respuesta.Mensaje = "No existe el empleado, intente con otro legajo.";
                return respuesta;
            }
            try
            {
                respuesta.Datos = empleadoExiste;
                respuesta.Exito = true;
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = "No se pudo recuperar al empleado. Detalles: " + ex.Message;
                return respuesta;
            }

        }

        public async Task<RespuestaInterna<bool>> EliminarAsync(int legajo)
        {
            var respuesta = new RespuestaInterna<bool>();
            var empleadoExiste = await _bancoDBContext.Empleado.FirstOrDefaultAsync(x => x.Legajo == legajo);
            if (empleadoExiste == null)
            {
                respuesta.Datos = false;
                respuesta.Mensaje = "El empleado no existe";
                return respuesta;
            }
            try
            {
                _bancoDBContext.Remove(empleadoExiste);
                await _bancoDBContext.SaveChangesAsync();
                respuesta.Datos = true;
                respuesta.Exito = true;
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = "No se pudo eliminar al empleado. Detalles: " + ex.Message;
                return respuesta;
            }
        }
    }
}
