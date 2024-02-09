using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutriSoftwareV2.Negocio.Domain;

namespace NutriSoftwareV2.Negocio.Data.Configuration
{
    public class ArquivosPacienteConfiguration : IEntityTypeConfiguration<Arquivo>
    {
        public void Configure(EntityTypeBuilder<Arquivo> builder)
        {
            builder.ToTable("Arquivo");
            builder.HasKey(p=>new{p.Id});

            builder.HasOne(p => p.Paciente)
                .WithMany(a => a.Arquivos)
                .HasForeignKey(p => p.PacienteId);
            
        }
    }
}