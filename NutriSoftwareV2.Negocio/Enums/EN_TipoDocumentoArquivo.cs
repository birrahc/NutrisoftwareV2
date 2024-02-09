using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutriSoftwareV2.Negocio.Enums
{
    public enum EN_TipoDocumentoArquivo
    {
        [Description("Plano Alimentar")]
        [Display(Name = "Plano Alimentar")]
        PlanoAlimentar = 1,

        Imagem = 2,

        Receita = 3

    }
}
