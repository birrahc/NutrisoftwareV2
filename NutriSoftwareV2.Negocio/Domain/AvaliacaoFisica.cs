using NutriSoftwareV2.Negocio.Svc;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriSoftwareV2.Negocio.Domain
{
    public class AvaliacaoFisica
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public int NumAvaliacao { get; set; }
        public DateTime DataAvaliacao { get; set; }
        public double? Peso { get; set; }
        public double? CircCintura { get; set; }
        public double? CircAbdominal { get; set; }
        public double? CircQuadril { get; set; }
        public double? CircPeito { get; set; }
        public double? CircBracoDireito { get; set; }
        public double? CircBracoEsquerdo { get; set; }
        public double? CircCoxadireita { get; set; }
        public double? CircCoxaEsquerda { get; set; }
        public double? CircPanturrilhaDireita { get; set; }
        public double? CircPanturrilhaEsquerda { get; set; }
        public double? DCTriceps { get; set; }
        public double? DCEscapular { get; set; }
        public double? DCSupraIliaca { get; set; }
        public double? DCAbdominal { get; set; }
        public double? DCAxilar { get; set; }
        public double? DCPeitoral { get; set; }
        public double? DCCoxa { get; set; }
        public Paciente? Paciente { get; set; }

        [NotMapped]
        private double? SomatoriaDc
        {
            get
            {
                if (this.DCTriceps.HasValue &&
                      this.DCEscapular.HasValue &&
                      this.DCSupraIliaca.HasValue &&
                      this.DCAbdominal.HasValue &&
                      this.DCAxilar.HasValue &&
                      this.DCPeitoral.HasValue &&
                      this.DCCoxa.HasValue)
                {
                   return Convert.ToDouble((
                        this.DCTriceps +
                        this.DCEscapular +
                        this.DCSupraIliaca +
                        this.DCAbdominal +
                        this.DCAxilar +
                        this.DCPeitoral +
                        this.DCCoxa
                        ));
                    
                }
                return null;
            }
        }

       
        [NotMapped]
        private double ? Densidade {
            get { 
                if(this.SomatoriaDc.HasValue && this.Paciente !=null && this.Paciente.Sexo.HasValue && this.Paciente?.Idade > 0)
                    return SvcAvaliacao.CalculularDensidade(this.SomatoriaDc.Value, this.Paciente.Sexo.Value, this.Paciente.Idade).Value;
                return null;
            }
        }

        [NotMapped]
        public double ? PercentualGordura { 
            get {
                if (this.Densidade.HasValue)
                    return (double)SvcAvaliacao.CalcularPercentualGordura(this.Densidade.Value);
                return null;
            } 
        }
        [NotMapped]
        public double ? MassaMuscular {
            get {
                if(this.PercentualGordura.HasValue && this.Peso.HasValue)
                    return SvcAvaliacao.CalcularMassaMuscular(this.PercentualGordura.Value, this.Peso.Value);
                return null;
            }
        }
        [NotMapped]
        public double ? Gordura { 
            get {
                if(this.PercentualGordura.HasValue && this.Peso.HasValue)
                    return (double)SvcAvaliacao.CalcularGordura(this.PercentualGordura.Value, this.Peso.Value);
                return null;
            }
        }

        public bool IsValid() 
        {
            return (this.Gordura.HasValue && this.MassaMuscular.HasValue && PercentualGordura.HasValue);
        }
        

    }
}