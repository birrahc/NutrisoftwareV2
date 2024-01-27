using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NutriSoftwareV2.Negocio.Enums
{
    public enum EN_TipoAlimentoBebida
    {
        [Description("Comida")]
        [Display(Name ="Comida")]
        Comida = 1,
        [Description("Bebida")]
        [Display(Name="Bebida")]
        Bebida = 2
    }
}
