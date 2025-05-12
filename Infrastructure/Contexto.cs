using DesafioBackendAPI.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace DesafioBackendAPI.Infrastructure
{
    public partial class Contexto : DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public Contexto(IConfiguration configuratation)
        {
            _configuration = configuratation;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(_connectionString)
                .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Conta>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<Transacao>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<Conta>()
                .HasMany(e => e.Transacoes)
                .WithOne(e => e.Conta)
                .HasForeignKey(e => e.ContaId)
                .IsRequired();

            OnModelCreatingPartial(modelBuilder);
        }

        public virtual DbSet<Conta> Conta { get; set; }
        public virtual DbSet<Transacao> Transacao { get; set; }

        public virtual DbSet<Usuario> Usuario { get; set; }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


    }
}