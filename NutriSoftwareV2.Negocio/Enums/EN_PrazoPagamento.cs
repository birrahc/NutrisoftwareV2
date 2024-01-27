using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Linq;

namespace NutriSoftwareV2.Negocio.Enums
{
    public enum EN_PrazoPagamento
    {
        [Description("A vista")]
        [Display(Name = "A vista")]
        Avista =1,

        [Description("Parcelado")]
        [Display(Name = "Parcelado")]
        Parcelado =2
    }
}
