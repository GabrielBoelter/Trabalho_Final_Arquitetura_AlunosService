using AlunosService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace AlunosService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Aluno> Alunos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Aluno>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Telefone).HasMaxLength(20);
                entity.Property(e => e.CPF).HasMaxLength(14);
                entity.Property(e => e.DataNascimento).IsRequired();
                entity.Property(e => e.Endereco).HasMaxLength(200);
                entity.Property(e => e.Cidade).HasMaxLength(50);
                entity.Property(e => e.Estado).HasMaxLength(2);
                entity.Property(e => e.CEP).HasMaxLength(10);
                entity.Property(e => e.Status).IsRequired();
                entity.Property(e => e.DataCadastro).IsRequired();
                entity.Property(e => e.Observacoes).HasMaxLength(500);

                // Índices
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.CPF).IsUnique();
                entity.HasIndex(e => e.Status);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}