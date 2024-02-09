using System.Collections.Generic;
using System;
namespace NutriSoftwareV2.Negocio.Domain
{
    public class Paciente : Pessoa
    {
        public int? ProfissaoId { get; set; }
        public Profissao? Profissao { get; set; }
        public ICollection<AnotacoesPaciente>?Anotacoes { get; set; }
        public ICollection<Consulta>? Consultas{ get; set; }
        public ICollection<Dieta>? Dietas{ get; set; }
        public ICollection<AvaliacaoFisica>?AvaliacoesFisicas{ get; set; }
        public ICollection<Arquivo>? Arquivos { get; set; }
       
        //public  Anamnese Anamnese { get; set; }
    }
}