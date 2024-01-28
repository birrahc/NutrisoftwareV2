using Microsoft.VisualStudio.TestTools.UnitTesting;
using NutriSoftwareV2.Negocio.Svc;
using System;
using System.Collections.Generic;
using System.Text;

namespace NutriSoftwareV2.UnitTest
{
    [TestClass]
    public class AnocoesTest
    {
        [TestMethod]
        public void TestListarAnocoes() 
        {
            var anotacoes = SvcAnotacoes.ListarAnotacoesPaciente(191);
        }
    }
}
