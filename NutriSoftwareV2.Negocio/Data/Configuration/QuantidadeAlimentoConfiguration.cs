
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutriSoftwareV2.Negocio.Domain;

namespace NutriSoftwareV2.Negocio.Data.Configuration
{
    public class QuantidadeAlimentoConfiguration : IEntityTypeConfiguration<QuantidadeAlimento>
    {
        public void Configure(EntityTypeBuilder<QuantidadeAlimento> builder)
        {
            builder.ToTable("QuantidadeAlimento");
            builder.HasKey(p=> new {p.CodigoDieta,p.Hora,p.AlimentoId,p.Tipo });
            //builder.Property(p => p.AlimentoId).HasColumnName("AlimentoId");
           
                

        }
    }
}