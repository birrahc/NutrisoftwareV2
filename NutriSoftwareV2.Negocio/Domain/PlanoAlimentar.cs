using NutriSoftwareV2.Negocio.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace NutriSoftwareV2.Negocio.Domain
{
    public class PlanoAlimentar
    {
        //public int PacienteId { get; set; }
        public string CodigoDieta { get; set; }
        public string HoraAlimentos { get; set; }
        public int ? ObservacaoPlanoId { get; set; }
        public virtual ICollection<QuantidadeAlimento> QuantidadeAlimentos { get; set; }
        public virtual ObservacaoPlano ObservacaoPlano { get; set; }

     
    }
}
