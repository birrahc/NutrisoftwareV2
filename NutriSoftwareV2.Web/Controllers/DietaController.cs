using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using NutriSoftwareV2.Negocio.Domain;
using NutriSoftwareV2.Negocio.Enums;
using NutriSoftwareV2.Negocio.Svc;
using NutriSoftwareV2.Web.Dto;
using NutriSoftwareV2.Web.Identity;
using NutriSoftwareV2.Web.Svc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NutriSoftwareV2.Web.Controllers
{

    [Authorize]
    public class DietaController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ApplicationDbContext db;
        public DietaController(IMemoryCache memory, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext db)
        {
            _memoryCache = memory;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.db = db;
        }
        public ActionResult Index()
        {
            var usuario = userManager.GetUserAsync(User).Result;
            var usuarioSistema = db.UsuarioSistema.FirstOrDefault(u => u.Id == usuario.UsuarioSistemaId);
            string UsuarioLogado = $"{usuarioSistema.Nome} {usuarioSistema.SobreNome}";
            ViewBag.UsuarioLogado = UsuarioLogado;

            return View();
        }

        public ActionResult DietasPaciente(int Id)
        {
            var usuario = userManager.GetUserAsync(User).Result;
            var usuarioSistema = db.UsuarioSistema.FirstOrDefault(u => u.Id == usuario.UsuarioSistemaId);
            string UsuarioLogado = $"{usuarioSistema.Nome} {usuarioSistema.SobreNome}";
            ViewBag.UsuarioLogado = UsuarioLogado;

            return View("DietasPaciente", SvcDieta.ListarDietasPaciente(Id));
        }

        public ActionResult MontarDieta(int Id)
        {
            string DietaCodigo = Guid.NewGuid().ToString();

            ViewBag.CodigoDieta = DietaCodigo;
            Dieta dieta = new Dieta { CodigoDieta = DietaCodigo, Paciente = SvcPaciente.BuscarPaciente(Id), PacienteId = Id };
            return View("CadastrarDieta", dieta);
        }

        [HttpPost]
        public ActionResult AdicionarAlimento(DtoParametrosMontarDieta pMontar)
        {
            if (JaExiste(pMontar.ConvertParamToQuantidadeAlimento(), _memoryCache))
                throw new ApplicationException("Já existe esse alimento para esse horario./");

            SvcMemoryCache.AmarzenarEntidade<QuantidadeAlimento>(pMontar.ConvertParamToQuantidadeAlimento(), _memoryCache, pMontar.CodigoDieta);

            var codigoObs = $"{pMontar.CodigoDieta}_obs";
            var alimentos = SvcMemoryCache.ListarEntidade<QuantidadeAlimento>(_memoryCache, pMontar.CodigoDieta);
            var horarios = alimentos.GroupBy(p => p.Hora).ToList();
            var observacoes = SvcMemoryCache.ListarEntidade<ObservacaoPlano>(_memoryCache, codigoObs);


            List<PlanoAlimentar> planos = new List<PlanoAlimentar>();
            horarios.ForEach(p =>
            {
                planos.Add(new PlanoAlimentar
                {
                    CodigoDieta = pMontar.CodigoDieta,
                    HoraAlimentos = p.Key,
                    QuantidadeAlimentos = alimentos.Where(h => h.Hora == p.Key).ToList(),
                    ObservacaoPlano = observacoes.FirstOrDefault(h => h.HorarioReferencia == p.Key)
                });
            });

            return PartialView("PartiaisDieta/_ListaAlimentos", planos);
        }


        [HttpPost]
        public ActionResult CadastrarObservacao(ObservacaoPlano pObs)
        {
            if (pObs != null && pObs.HorarioReferencia != null)
            {
                var codigoObs = $"{pObs.CodigoDieta}_obs";
                var Obs = SvcMemoryCache.ListarEntidade<ObservacaoPlano>(_memoryCache, codigoObs);
                if (Obs.Where(h => h.HorarioReferencia == pObs.HorarioReferencia).Any())
                {
                    SvcMemoryCache.ListarEntidade<ObservacaoPlano>(_memoryCache, codigoObs)
                        .FirstOrDefault(p => p.HorarioReferencia == pObs.HorarioReferencia).Anotacoes = pObs.Anotacoes;
                }
                else
                {
                    SvcMemoryCache.AmarzenarEntidade<ObservacaoPlano>(pObs, _memoryCache, codigoObs);
                }

                var alimentos = SvcMemoryCache.ListarEntidade<QuantidadeAlimento>(_memoryCache, pObs.CodigoDieta);
                var observacoes = SvcMemoryCache.ListarEntidade<ObservacaoPlano>(_memoryCache, codigoObs);
                var horarios = alimentos.GroupBy(p => p.Hora).ToList();

                List<PlanoAlimentar> planos = new List<PlanoAlimentar>();
                horarios.ForEach(p =>
                {
                    planos.Add(new PlanoAlimentar
                    {
                        CodigoDieta = pObs.CodigoDieta,
                        HoraAlimentos = p.Key,
                        QuantidadeAlimentos = alimentos.Where(h => h.Hora == p.Key).ToList(),
                        ObservacaoPlano = observacoes.FirstOrDefault(h => h.HorarioReferencia == p.Key)
                    });
                });
                return PartialView("PartiaisDieta/_ListaAlimentos", planos);


            }
            return PartialView("PartiaisDieta/_ListaAlimentos");
        }

        [HttpPost]
        public ActionResult RemoverItemDieta(string CodigoDieta , string Hora, int AlimentoId, EN_TipoDietaAlimentos Tipo)
        {
            var alimentos = SvcMemoryCache.ListarEntidade<QuantidadeAlimento>(_memoryCache, CodigoDieta)
                .FirstOrDefault(p=>p.CodigoDieta == CodigoDieta && p.Hora == Hora && p.AlimentoId == AlimentoId && p.Tipo == Tipo);

            SvcMemoryCache.RemoverItemEntidade<QuantidadeAlimento>(alimentos, _memoryCache, CodigoDieta);
            var codigoObs = $"{CodigoDieta}_obs";
            var ListaAlimentos = SvcMemoryCache.ListarEntidade<QuantidadeAlimento>(_memoryCache, CodigoDieta);
            var observacoes = SvcMemoryCache.ListarEntidade<ObservacaoPlano>(_memoryCache, codigoObs);
            var horarios = ListaAlimentos.GroupBy(p => p.Hora).ToList();

            List<PlanoAlimentar> planos = new List<PlanoAlimentar>();
            horarios.ForEach(p =>
            {
                planos.Add(new PlanoAlimentar
                {
                    CodigoDieta = CodigoDieta,
                    HoraAlimentos = p.Key,
                    QuantidadeAlimentos = ListaAlimentos.Where(h => h.Hora == p.Key).ToList(),
                    ObservacaoPlano = observacoes.FirstOrDefault(h => h.HorarioReferencia == p.Key)
                });
            });
            return PartialView("PartiaisDieta/_ListaAlimentos",planos);
        }


        [HttpPost]
        public ActionResult FinalizarDieta(string CodigoDieta, int? PacienteId)
        {
            var codigoObs = $"{CodigoDieta}_obs";
            var alimentos = SvcMemoryCache.ListarEntidade<QuantidadeAlimento>(_memoryCache, CodigoDieta);
            var observacoes = SvcMemoryCache.ListarEntidade<ObservacaoPlano>(_memoryCache, codigoObs);

            if (PacienteId == null)
                return View("CadastrarDieta");

            if (alimentos == null || !alimentos.Any())
                return View("CadastrarDieta");

            List<QuantidadeAlimento> alimentosParaInserir = new List<QuantidadeAlimento>();
            alimentos.ForEach(p =>
            {
                alimentosParaInserir.Add(new QuantidadeAlimento
                {
                    Hora = p.Hora,
                    CodigoDieta = CodigoDieta,
                    AlimentoId = p.AlimentoId,
                    TipoMedidaId = p.TipoMedidaId,
                    Quantidade = p.Quantidade,
                    Tipo = p.Tipo
                });

            });

            SvcDieta.SalvarDieta(CodigoDieta, PacienteId.Value, alimentosParaInserir, observacoes);
            return RedirectToAction("DietasPaciente", new { Id = PacienteId });
        }

        //AutoCompolete
        [HttpPost]
        public JsonResult AutoComplete(string busca)
        {

            var lista = SvcAlimentoBebida.ListarAlimentos();
            var alimentos = (from alimento in lista
                             where alimento.Nome.StartsWith(busca, StringComparison.OrdinalIgnoreCase)
                             select new
                             {
                                 label = alimento.Nome,
                                 val = alimento.Id
                             }).ToList();

            return Json(alimentos.Distinct());
        }

        [HttpPost]
        public JsonResult AutoCompleteTipoMedidas(string busca)
        {

            var lista = SvcTipoMedida.ListarTiposDeMedida();
            var medidas = (from medida in lista
                           where medida.Descricao.StartsWith(busca, StringComparison.OrdinalIgnoreCase)
                           select new
                           {
                               label = medida.Descricao,
                               val = medida.Id
                           }).ToList();

            return Json(medidas.Distinct());
        }




        public static bool JaExiste(QuantidadeAlimento pQuantidadeAlimento, IMemoryCache memoryCache) 
        {
            bool existe = SvcMemoryCache.ListarEntidade<QuantidadeAlimento>(memoryCache, pQuantidadeAlimento.CodigoDieta)
                .Any(p=>p.CodigoDieta == pQuantidadeAlimento.CodigoDieta &&
                          p.Hora== pQuantidadeAlimento.Hora && 
                          p.AlimentoId == pQuantidadeAlimento.AlimentoId &&
                          p.Tipo == pQuantidadeAlimento.Tipo);

            return existe;
        }
        public static List<QuantidadeAlimento> ListarMemoryQuantidadeAlimento(IMemoryCache memory, string chave)
        {
            return memory.TryGetValue(chave, out List<QuantidadeAlimento> alimentos) ?
                alimentos : new List<QuantidadeAlimento>();

        }


        public static void AmarzenarMemoryQuantidadeAlimento(QuantidadeAlimento pEntidade, IMemoryCache memory, string chave)
        {
            List<QuantidadeAlimento> Entidades = ListarMemoryQuantidadeAlimento(memory, chave);
            Entidades.Add(pEntidade);
            memory.Set(chave, Entidades);
        }



    }
}
