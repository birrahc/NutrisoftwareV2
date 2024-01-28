using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutriSoftwareV2.Negocio.Domain
{
    public class Profissao
    {
        public int Id { get; set; }
        public string Descricao { get; set; }

        public List<Paciente>? PacientesProfissao { get; set; }
    }
}
