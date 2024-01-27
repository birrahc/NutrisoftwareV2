using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NutriSoftwareV2.Negocio.Enums
{
    public enum EN_TipoConsulta
    {
        Consulta = 1,
        Retorno = 2,
        [Description("Consulta (pacote)")]
        [Display(Name ="Consulta (pacote)")]
        ConsultaPacote =3,
        [Description("Retorno (pacote)")]
        [Display(Name = "Retorno (pacote)")]
        RetornoPacote =4,
        Desafio =5
    }
}