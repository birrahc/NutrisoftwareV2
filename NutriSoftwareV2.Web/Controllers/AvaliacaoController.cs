using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NutriSoftwareV2.Negocio.Svc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NutriSoftwareV2.Negocio;
using NutriSoftwareV2.Negocio.Domain;
using NutriSoftwareV2.Negocio.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using NutriSoftwareV2.Web.Identity;

namespace NutriSoftwareV2.Web.Controllers
{
    [Authorize]
    public class AvaliacaoController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ApplicationDbContext db;
        public AvaliacaoController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext db)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.db = db;
        }
        // GET: AvaliacaoController
        public ActionResult Index()
        {
            
            var usuario = userManager.GetUserAsync(User).Result;
            var usuarioSistema = db.UsuarioSistema.FirstOrDefault(u => u.Id == usuario.UsuarioSistemaId);
            string UsuarioLogado = $"{usuarioSistema.Nome} {usuarioSistema.SobreNome}";
            ViewBag.UsuarioLogado = UsuarioLogado;

            return View();
        }

        // GET: AvaliacaoController/Details/5
        [HttpGet]
        public ActionResult AvaliacoesDoPaciente(int Id)
        {
            var usuario = userManager.GetUserAsync(User).Result;
            var usuarioSistema = db.UsuarioSistema.FirstOrDefault(u => u.Id == usuario.UsuarioSistemaId);
            string UsuarioLogado = $"{usuarioSistema.Nome} {usuarioSistema.SobreNome}";
            ViewBag.UsuarioLogado = UsuarioLogado;

            List<AvaliacaoFisica> pAvaliacoes  = SvcAvaliacao.ListarAvaliacoesPorPaciente(Id);
            return View("AvaliacoesPaciente", pAvaliacoes);
        }

        [HttpGet]
        public ActionResult ListarEntreAvaliacoes(ParametrosPesquisaEntreAvaliacoes pPesquisa)
        {
            if (!pPesquisa.PrimeiroParamPesquisa.HasValue && !pPesquisa.SegundoParamPesquisa.HasValue)
                return PartialView("PartiaisAvaliacao/_ConteudoAvaliacao", SvcAvaliacao.ListarAvaliacoesPorPaciente(pPesquisa.PacienteId));
            var entreAvaliacoes = SvcAvaliacao.ListarEntreAvaliacoesPaciente(pPesquisa);
            return PartialView("PartiaisAvaliacao/_ConteudoAvaliacao", entreAvaliacoes);
        }

        [HttpGet]
        public ActionResult CadastraAvaliacaoPaciente(int pacienteId)
        {
            ViewBag.IdPaciente = pacienteId;
            ViewBag.AvaliacaoAnterior = SvcAvaliacao.ListarAvaliacoesPorPaciente(pacienteId).OrderBy(p => p.DataAvaliacao).LastOrDefault();
            return PartialView("PartiaisAvaliacao/_FomularioAvaliacao");
        }

        [HttpPost]
        public ActionResult CadastraAvaliacaoPaciente(int IdPaciente, AvaliacaoFisica avaliacao)
        {
            if (avaliacao != null && avaliacao.PacienteId > 0)
            {
                SvcAvaliacao.CadastrarAvaliacao(avaliacao);
                List<AvaliacaoFisica> AvaliacoesPaciente = SvcAvaliacao.ListarAvaliacoesPorPaciente(avaliacao.PacienteId);
                return PartialView("PartiaisAvaliacao/_ConteudoAvaliacao", AvaliacoesPaciente);
            }

            return PartialView("PartiaisAvaliacao/_ConteudoAvaliacao");
        }

        [HttpGet]
        public ActionResult EditarAvaliacaoPaciente(int Id)
        {

            AvaliacaoFisica avaliacao = SvcAvaliacao.BuscarAvalicao(Id);
            return PartialView("PartiaisAvaliacao/_FomularioAvaliacao", avaliacao);
        }

        [HttpPost]
        public ActionResult EditarAvaliacaoPaciente(AvaliacaoFisica avaliacao)
        {
            if (avaliacao != null && avaliacao.PacienteId > 0)
            {
                SvcAvaliacao.EditarAvaliacao(avaliacao);
                List<AvaliacaoFisica> Avaliacoes = SvcAvaliacao.ListarAvaliacoesPorPaciente(avaliacao.PacienteId);
                return PartialView("PartiaisAvaliacao/_ConteudoAvaliacao", Avaliacoes);
            }

            return PartialView("PartiaisAvaliacao/_ConteudoAvaliacao");
        }

        [HttpPost]
        public ActionResult DeletarAvaliacao(int avaliacaoId, int pacienteId)
        {
            SvcAvaliacao.DeletarAvaliacao(avaliacaoId);
            return PartialView("PartiaisAvaliacao/_ConteudoAvaliacao", SvcAvaliacao.ListarAvaliacoesPorPaciente(pacienteId));
        }




        // GET: AvaliacaoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AvaliacaoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AvaliacaoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AvaliacaoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AvaliacaoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AvaliacaoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
