
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutriSoftwareV2.Negocio.Domain;

namespace NutriSoftwareV2.Negocio.Data.Configuration
{
    public class DietaConfiguration : IEntityTypeConfiguration<Dieta>
    {
        public void Configure(EntityTypeBuilder<Dieta> builder)
        {
            builder.ToTable("Dieta");
            builder.HasKey(p=> new {p.CodigoDieta});

        }
    }
}