using System.Collections.Generic;

namespace NutriSoftwareV2.Negocio.Domain
{
    public class Nutricionista : Pessoa
    {
        public string CRN{ get; set; }
        public ICollection<Formacao>? Formacoes { get; set; }
        public ICollection<Consulta>? Consultas { get; set; }
    }   
}