using NutriSoftwareV2.Negocio.Svc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutriSoftwareV2.Tests
{
    [TestClass]
    public class PagamentoTest
    {
        [TestMethod]
        public void TesteBuscarConsulta()
        {
           var x = SvcPagamento.BuscarPagamento(3558);
        }
    }
}
