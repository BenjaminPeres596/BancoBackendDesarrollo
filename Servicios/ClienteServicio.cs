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
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

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
            var clienteExiste = await _bancoDBContext.Cliente.FirstOrDefaultAsync(x => x.Cuil == cliente.Cuil);
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

        public async Task<RespuestaInterna<Cliente>> ObtenerPorDniAsync(long cuil)
        {
            var respuesta = new RespuestaInterna<Cliente>();
            var clienteExiste = await _bancoDBContext.Cliente.Include(x => x.Banco).FirstOrDefaultAsync(x => x.Cuil == cuil);
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

        public async Task<RespuestaInterna<bool>> EliminarAsync(long cuil)
        {
            var respuesta = new RespuestaInterna<bool>();
            var clienteExiste = await _bancoDBContext.Cliente.FirstOrDefaultAsync(x => x.Cuil == cuil);
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

        public async Task<RespuestaInterna<Cliente>> LoginAuth(long cuil, string usuario, string contraseña, string authCode)
        {
            var respuesta = new RespuestaInterna<Cliente>();
            try
            {
                var respuestaRenaper = await AuthRenaper(authCode);
                var clienteExiste = await _bancoDBContext.Cliente.Where(x => x.Cuil == cuil && x.Usuario == usuario).FirstOrDefaultAsync();

                if (respuestaRenaper.Exito == false)
                {
                    respuesta.Mensaje = "Error al validar con el renaper vuelva a intentar";
                    return respuesta;
                }
                else if (respuestaRenaper.Datos.Estado == false)
                {
                    respuesta.Mensaje = "El cliente no se encuentra habilitado, comuniquese con el renaper";
                    return respuesta;
                }
                else if (clienteExiste == null || !clienteExiste.VerificarClave(contraseña))
                {
                    respuesta.Mensaje = "Dni, usuario o contraseña incorrectos, intente nuevamente.";
                    return respuesta;
                }
                else if (long.Parse(respuestaRenaper.Datos.Cuil) != clienteExiste.Cuil)
                {
                    respuesta.Mensaje = ("No coincide el Usuario del Renaper con el Cliente ingresado");
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

        public async Task<RespuestaInterna<ClienteData>> AuthRenaper(string authCode)
        {
            var respuesta = new RespuestaInterna<ClienteData>();
            try
            {
                var options = new RestClientOptions("https://colosal.duckdns.org:15001/renaper/api/Auth/loguearJWT");
                var client = new RestClient();
                var JSONCliente = new ClienteJSON
                {
                    clientId = "88cd7688-cb2b-4499-8d43-c805a4c734bf",
                    clientSecret = "12345678",
                    authorizationCode = authCode
                };
                var request = new RestRequest("https://colosal.duckdns.org:15001/renaper/api/Auth/loguearJWT").AddJsonBody(JSONCliente);
                var response = await client.PostAsync<RespuestaInterna<string>>(request);

                if (response.Exito)
                {
                    var jwtToken = response.Datos;

                    var tokenHandler = new JwtSecurityTokenHandler();

                    // Cargar la clave pública RSA desde el XML
                    RSA rsaPublicKey = RSA.Create();
                    rsaPublicKey.FromXmlString(@"<RSAKeyValue><Modulus>9BJ0WxXATSJ6KtiSHhglSd3kgc6j5kXLp8sx5hm5KN2Y8H1uygVrPAJGBqPEIgRpMHG8yMFyKh2hXLSnZNLtZ+7c+fMIUYJYARS8f4yxF3CpkMtVW4wJ5Sbg99vIyi8Hi/134QuwU9ghYKiGgaYEvsQo5P9R+y/MiJrclETu5mkUdazs0Sua5+WdnsmJqykVxrfHtgvlavtmhF2B8zUWWOb8zdPgWqzxULt4RHWIasdf6GxzG+XGK+6jyNfb4DpUJQBlHssVGgflNEukoYefTcqx865JeGMeIBJzmxceiD2PrEnDsHHYk8w5/2dAWbnf8Pk19T3CXDDd73MLiPR5xQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>");

                    var validationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new RsaSecurityKey(rsaPublicKey),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };

                    ClaimsPrincipal principal = tokenHandler.ValidateToken(jwtToken, validationParameters, out SecurityToken validatedToken);
                    var jwtPayload = ((JwtSecurityToken)validatedToken).Payload;

                    var clienteData = new ClienteData
                    {
                        Nombre = jwtPayload["Nombre"].ToString(),
                        Rol = jwtPayload["Rol"].ToString(),
                        Apellido = jwtPayload["Apellido"].ToString(),
                        Email = jwtPayload["Email"].ToString(),
                        Cuil = jwtPayload["Cuil"].ToString(),
                        Estado = Convert.ToBoolean(jwtPayload["Estado"]),
                        EstadoCrediticio = Convert.ToBoolean(jwtPayload["EstadoCrediticio"])
                    };

                    respuesta.Datos = clienteData;
                    respuesta.Exito = true;
                    respuesta.Mensaje = "Cliente validado correctamente";
                }
                else
                {
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