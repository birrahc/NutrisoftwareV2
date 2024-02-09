using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NutriSoftwareV2.Negocio.Data.NutriDbContext;
using NutriSoftwareV2.Negocio.Domain;
using NutriSoftwareV2.Negocio.Enums;
using NutriSoftwareV2.Negocio.Svc;
using NutriSoftwareV2.Web.Identity;
using NutriSoftwareV2.Web.Svc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;

namespace NutriSoftwareV2.Web.Controllers
{
    [Authorize]
    public class PacienteController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ApplicationDbContext db;
        public PacienteController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext db)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.db = db;
        }
        public ActionResult Index(int? Id)
        {
            var usuario = userManager.GetUserAsync(User).Result;
            var usuarioSistema = db.UsuarioSistema.FirstOrDefault(u => u.Id == usuario.UsuarioSistemaId);
            string UsuarioLogado = $"{usuarioSistema.Nome} {usuarioSistema.SobreNome}";
            ViewBag.UsuarioLogado = UsuarioLogado;

            List<Paciente> pacientes = SvcPaciente.ListarPacientes();
            if (Id.HasValue)
            {
                ViewBag.Paciente = SvcPaciente.BuscarPacienteCompleto(Id.Value);
            }
            return View(pacientes);
        }

        [HttpPost]
        public ActionResult PesquisaPaciente(string pesquisaPaciente)
        {
            var pesquisa = SvcPaciente.PesquisarPaciente(pesquisaPaciente);
            return PartialView("PartiaisPaciente/_ListaDePacientes", pesquisa);
        }

        [HttpGet]
        public ActionResult Create()
        {
            using (NutriDbContext db = new NutriDbContext())
            {
                ViewBag.Profissoes = ListarProfissoes(db);
            }

            return PartialView("PartiaisPaciente/_FormularioPaciente");
        }

        [HttpPost]
        public ActionResult Create(Paciente paciente)
        {
            try
            {
                if (paciente != null)
                {
                    paciente.CPF = Utils.RemoverMascara(paciente.CPF);
                    SvcPaciente.CadastrarPaciente(paciente);
                    ViewBag.Paciente = paciente;
                    var pacientes = SvcPaciente.ListarPacientes();
                    return PartialView("PartiaisPaciente/_Conteudo", pacientes);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int Id)
        {

            var paciente = SvcPaciente.BuscarPacienteCompleto(Id);
            using (NutriDbContext db = new NutriDbContext())
            {
                ViewBag.Profissoes = ListarProfissoes(db, paciente.ProfissaoId);
            }
            return PartialView("PartiaisPaciente/_FormularioPaciente", paciente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Paciente paciente)
        {
            try
            {
                if (paciente != null)
                {
                    paciente.CPF = Utils.RemoverMascara(paciente?.CPF);
                    SvcPaciente.AtualizarPaciente(paciente);

                    var pacientes = SvcPaciente.ListarPacientes();
                    ViewBag.Paciente = SvcPaciente.BuscarPacienteCompleto(paciente.Id);
                    return PartialView("PartiaisPaciente/_Conteudo", pacientes);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }



        [HttpPost]
        public ActionResult Delete(int Id)
        {
            try
            {
                SvcPaciente.DeletarPaciente(Id);
                var pacientes = SvcPaciente.ListarPacientes();
                return PartialView("PartiaisPaciente/_Conteudo", pacientes);
            }
            catch
            {
                return View();
            }
        }


        [HttpGet]
        public ActionResult CadastrarAnotacoesPaciente(int pacienteId)
        {
            ViewBag.IdPaciente = pacienteId;
            var anotacao = new AnotacoesPaciente { PacienteId = pacienteId, Data = DateTime.Now };
            return PartialView("PartiaisPaciente/_FormularioObservacaoPaciente", anotacao);
        }

        [HttpPost]
        public ActionResult CadastrarAnotacoesPaciente(AnotacoesPaciente anotacao)
        {
            SvcAnotacoes.CadastrarAnotacao(anotacao);
            var anotacoes = SvcAnotacoes.ListarAnotacoesPaciente(anotacao.PacienteId);
            return PartialView("PartiaisPaciente/_ObservacaoPaciente", anotacoes.OrderByDescending(d => d.Id));
        }

        [HttpGet]
        public ActionResult EditarAnotacoesPaciente(int anotacaoId)
        {
            var anotacao = SvcAnotacoes.BuscarAnotacao(anotacaoId);
            return PartialView("PartiaisPaciente/_FormularioObservacaoPaciente", anotacao);
        }

        [HttpPost]
        public ActionResult EditarAnotacoesPaciente(AnotacoesPaciente anotacao)
        {
            SvcAnotacoes.EditarAnotacao(anotacao);
            var anotacoes = SvcAnotacoes.ListarAnotacoesPaciente(anotacao.PacienteId);
            return PartialView("PartiaisPaciente/_ObservacaoPaciente", anotacoes.OrderByDescending(d => d.Id));
        }

        [HttpPost]
        public ActionResult DeletarAnotacao(int anotacaoId)
        {
            var anotacao = SvcAnotacoes.BuscarAnotacao(anotacaoId);
            int pacienteId = anotacao.PacienteId;
            SvcAnotacoes.DeletarAnotacao(anotacao);
            var anotacoes = SvcAnotacoes.ListarAnotacoesPaciente(pacienteId);
            return PartialView("PartiaisPaciente/_ObservacaoPaciente", anotacoes.OrderByDescending(p => p.Id));
        }


        public ActionResult ListarArquivosPaciente(int Id)
        {
            return PartialView("PartiaisPaciente/_ListaDeArquivosPaciente", SvcArquivo.ListarArquivosDoPaciente(Id));
        }

        [HttpGet]
        public ActionResult CadastrarEditarArquivo(int Id, int? ObjetoId = null)
        {
            Arquivo arquivo = new Arquivo();
            if (ObjetoId.HasValue)
            {
                arquivo = SvcArquivo.BuscarAquivo(ObjetoId.Value);
            }
            else
            {
                arquivo.PacienteId = Id;
            }
            return PartialView("PartiaisPaciente/_FormularioCadastrarArquivo", arquivo);
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarEditarArquivo(IFormFile NewFile, int PacienteId, string NomeDocumento, EN_TipoDocumentoArquivo TipoDocumento)
        {
            if (NewFile == null || NewFile.Length == 0)
                return Content("Arquivo não selecionado");
            using (NutriDbContext db = new NutriDbContext())
            {
                using (var memoryStream = new MemoryStream())
                {
                    await NewFile.CopyToAsync(memoryStream);
                    var arquivoModel = new Arquivo
                    {
                        NomeDocumento = NomeDocumento,
                        FileName = NewFile.FileName,
                        ContentType = NewFile.ContentType,
                        TamanhoArquivo = NewFile.Length,
                        TipoDocumento = TipoDocumento,
                        PacienteId = PacienteId,
                        DadosArquivo = Convert.ToBase64String(memoryStream.ToArray())
                    };

                    db.Arquivo.Add(arquivoModel);
                    await db.SaveChangesAsync();
                    return PartialView("PartiaisPaciente/_ListaDeArquivosPaciente", SvcArquivo.ListarArquivosDoPaciente(PacienteId));
                }
            }

        }

        public IActionResult RemoverArquivo(int Id,int PacienteId)
        {
            SvcArquivo.DeletarArquivo(Id);
            return PartialView("PartiaisPaciente/_ListaDeArquivosPaciente", SvcArquivo.ListarArquivosDoPaciente(PacienteId);
        }
        public async Task<IActionResult> Download(int Id)
        {

            var arquivo = SvcArquivo.BuscarAquivo(Id);

            if (arquivo == null && arquivo?.DadosArquivo?.Length < 1)
            {
                return NotFound(); // Retorna 404 se o arquivo não for encontrado
            }

            try
            {
                var result = new FileStreamResult(new MemoryStream(arquivo.ArquivoByte), arquivo.ContentType)
                {
                    FileDownloadName = arquivo.FileName
                };

                // Configuração do cabeçalho Content-Disposition para forçar o download do arquivo
                Response.Headers["Content-Disposition"] = $"attachment; filename=\"{arquivo.FileName}\"";

                return result;
            }
            catch (Exception ex) {
                throw new Exception("Erro ao tentar fazer download do Arquivo");
            }
        }

        public static SelectList ListarProfissoes(NutriDbContext db, int? Id = null)
        {
            var profissoes = db.Profissao.ToList();
            return new SelectList(profissoes, "Id", "Descricao", Id);
        }
    }
}
