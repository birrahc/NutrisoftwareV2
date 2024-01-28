using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NutriSoftwareV2.Negocio.Domain;
using NutriSoftwareV2.Negocio.Svc;
using NutriSoftwareV2.Web.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace NutriSoftwareV2.Web.Controllers
{
    [Authorize]
    public class Paciente2Controller : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ApplicationDbContext db;
        public Paciente2Controller(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext db)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.db = db;
        }
        public ActionResult Index(int? Id, int?page)
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
            var itemsByPage = 10;
            var currentPage = page ?? 1;

            return View(pacientes.ToPagedList(currentPage,itemsByPage));
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
            return PartialView("PartiaisPaciente/_FormularioPaciente");
        }

        [HttpPost]
        public ActionResult Create(Paciente paciente)
        {
            try
            {
                if (paciente != null)
                {
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
                    SvcPaciente.AtualizarPaciente(paciente);
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
    }
}
