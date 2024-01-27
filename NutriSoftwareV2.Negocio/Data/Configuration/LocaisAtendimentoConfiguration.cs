
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutriSoftwareV2.Negocio.Domain;

namespace NutriSoftwareV2.Negocio.Data.Configuration
{
    public class LocaisAtendimentoConfiguration : IEntityTypeConfiguration<LocaisAtendimento>
    {
        public void Configure(EntityTypeBuilder<LocaisAtendimento> builder)
        {
            builder.ToTable("LocaisAtendimento");
            builder.HasKey(p=>p.Id);
            

            //builder.Property(p=>p.Nome).HasColumnType("VARCHAR(60)").IsRequired();
            //builder.Property(p=>p.Sexo).HasColumnType("INT").IsRequired();
            //builder.Property(p=>p.Nascimento).HasColumnType("DATE").IsRequired();
            //builder.Property(p=>p.CPF).HasColumnType("VARCHAR(11)").IsRequired(false);
            //builder.Property(p=>p.Email).HasColumnType("VARCHAR(40)").IsRequired(false);
            //builder.Property(p=>p.Telefone).HasColumnType("VARCHAR(11)").IsRequired(false);
            //builder.Property(p=>p.TipoDeContatoTelefone).IsRequired(false);

            // builder.HasMany(p=>p.Avaliacoes)
            //     .WithOne(p=>p.Paciente)
            //     .OnDelete(DeleteBehavior.Cascade);

            // // builder.HasOne(p=>p.Anminese)
            // //      .WithOne(p=>p.Paciente)
            // //      .OnDelete(DeleteBehavior.Cascade);

            // builder.HasMany(p=>p.Consultas)
            //         .WithOne(p=>p.Paciente)
                    
            //         .OnDelete(DeleteBehavior.Cascade);

            // builder.HasMany(p=>p.Dietas)
            //     .WithOne(p=>p.Paciente)
            //     .OnDelete(DeleteBehavior.Cascade);
        }
    }
}