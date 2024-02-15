using Core.DTO;
using Data.Data;
using Microsoft.EntityFrameworkCore;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class MotivoServicio : IMotivoServicio
    {
        private BancoDBContext _bancoDBContext;
        public MotivoServicio()
        {
            _bancoDBContext = new BancoDBContext();
        }

        public async Task<RespuestaInterna<bool>> Post(TipoMotivo motivo)
        {
            var respuesta = new RespuestaInterna<bool>();
            try
            {
                var motivoExiste = await _bancoDBContext.TipoMotivo.FirstOrDefaultAsync(m => m.Nombre == motivo.Nombre);
                if (motivoExiste != null)
                {
                    respuesta.Datos = false;
                    respuesta.Mensaje = "Ya existe el motivo";
                    return respuesta;
                }
                else
                {
                    respuesta.Datos = true;
                    respuesta.Exito = true;
                    respuesta.Mensaje = "Motivo creado exitosamente";
                    await _bancoDBContext.TipoMotivo.AddAsync(motivo);
                    await _bancoDBContext.SaveChangesAsync();
                    return respuesta;
                }
            }
            catch (Exception ex)
            {
                respuesta.Datos = false;
                respuesta.Exito = false;
                respuesta.Mensaje = ex.Message;
                return respuesta;
            }
        }

        public async Task<RespuestaInterna<bool>> Delete(int id)
        {
            var respuesta = new RespuestaInterna<bool>();
            try
            {
                var tipoMotivoExiste = await _bancoDBContext.TipoMotivo.FirstOrDefaultAsync(tm => tm.Id == id);
                if (tipoMotivoExiste == null)
                {
                    respuesta.Datos = false;
                    respuesta.Mensaje = "No existe el motivo";
                    return respuesta;
                }
                else
                {
                    respuesta.Datos = true;
                    respuesta.Exito = true;
                    respuesta.Mensaje = "Motivo eliminado correctamente";
                    _bancoDBContext.TipoMotivo.Remove(tipoMotivoExiste);
                    await _bancoDBContext.SaveChangesAsync();
                    return respuesta;
                }
            }
            catch (Exception ex)
            {
                respuesta.Datos = false;
                respuesta.Mensaje = "No se pudo eliminar el motivo. Detalles: " + ex.Message;
                return respuesta;
            }
        }

        public async Task<RespuestaInterna<List<TipoMotivo>>> Get()
        {
            var respuesta = new RespuestaInterna<List<TipoMotivo>>();
            try
            {
                var motivos = await _bancoDBContext.TipoMotivo.ToListAsync();
                respuesta.Datos = motivos;
                respuesta.Exito = true;
                respuesta.Mensaje = "Motivos recuperados correctamente";
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = "No se pudo recuperar los motivos. Detalles: " + ex.Message;
                return respuesta;
            }
        }
    }
}