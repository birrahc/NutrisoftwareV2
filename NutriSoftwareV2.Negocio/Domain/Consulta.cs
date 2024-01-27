using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriSoftwareV2.Negocio.Domain
{
    public class Consulta
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public DateTime? DataConsulta { get; set; }
        public string Anotacoes { get; set; }
        public int ? AvaliacaoId { get; set; }
        public string DietaId { get; set; }
        //public Nutricionista Nutricionista { get; set;}
        public Paciente Paciente { get; set; }
        public AvaliacaoFisica Avaliacao { get; set; }
        public Dieta Dieta { get; set; }
        public int? PagamentoId { get; set; }
        public Pagamento Pagamento { get; set; }
        [NotMapped]
        public string Descricao { get=>$"{DataConsulta} - {Paciente?.Nome}"; }


    }
}