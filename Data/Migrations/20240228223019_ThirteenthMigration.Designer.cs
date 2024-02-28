﻿// <auto-generated />
using System;
using Data.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Migrations
{
    [DbContext(typeof(BancoDBContext))]
    [Migration("20240228223019_ThirteenthMigration")]
    partial class ThirteenthMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Data.Models.Banco", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Calle")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("Numero")
                        .HasColumnType("bigint");

                    b.Property<string>("RazonSocial")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("Telefono")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Banco");
                });

            modelBuilder.Entity("Data.Models.Cliente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Apellido")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("BancoId")
                        .HasColumnType("integer");

                    b.Property<string>("Clave")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("Cuil")
                        .HasColumnType("bigint");

                    b.Property<string>("Mail")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Sal")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Usuario")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("BancoId");

                    b.ToTable("Cliente");
                });

            modelBuilder.Entity("Data.Models.Cuenta", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Cbu")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ClienteId")
                        .HasColumnType("integer");

                    b.Property<string>("FechaAlta")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<float>("Saldo")
                        .HasColumnType("real");

                    b.Property<int>("TipoCuentaId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ClienteId");

                    b.HasIndex("TipoCuentaId");

                    b.ToTable("Cuenta");
                });

            modelBuilder.Entity("Data.Models.Empleado", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Apellido")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("BancoId")
                        .HasColumnType("integer");

                    b.Property<long>("Dni")
                        .HasColumnType("bigint");

                    b.Property<long>("Legajo")
                        .HasColumnType("bigint");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("BancoId");

                    b.ToTable("Empleado");
                });

            modelBuilder.Entity("Data.Models.TipoCuenta", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("TipoCuenta");
                });

            modelBuilder.Entity("Data.Models.TipoMotivo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("TipoMotivo");
                });

            modelBuilder.Entity("Data.Models.Transferencia", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CuentaDestinoId")
                        .HasColumnType("integer");

                    b.Property<int>("CuentaOrigenId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("Monto")
                        .HasColumnType("bigint");

                    b.Property<int>("TipoMotivoId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CuentaDestinoId");

                    b.HasIndex("CuentaOrigenId");

                    b.HasIndex("TipoMotivoId");

                    b.ToTable("Transferencia");
                });

            modelBuilder.Entity("Data.Models.Cliente", b =>
                {
                    b.HasOne("Data.Models.Banco", "Banco")
                        .WithMany()
                        .HasForeignKey("BancoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Banco");
                });

            modelBuilder.Entity("Data.Models.Cuenta", b =>
                {
                    b.HasOne("Data.Models.Cliente", "Cliente")
                        .WithMany()
                        .HasForeignKey("ClienteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Models.TipoCuenta", "TipoCuenta")
                        .WithMany()
                        .HasForeignKey("TipoCuentaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cliente");

                    b.Navigation("TipoCuenta");
                });

            modelBuilder.Entity("Data.Models.Empleado", b =>
                {
                    b.HasOne("Data.Models.Banco", "Banco")
                        .WithMany()
                        .HasForeignKey("BancoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Banco");
                });

            modelBuilder.Entity("Data.Models.Transferencia", b =>
                {
                    b.HasOne("Data.Models.Cuenta", "CuentaDestino")
                        .WithMany()
                        .HasForeignKey("CuentaDestinoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Models.Cuenta", "CuentaOrigen")
                        .WithMany()
                        .HasForeignKey("CuentaOrigenId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Models.TipoMotivo", "TipoMotivo")
                        .WithMany()
                        .HasForeignKey("TipoMotivoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CuentaDestino");

                    b.Navigation("CuentaOrigen");

                    b.Navigation("TipoMotivo");
                });
#pragma warning restore 612, 618
        }
    }
}
