
using System;
using System.ComponentModel.DataAnnotations.Schema;
using NutriSoftwareV2.Negocio.Enums;
using NutriSoftwareV2.Negocio.Svc;

namespace NutriSoftwareV2.Negocio.Domain
{
    public class Pessoa
    {
        public int Id { get; set; }
        public string?  Nome { get; set; }
        public EN_Sexo? Sexo{ get; set; }
        public DateTime ? Nascimento { get; set; }
        public string? CPF { get; set; }

        [NotMapped]
        public int ? Idade { 
            get{
                if (Nascimento.HasValue)
                {
                    int idade = DateTime.Now.Year - this.Nascimento.Value.Year;
                    if (DateTime.Now.DayOfYear < this.Nascimento.Value.DayOfYear)
                        idade = idade - 1;
                    return idade;
                }
                return null;
            } 
        }
        public float ? Altura { get; set; }
       
        public string?  Email { get; set; }
        public string ?  Telefone { get; set; }
        public EN_TipoDeContatoTelefone ? TipoDeContatoTelefone { get; set; }

        [NotMapped]
        public string? TipoTelefoneGetDescription { get => this.TipoDeContatoTelefone.HasValue? SvcEnum.GetDescription(this.TipoDeContatoTelefone) :null; }

    }
}