using NutriSoftwareV2.Negocio.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace NutriSoftwareV2.Negocio.Domain
{
    public class Pagamento
    {
        public int Id { get; set; }
        public DateTime? DataPagamento { get; set; }
        public EN_TipoAtendimento? TipoAtendimento { get; set; }
        public EN_TipoPlano? TipoPlano { get; set; }
        public decimal? Valor { get; set; }
        public EN_FormaPagamento? FormaPagamento { get; set; }
        public int? NumeroParcelas { get; set; }
        public decimal? ValorParcelas { get; set; }
        public decimal? Desconto { get; set; }
        public EN_StatusPagamento? Situacao { get; set; }
        public string? Observacao { get; set; }
        public EN_PrazoPagamento? PrazoPagamento { get; set; }

        [NotMapped]
        public List<int>? ConsultasId { get; set; }
        public virtual ICollection<Consulta>? Consulta { get; set; }

        public int? LocaisAtendimentoId { get; set; }
        public virtual LocaisAtendimento? LocaisAtendimento { get; set; }

    }
}