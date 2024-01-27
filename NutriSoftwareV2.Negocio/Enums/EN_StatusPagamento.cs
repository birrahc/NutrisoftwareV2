using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Linq;

namespace NutriSoftwareV2.Negocio.Enums
{
    public enum EN_StatusPagamento
    {
        [Description("Pago parcial")]
        [Display(Name = "A Pagar")]
        APagar =1,

        Pago=2,

        [Description("Pago parcial")]
        [Display(Name = "Pago parcial")]
        PagoParcial =3,

        Gratuito =4,

    }
}
