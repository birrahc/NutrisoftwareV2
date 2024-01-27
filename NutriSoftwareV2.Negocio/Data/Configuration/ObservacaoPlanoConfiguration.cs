using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutriSoftwareV2.Negocio.Domain;

namespace NutriSoftwareV2.Negocio.Data.Configuration
{
    public class ObservacaoPlanoConfiguration : IEntityTypeConfiguration<ObservacaoPlano>
    {
        public void Configure(EntityTypeBuilder<ObservacaoPlano> builder)
        {
            builder.ToTable("ObservacaoPlano");
            builder.HasKey(p=>new { p.Id });
        }
    }
}