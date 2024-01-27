using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutriSoftwareV2.Negocio.Domain;

namespace NutriSoftwareV2.Negocio.Data.Configuration
{
    public class TipoMediaConfiguration : IEntityTypeConfiguration<TipoMedida>
    {
        public void Configure(EntityTypeBuilder<TipoMedida> builder)
        {
            builder.ToTable("TipoMedida");
            builder.HasKey(p=>p.Id);
        }
    }
}