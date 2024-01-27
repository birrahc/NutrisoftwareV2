
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutriSoftwareV2.Negocio.Domain;
using NutriV2.Domain;

namespace NutriSoftwareV2.Negocio.Data.Configuration
{
    public class PlanoAlimentarConfiguration : IEntityTypeConfiguration<PlanoAlimentar>
    {
        public void Configure(EntityTypeBuilder<PlanoAlimentar> builder)
        {
            builder.ToTable("PlanoAlimentar");
            builder.HasKey(p=>new { p.CodigoDieta,p.HoraAlimentos});
            builder.HasMany(p => p.QuantidadeAlimentos)
                .WithOne(p=>p.PlanoAlimentar)
                .HasForeignKey(p =>new { p.CodigoDieta, p.Hora });

           
                
            //builder.HasKey(p=> new {p.PacienteId, p.HoraId, p.AlimentoId,p.Tipo});
            //builder.Property(p => p.AlimentoId).HasColumnName("AlimentoId");

        }
    }
}