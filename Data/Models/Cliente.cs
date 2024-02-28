using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace Data.Models
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Usuario { get; set; }
        public string Clave { get; set; }
        public string Sal { get; set; }
        public long Cuil { get; set; }
        public string Mail { get; set; }
        public int BancoId { get; set; }
        public Banco Banco { get; set; }

        public void EstablecerClave(string contraseña)
        {
            Sal = GenerarSalUnica();
            string contraseñaConSal = contraseña + Sal;
            Clave = HashContraseña(contraseñaConSal);
        }
        public bool VerificarClave(string contraseña)
        {
            string contraseñaConSal = contraseña + Sal;
            string hashContraseña = HashContraseña(contraseñaConSal);
            return Clave == hashContraseña;
        }
        private string GenerarSalUnica()
        {
            byte[] bytesSal = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(bytesSal);
            }
            return Convert.ToBase64String(bytesSal);
        }
        private string HashContraseña(string contraseña)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytesContraseña = Encoding.UTF8.GetBytes(contraseña);
                byte[] hashBytes = sha256.ComputeHash(bytesContraseña);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
