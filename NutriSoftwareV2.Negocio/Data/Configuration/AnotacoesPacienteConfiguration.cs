using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutriSoftwareV2.Negocio.Domain;

namespace NutriSoftwareV2.Negocio.Data.Configuration
{
    public class AnotacoesPacienteConfiguration : IEntityTypeConfiguration<AnotacoesPaciente>
    {
        public void Configure(EntityTypeBuilder<AnotacoesPaciente> builder)
        {
            builder.ToTable("Observacao");
            builder.HasKey(p=>new{p.Id});
            
        }
    }
}