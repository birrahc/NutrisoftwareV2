using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NutriSoftwareV2.Negocio.Enums
{
    public enum EN_FormaPagamento
    {
        Dinheiro=1,

        [Description("Cartão de débito")]
        [Display(Name ="Cartão de débito")]
        CartaoDebito=2,

        [Description("Cartão de crédito")]
        [Display(Name = "Cartão de crédito")]
        CartaoCredito =3,

        Cheque=4
    }
}
