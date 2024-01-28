using System;

namespace NutriSoftwareV2.Negocio.Domain
{
    public class AnotacoesPaciente
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public DateTime ? Data { get; set; }
        public string Titulo{get;set;}
        public string Anotacoes { get; set; }
        public Paciente? Paciente { get; set; }

        public AnotacoesPaciente()
        {
            this.Data = this.Data ==null ? DateTime.Now : this.Data;
        }

    }
}