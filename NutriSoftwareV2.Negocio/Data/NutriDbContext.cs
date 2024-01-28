using NutriSoftwareV2.Negocio.Domain;
using NutriSoftwareV2.Negocio.Data.Configuration;
using Microsoft.EntityFrameworkCore;

namespace NutriSoftwareV2.Negocio.Data.NutriDbContext
{
    public class NutriDbContext : DbContext
    {
       
        public DbSet<Paciente> pacientes{get ; set;}
        public DbSet<AnotacoesPaciente> AnotacosPaciente { get; set; }
        public DbSet<AvaliacaoFisica> AvaliacoesFisicas { get; set; }
        public DbSet<TipoMedida> TipoMedidas { get; set; }
        public DbSet<AlimentoBebida> AlimentoBebidas { get; set; }
        public DbSet<QuantidadeAlimento> QuandadeAlimento { get; set; }
        public DbSet<PlanoAlimentar> PlanoAlimentar { get; set; }
        public DbSet<ObservacaoPlano> ObservacaoPlano{get ; set;}
        public DbSet<Dieta> Dieta{get ; set;}
        public DbSet<Consulta> Consulta{get ; set;}
        public DbSet<Pagamento> Pagamento{get ; set;}
        public DbSet<LocaisAtendimento> LocalAtendimento{get ; set;}
        public DbSet<Profissao> Profissao { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        {
            //var connetionString = Configuration.GetConnectionString("DefaultConnection");
            //options.UseMySql(connetionString, ServerVersion.AutoDetect(connetionString));
            optionsBuilder
            .UseMySQL("Server=localhost; Database=softnutricao; Convert Zero Datetime=True; Uid=root; Pwd=123456;"); 
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(NutriDbContext).Assembly);
        }
    }
}