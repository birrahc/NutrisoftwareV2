using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriSoftwareV2.Negocio.Domain;
using NutriSoftwareV2.Negocio.Svc;

namespace NutriSoftwareV2.Web.Controllers
{
    public class ArquivoController : Controller
    {
        public IActionResult Index(int Id)
        {
            return View("../Paciente/PartiaisPaciente/_ListaDeArquivosPaciente", SvcArquivo.ListarArquivosDoPaciente(Id));
        }

        [HttpGet]
        public async Task<IActionResult> CadastrarEditarArquivo(int Id, int? ArquivoId)
        {
            Arquivo pArquivo = null;
            if (ArquivoId.HasValue)
                pArquivo = SvcArquivo.BuscarAquivo(ArquivoId.Value);
            else
                pArquivo = new Arquivo { PacienteId = Id };

            return PartialView("PartiaisPaciente/_FormularioCadastrarArquivo", pArquivo);

        }

       

        //[HttpPost]
        //public async Task<IActionResult> Upload(List<IFormFile> arquivos, long CodigoProduto)
        //{
        //    _info = _service.GetUserInfoAsync(User.Identity.Name, false, false).Result;
        //    foreach (var arquivo in arquivos)
        //    {
        //        // Propriedades do objeto IFormFile
        //        string nomeDoArquivo = arquivo.FileName; // Nome original do arquivo
        //        string tipoDeConteudo = arquivo.ContentType; // Tipo de conteúdo MIME do arquivo
        //        long tamanhoDoArquivo = arquivo.Length; // Tamanho do arquivo em bytes

        //        // Exemplo de leitura dos dados do arquivo
        //        using (var memoryStream = new MemoryStream())
        //        {
        //            await arquivo.CopyToAsync(memoryStream);
        //            byte[] dadosDoArquivo = memoryStream.ToArray();
        //            var imgBase64 = Convert.ToBase64String(dadosDoArquivo);

        //            SvcProdutoFoto.SalvarFoto(_info.ChaveEmpresa.Value, (int)CodigoProduto, imgBase64);

        //            // Agora você pode manipular os dados do arquivo conforme necessário
        //        }
        //    }

        //    var produto = _mapper.Map<ProdutoInputModel>(ReposProduto.Buscar(_info.ChaveEmpresa.Value, CodigoProduto));
        //    var imagens = SvcProdutoFoto.ListarFotos(_info.ChaveEmpresa.Value, produto.FotosIdsStorage);

        //    return PartialView("Partiais/_ImagensProduto", imagens);
        //}

    }
}
