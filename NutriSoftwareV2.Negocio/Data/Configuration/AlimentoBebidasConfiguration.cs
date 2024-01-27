using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutriSoftwareV2.Negocio.Domain;

namespace NutriSoftwareV2.Negocio.Data.Configuration
{
    public class AlimentoBebidasConfiguration : IEntityTypeConfiguration<AlimentoBebida>
    {
        public void Configure(EntityTypeBuilder<AlimentoBebida> builder)
        {

            builder.ToTable("AlimentoBebida");
            builder.HasKey(p=>p.Id);
            
        }
    }
}