using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NutriSoftwareV2.Negocio.Enums
{
    public enum EN_TipoPlano
    {
        [Description("Consulta normal")]
        [Display(Name = "Consulta normal")]
        ConsultaNormal =1,

        [Description("pacote")]
        [Display(Name ="pacote")]
        Pacote = 2,

        [Description("Gratuito")]
        [Display(Name ="Gratuito")]
        Gratuito = 3,

        [Description("Convênio")]
        [Display(Name = "Convênio")]
        Convenio = 4,
    }
}
