using NutriSoftwareV2.Negocio.Data.NutriDbContext;
using NutriSoftwareV2.Negocio.Domain;
using NutriSoftwareV2.Negocio.Svc;
using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutriSoftwareV2.Tests
{
    [TestClass] 
    public class ArquivoTest
    {
        [TestMethod]
        public void testBuscarArquivo() {
            using (NutriDbContext db = new NutriDbContext()) {

              var teste =   db.Arquivo.ToList();
            }
        }

        [TestMethod]
        public void teste()
        {
       
            string imagePath = "D:/Boleto-Carline-Moesch (2).pdf";

            if (File.Exists(imagePath))
            {
                try
                {
                    byte[] imageData = File.ReadAllBytes(imagePath);

                   
                    if (imageData.Length > 0)
                    {
                        Console.WriteLine("Imagem convertida em array de bytes com sucesso!");

                        Arquivo arquivo = new Arquivo
                        {
                            NomeDocumento = "Teste",
                            FileName = "Boleto-Carline-Moesch (2).pdf",
                            TipoDocumento = Negocio.Enums.EN_TipoDocumentoArquivo.PlanoAlimentar,
                            ContentType = "application/pdf",
                            TamanhoArquivo = imageData.Length,
                            //File = imageData,
                            PacienteId = 2

                        };
                        SvcArquivo.CadastrarArquivo(arquivo);
                    }
                    else
                    {
                        Console.WriteLine("O arquivo de imagem está vazio.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ocorreu um erro ao ler a imagem: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("O arquivo de imagem não foi encontrado.");
            }
        }
    }
}
