using System;
using System.Collections.Generic;
using System.Text;

namespace NutriSoftwareV2.Negocio.Dto
{
    public class ParametrosPesquisaEntreAvaliacoes
    {
        public int PacienteId { get; set; }
        public int ? PrimeiroParamPesquisa { get; set; }
        public int ? SegundoParamPesquisa { get; set; }
    }
}
