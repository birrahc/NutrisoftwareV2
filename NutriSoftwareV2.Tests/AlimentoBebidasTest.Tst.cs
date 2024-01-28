using Microsoft.VisualStudio.TestTools.UnitTesting;
using NutriSoftwareV2.Negocio.Svc;
using System;
using System.Collections.Generic;
using System.Text;

namespace NutriSoftwareV2.UnitTest
{
    [TestClass]
    public class AlimentoBebidasTest
    {
        [TestMethod]
        public void TesteListar() 
        {
            var test = SvcAlimentoBebida.ListarAlimentos();
        }

        [TestMethod]
        public void TesteListarMedidas()
        {
            var test = SvcTipoMedida.ListarTiposDeMedida();
        }
    }
}
