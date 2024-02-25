using Core.DTO;
using Data.Models;
using Data.Data;
using Microsoft.EntityFrameworkCore;
using RestSharp;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Servicios
{
    public class ClienteServicio : IClienteServicio
    {
        private BancoDBContext _bancoDBContext;
        public ClienteServicio()
        {
            _bancoDBContext = new BancoDBContext();
        }
        public async Task<RespuestaInterna<Cliente>> AgregarAsync(Cliente cliente)
        {
            var respuesta = new RespuestaInterna<Cliente>();
            var clienteExiste = await _bancoDBContext.Cliente.FirstOrDefaultAsync(x => x.Dni == cliente.Dni);
            var bancoExiste = await _bancoDBContext.Banco.FirstOrDefaultAsync(x => x.Id == cliente.BancoId);
            if (clienteExiste != null)
            {
                respuesta.Mensaje = "El cliente ya existe";
                return respuesta;
            }
            if (bancoExiste == null)
            {
                respuesta.Mensaje = "El banco no existe";
                return respuesta;
            }

            try
            {
                cliente.Banco = bancoExiste;
                cliente.EstablecerClave(cliente.Clave);
                await _bancoDBContext.Cliente.AddAsync(cliente);
                await _bancoDBContext.SaveChangesAsync();
                respuesta.Exito = true;
                respuesta.Datos = cliente;
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = "No se pudo agregar al cliente. Detalles: " + ex.Message;
                return respuesta;
            }
        }

        public async Task<RespuestaInterna<List<Cliente>>> ObtenerAsync()
        {
            var respuesta = new RespuestaInterna<List<Cliente>>();
            try
            {
                var clientes = await _bancoDBContext.Cliente.Include(x => x.Banco).ToListAsync();
                respuesta.Datos = clientes;
                respuesta.Exito = true;
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = "No se pudo recuperar los clientes. Detalles: " + ex.Message;
                return respuesta;
            }

        }

        public async Task<RespuestaInterna<Cliente>> ObtenerPorDniAsync(int dni)
        {
            var respuesta = new RespuestaInterna<Cliente>();
            var clienteExiste = await _bancoDBContext.Cliente.Include(x => x.Banco).FirstOrDefaultAsync(x => x.Dni == dni);
            if (clienteExiste == null)
            {
                respuesta.Mensaje = "El cliente no existe";
                return respuesta;
            }
            try
            {
                respuesta.Datos = clienteExiste;
                respuesta.Exito = true;
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = "No se pudo obtener el cliente. Detalles: " + ex.Message;
                return respuesta;
            }
        }

        public async Task<RespuestaInterna<bool>> EliminarAsync(int dni)
        {
            var respuesta = new RespuestaInterna<bool>();
            var clienteExiste = await _bancoDBContext.Cliente.FirstOrDefaultAsync(x => x.Dni == dni);
            if (clienteExiste == null)
            {
                respuesta.Datos = false;
                respuesta.Mensaje = "El cliente no existe";
                return respuesta;
            }
            try
            {
                _bancoDBContext.Remove(clienteExiste);
                await _bancoDBContext.SaveChangesAsync();
                respuesta.Datos = true;
                respuesta.Exito = true;
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = "No se pudo eliminar al cliente. Detalles: " + ex.Message;
                return respuesta;
            }
        }

        public async Task<RespuestaInterna<Cliente>> LoginAuth(int dni, string usuario, string contraseña)
        {
            var respuesta = new RespuestaInterna<Cliente>();
            try
            {
                var clienteExiste = await _bancoDBContext.Cliente.Where(x => x.Dni == dni && x.Usuario == usuario).FirstOrDefaultAsync();

                if (clienteExiste == null || !clienteExiste.VerificarClave(contraseña))
                {
                    respuesta.Mensaje = "Dni, usuario o contraseña incorrectos, intente nuevamente.";
                    return respuesta;
                }
                else
                {
                    respuesta.Datos = clienteExiste;
                    respuesta.Exito = true;
                    respuesta.Mensaje = "Inicio de sesion exitoso.";
                    return respuesta;
                }
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = "No se pudo verificar al cliente. Detalles: " + ex.Message;
                return respuesta;
            }
        }
        // Ejemplo ficticio para corregir los errores CS0136 y CS0103
        public async Task<RespuestaInterna<ClienteData>> AuthRenaper(string authCode)
        {
            var respuesta = new RespuestaInterna<ClienteData>();
            try
            {
                var options = new RestClientOptions("https://colosal.duckdns.org:15001/renaper/api/Auth/loguearJWT")
                {
                    //Authenticator = new HttpBasicAuthenticator("username", "password")
                };
                //var client = new RestClient(options);
                var client = new RestClient();
                //var request = new RestRequest("statuses/home_timeline.json");
                var JSONCliente = new ClienteJSON
                {
                    clientId = "88cd7688-cb2b-4499-8d43-c805a4c734bf",
                    clientSecret = "12345678",
                    authorizationCode = authCode
                };
                var request = new RestRequest("https://colosal.duckdns.org:15001/renaper/api/Auth/loguearJWT").AddJsonBody(JSONCliente);
                // The cancellation token comes from the caller. You can still make a call without it.
                var response = await client.PostAsync<RespuestaInterna<string>>(request);

                // Verificar si la respuesta fue exitosa
                if (response.Exito)
                {
                    // Obtener el JWT de la respuesta
                    var jwtToken = response.Datos;

                    // Decodificar el JWT en el payload
                    string[] jwtParts = jwtToken.Split('.');
                    string payload = jwtParts[1];
                    byte[] payloadBytes = Convert.FromBase64String(PadBase64String(payload));
                    string decodedPayload = Encoding.UTF8.GetString(payloadBytes);

                    var clienteData = JsonConvert.DeserializeObject<ClienteData>(decodedPayload);

                    // Asignar el payload decodificado a la respuesta
                    respuesta.Datos = clienteData;
                    respuesta.Exito = true;
                    respuesta.Mensaje = "Cliente validado correctamente";
                }
                else
                {
                    // En caso de que la respuesta no sea exitosa, asignar el mensaje de la respuesta original
                    respuesta.Mensaje = response.Mensaje;
                }

                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = "No se pudo verificar al cliente. Detalles: " + ex.Message;
                return respuesta;
            }
        }

        // Asegurar que la cadena Base64 tenga la longitud adecuada
        static string PadBase64String(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: return base64 + "==";
                case 3: return base64 + "=";
                default: return base64;
            }
        }
    }
}