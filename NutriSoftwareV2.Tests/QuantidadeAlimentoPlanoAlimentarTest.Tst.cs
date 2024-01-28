using Microsoft.VisualStudio.TestTools.UnitTesting;
using NutriSoftwareV2.Negocio.Domain;
using NutriSoftwareV2.Negocio.Enums;
using NutriSoftwareV2.Negocio.Svc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NutriSoftwareV2.UnitTest
{
    [TestClass]
    public class QuantidadeAlimentoPlanoAlimentarTest
    {
        [TestMethod]
        public void testInserirPlanoAlimentar()
        {
            //var teste = SvcPlanoAlimentar.ListarPlanosAlimentaresPaciente(2);

            string codigo = Guid.NewGuid().ToString();
            List<QuantidadeAlimento> alimentos = new List<QuantidadeAlimento>()
            {
                new QuantidadeAlimento{Hora="10:00", CodigoDieta = codigo, AlimentoId = 1, TipoMedidaId = 1 , Quantidade =2, Tipo = NutriSoftwareV2.Negocio.Enums.EN_TipoDietaAlimentos.Alimento},
                new QuantidadeAlimento{Hora="10:00", CodigoDieta = codigo, AlimentoId = 2, TipoMedidaId = 3, Quantidade = 2, Tipo = NutriSoftwareV2.Negocio.Enums.EN_TipoDietaAlimentos.Alimento},
                new QuantidadeAlimento{Hora="10:00", CodigoDieta = codigo, AlimentoId = 3, TipoMedidaId = 2, Quantidade = 3,Tipo = NutriSoftwareV2.Negocio.Enums.EN_TipoDietaAlimentos.Alimento},
                new QuantidadeAlimento{Hora="10:00",CodigoDieta = codigo, AlimentoId = 4, TipoMedidaId = 1, Quantidade = 4, Tipo = NutriSoftwareV2.Negocio.Enums.EN_TipoDietaAlimentos.Substituicao },
                new QuantidadeAlimento{Hora="10:00",CodigoDieta = codigo, AlimentoId = 5, TipoMedidaId = 4, Quantidade = 5, Tipo = NutriSoftwareV2.Negocio.Enums.EN_TipoDietaAlimentos.Substituicao },
                new QuantidadeAlimento{Hora="10:00",CodigoDieta = codigo, AlimentoId = 6, TipoMedidaId = 2, Quantidade = 6, Tipo = NutriSoftwareV2.Negocio.Enums.EN_TipoDietaAlimentos.Substituicao },

                new QuantidadeAlimento{Hora="10:00",CodigoDieta = codigo, AlimentoId = 9, TipoMedidaId = 4, Quantidade = 5, Tipo = NutriSoftwareV2.Negocio.Enums.EN_TipoDietaAlimentos.Intervalo },
                new QuantidadeAlimento{Hora="10:00",CodigoDieta = codigo, AlimentoId = 1, TipoMedidaId = 2, Quantidade = 6, Tipo = NutriSoftwareV2.Negocio.Enums.EN_TipoDietaAlimentos.Intervalo },

                new QuantidadeAlimento{Hora="12:00", CodigoDieta = codigo, AlimentoId = 9, TipoMedidaId = 1 , Quantidade =2, Tipo = NutriSoftwareV2.Negocio.Enums.EN_TipoDietaAlimentos.Alimento},
                new QuantidadeAlimento{Hora="12:00", CodigoDieta = codigo, AlimentoId = 8, TipoMedidaId = 3, Quantidade = 2, Tipo = NutriSoftwareV2.Negocio.Enums.EN_TipoDietaAlimentos.Alimento},
                new QuantidadeAlimento{Hora="12:00", CodigoDieta = codigo, AlimentoId = 7, TipoMedidaId = 2, Quantidade = 3,Tipo = NutriSoftwareV2.Negocio.Enums.EN_TipoDietaAlimentos.Alimento},
                new QuantidadeAlimento{Hora="12:00",CodigoDieta = codigo, AlimentoId = 6, TipoMedidaId = 1, Quantidade = 4, Tipo = NutriSoftwareV2.Negocio.Enums.EN_TipoDietaAlimentos.Alimento },
                new QuantidadeAlimento{Hora="12:00",CodigoDieta = codigo, AlimentoId = 5, TipoMedidaId = 4, Quantidade = 5, Tipo = NutriSoftwareV2.Negocio.Enums.EN_TipoDietaAlimentos.Substituicao },
                new QuantidadeAlimento{Hora="12:00",CodigoDieta = codigo, AlimentoId = 4, TipoMedidaId = 2, Quantidade = 6, Tipo = NutriSoftwareV2.Negocio.Enums.EN_TipoDietaAlimentos.Substituicao },

                new QuantidadeAlimento{Hora="12:00",CodigoDieta = codigo, AlimentoId = 2, TipoMedidaId = 4, Quantidade = 5, Tipo = NutriSoftwareV2.Negocio.Enums.EN_TipoDietaAlimentos.Intervalo },
                new QuantidadeAlimento{Hora="12:00",CodigoDieta = codigo, AlimentoId = 1, TipoMedidaId = 2, Quantidade = 6, Tipo = NutriSoftwareV2.Negocio.Enums.EN_TipoDietaAlimentos.Intervalo },

                new QuantidadeAlimento{Hora="16:00", CodigoDieta = codigo, AlimentoId = 4, TipoMedidaId = 1 , Quantidade =2, Tipo = NutriSoftwareV2.Negocio.Enums.EN_TipoDietaAlimentos.Alimento},
                new QuantidadeAlimento{Hora="16:00", CodigoDieta = codigo, AlimentoId = 9, TipoMedidaId = 3, Quantidade = 2, Tipo = NutriSoftwareV2.Negocio.Enums.EN_TipoDietaAlimentos.Alimento},
                new QuantidadeAlimento{Hora="16:00", CodigoDieta = codigo, AlimentoId = 5, TipoMedidaId = 2, Quantidade = 3,Tipo = NutriSoftwareV2.Negocio.Enums.EN_TipoDietaAlimentos.Alimento},
                new QuantidadeAlimento{Hora="16:00",CodigoDieta = codigo, AlimentoId = 7, TipoMedidaId = 1, Quantidade = 4, Tipo = NutriSoftwareV2.Negocio.Enums.EN_TipoDietaAlimentos.Substituicao },
                new QuantidadeAlimento{Hora="16:00",CodigoDieta = codigo, AlimentoId = 3, TipoMedidaId = 4, Quantidade = 5, Tipo = NutriSoftwareV2.Negocio.Enums.EN_TipoDietaAlimentos.Substituicao },
                new QuantidadeAlimento{Hora="16:00",CodigoDieta = codigo, AlimentoId = 1, TipoMedidaId = 2, Quantidade = 6, Tipo = NutriSoftwareV2.Negocio.Enums.EN_TipoDietaAlimentos.Substituicao },


                new QuantidadeAlimento{Hora="16:00", CodigoDieta = codigo, AlimentoId = 9, TipoMedidaId = 1 , Quantidade =2, Tipo = NutriSoftwareV2.Negocio.Enums.EN_TipoDietaAlimentos.Intervalo},
                new QuantidadeAlimento{Hora="16:00", CodigoDieta = codigo, AlimentoId = 4, TipoMedidaId = 3, Quantidade = 2, Tipo = NutriSoftwareV2.Negocio.Enums.EN_TipoDietaAlimentos.Intervalo},


                new QuantidadeAlimento{Hora="20:00", CodigoDieta = codigo, AlimentoId = 8, TipoMedidaId = 2, Quantidade = 3,Tipo = NutriSoftwareV2.Negocio.Enums.EN_TipoDietaAlimentos.Alimento},
                new QuantidadeAlimento{Hora="20:00",CodigoDieta = codigo, AlimentoId = 1, TipoMedidaId = 1, Quantidade = 4, Tipo = NutriSoftwareV2.Negocio.Enums.EN_TipoDietaAlimentos.Alimento },
                new QuantidadeAlimento{Hora="20:00",CodigoDieta = codigo, AlimentoId = 2, TipoMedidaId = 4, Quantidade = 5, Tipo = NutriSoftwareV2.Negocio.Enums.EN_TipoDietaAlimentos.Alimento },
                new QuantidadeAlimento{Hora="20:00",CodigoDieta = codigo, AlimentoId = 5, TipoMedidaId = 2, Quantidade = 6, Tipo = NutriSoftwareV2.Negocio.Enums.EN_TipoDietaAlimentos.Substituicao },

                new QuantidadeAlimento{Hora="20:00", CodigoDieta = codigo, AlimentoId = 6, TipoMedidaId = 3, Quantidade = 2, Tipo = NutriSoftwareV2.Negocio.Enums.EN_TipoDietaAlimentos.Intervalo},

            };



            List<ObservacaoPlano> observacaos = new List<ObservacaoPlano>()
            {
                new ObservacaoPlano{ CodigoDieta = codigo, HorarioReferencia="10:00", Anotacoes="dieta das 10"},
                new ObservacaoPlano{ CodigoDieta = codigo, HorarioReferencia="12:00", Anotacoes="dieta das 12"}
            };

            SvcDieta.SalvarDieta(codigo, 2, alimentos, observacaos);



        }

        [TestMethod]
        public void TestBuscarQuantidadeAlimento()
        {
            var xxx = SvcQuantidadeAlimento.BuscarQuantidadeAlimento("32ee81e2-2da7-46b9-b6a6-46ebe2692a64", "08:00", 52, EN_TipoDietaAlimentos.Substituicao);
        }
    }
}
