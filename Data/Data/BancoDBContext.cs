using Microsoft.EntityFrameworkCore;
using Data.Models;

namespace Data.Data
{
    public partial class BancoDBContext : DbContext
    {
        public BancoDBContext()
        {
        }

        public BancoDBContext(DbContextOptions<BancoDBContext> options) : base(options)
        {
        }

        public virtual DbSet<Empleado> Empleado { get; set; }
        public virtual DbSet<Cuenta> Cuenta { get; set; }
        public virtual DbSet<Cliente> Cliente { get; set; }
        public virtual DbSet<TipoCuenta> TipoCuenta { get; set; }
        public virtual DbSet<Transferencia> Transferencia { get; set; } 
        public virtual DbSet<Banco> Banco { get; set; }
        public virtual DbSet<TipoMotivo> TipoMotivo { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=colosal.duckdns.org;Port=14998;Database=S31-Grupo8-Banco-Generacion;Persist Security Info=True;Password=tallersoft600;Username=postgres");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
