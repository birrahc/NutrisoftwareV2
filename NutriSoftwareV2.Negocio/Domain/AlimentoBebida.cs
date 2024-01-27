using NutriSoftwareV2.Negocio.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriSoftwareV2.Negocio.Domain
{
    public class AlimentoBebida
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public EN_TipoAlimentoBebida Tipo { get; set; }
        public string DescricaoTipoCaloria { get => Svc.SvcEnum.GetDescription(this.Tipo); }
        public float? Calorias { get; set; }


    }
}