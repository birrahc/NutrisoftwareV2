using System;
using System.Collections.Generic;
using System.Text;

namespace NutriSoftwareV2.Negocio.Domain
{
    public class Anamnese
    {
        public int PacienteId { get; set; }
        public Paciente Paciente { get; set; }
    }
}
