using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace NutriSoftwareV2.Negocio.Enums
{
    public enum EN_TipoDeContatoTelefone
    {

        [Display(Name = "Somente liga��es")]
        [Description("Somente liga��es")]
        SomenteLigacoes =1,


        [Display(Name = "Somente WhatsApp")]
        [Description("Somente WhatsApp")]
        SomenteWhatsApp =2,


        [Display(Name = "Liga��es e WhatsApp")]
        [Description("Liga��es e WhatsApp")]
        LigacoesEWhatsApp =3
    }
}