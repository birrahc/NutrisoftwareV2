using NutriSoftwareV2.Negocio.Domain;
using NutriSoftwareV2.Negocio.Enums;
using NutriSoftwareV2.Negocio.Svc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NutriSoftwareV2.Web.Dto
{
    public class DtoParametrosMontarDieta
    {
        public string CodigoDieta { get; set; }
        public string Hora { get; set; }
        public int AlimentoId { get; set; }
        public EN_TipoDietaAlimentos Tipo { get; set; }
        public float QuantidadeAlimento { get; set; }
        public int TipoMedidaAlimentoId { get; set; }
        public bool Editar { get; set; }

        public QuantidadeAlimento ConvertParamToQuantidadeAlimento() 
        {
            var pQuantidadeAlimento = new QuantidadeAlimento
            {
                CodigoDieta = this.CodigoDieta,
                Hora = this.Hora,
                AlimentoId = this.AlimentoId,
                Quantidade = this.QuantidadeAlimento,
                TipoMedidaId = this.TipoMedidaAlimentoId,
                Tipo = this.Tipo,
                Alimento = SvcAlimentoBebida.BuscarAlimento(this.AlimentoId),
                TipoMedida = SvcTipoMedida.BuscarTipoDeMedida(this.TipoMedidaAlimentoId)
                
            };

            return pQuantidadeAlimento;
        }



    }
}
