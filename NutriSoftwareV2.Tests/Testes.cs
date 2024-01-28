using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NutriSoftwareV2.Negocio.Svc;
using System;
using System.Collections.Generic;
using System.Text;

namespace NutriSoftwareV2.UnitTest
{
    [TestClass]
    public class Testes
    {
        [TestMethod]
        public void teste()
        {
            var teste = SvcPaciente.testes();
        }

        [TestMethod]
        public void teste2()
        {
            Pessoa pessoa = new Pessoa();
            pessoa.Nome = "Vitinho";
            pessoa.Idade = 12;
            pessoa.Mae = "Vânia";

            Console.WriteLine($"Nome = {pessoa.Nome}");
            Console.WriteLine($"Idade = {pessoa.Idade}");
            Console.WriteLine($"Mãe = {pessoa.Mae}");

        }
        [TestMethod]
        public void teste3()
        {
            cachorro cachorro = new cachorro();
            cachorro.nome = "chibal";
            cachorro.idade = 10;
            cachorro.OqueGostaDeComer = "Racão";
            Console.WriteLine("Nome Do Cão " + cachorro.nome);
            Console.WriteLine("Idade Do Cão " + cachorro.idade);
            Console.WriteLine("Oque Gosta De Comer " + cachorro.OqueGostaDeComer);





        }



        public class Pessoa
        {
            public string Nome { get; set; }
            public int Idade { get; set; }
            public string Mae { get; set; }
        }
        public class cachorro
        {
            public string nome { get; set; }
            public int idade { get; set; }
            public string OqueGostaDeComer { get; set; }
        }
    }
}
