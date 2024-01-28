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
using System.Threading;
using System.Threading.Tasks;

namespace NutriSoftwareV2.Web.Controllers
{
    [Authorize]
    public class ConsultaController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ApplicationDbContext db;
        public ConsultaController(IMemoryCache memory, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext db)
        {
            _memoryCache = memory;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.db = db;
        }


        public ActionResult Index(int Id, int? ConsultaId)
        {
            var usuario = userManager.GetUserAsync(User).Result;
            var usuarioSistema = db.UsuarioSistema.FirstOrDefault(u => u.Id == usuario.UsuarioSistemaId);
            string UsuarioLogado = $"{usuarioSistema.Nome} {usuarioSistema.SobreNome}";
            ViewBag.UsuarioLogado = UsuarioLogado;

            if (ConsultaId.HasValue)
                ViewBag.ConsultaDetalhada = SvcConsulta.BuscarConsulta(ConsultaId.Value);

            return View("Index", SvcPaciente.BuscarPacienteCompleto(Id));
        }
        public ActionResult CadastrarConsulta(int Id)
        {
            ViewBag.Codigo = Guid.NewGuid().ToString();
            ViewBag.Avaliacoes = null;
            return View("CadastarConsulta", new Consulta { PacienteId = Id, Paciente = SvcPaciente.BuscarPacienteCompleto(Id) });
        }



        [HttpPost]
        public ActionResult PrimeiroPassoObservacoes(int PacienteId, string Anotacoes, string Codigo, bool Editar = false)
        {
            var paciente = SvcPaciente.BuscarPaciente(PacienteId);
            var avaliacoesPaciente = SvcAvaliacao.ListarAvaliacoesPorPaciente(PacienteId);
            var ultimaAvaliacao = avaliacoesPaciente.Any() ? avaliacoesPaciente.Select(p => p.NumAvaliacao).Max() + 1 : 1;
            if (Editar)
                SvcMemoryCache.BuscarEntidade<Consulta>(_memoryCache, Codigo).Anotacoes = Anotacoes;
            else
            {
                var avalicaoPaciente = new AvaliacaoFisica { PacienteId = PacienteId, Paciente = paciente, NumAvaliacao = ultimaAvaliacao, DataAvaliacao = DateTime.Now };
                var consulta = new Consulta { Anotacoes = Anotacoes, PacienteId = PacienteId, Paciente = paciente, Avaliacao = avalicaoPaciente };

                var x = SvcMemoryCache.BuscarEntidade<Consulta>(_memoryCache, Codigo);

                if (x != null && x.Avaliacao != null)
                    SvcMemoryCache.BuscarEntidade<Consulta>(_memoryCache, Codigo).Anotacoes = Anotacoes;
                else
                    SvcMemoryCache.AmarzenarEntidadeUnica<Consulta>(consulta, _memoryCache, Codigo);

            }

            ViewBag.AvaliacaoAnterior = avaliacoesPaciente.LastOrDefault();
            ViewBag.CodigoConsulta = Codigo;
            ViewBag.Codigo = Codigo;
            ViewBag.Editar = Editar;
            return PartialView("PartiaisConsulta/_ConteudoConsulta", SvcMemoryCache.BuscarEntidade<Consulta>(_memoryCache, Codigo));
        }

        [HttpPost]
        public ActionResult CalcularAvaliacao(AvaliacaoFisica pAvaliacao, string Codigo, bool Editar = false)
        {
            pAvaliacao.Paciente = SvcPaciente.BuscarPaciente(pAvaliacao.PacienteId);
            var avaliacoesPaciente = SvcAvaliacao.ListarAvaliacoesPorPaciente(pAvaliacao.PacienteId);
            ViewBag.AvaliacaoAnterior = avaliacoesPaciente.LastOrDefault();
            ViewBag.CodigoConsulta = Codigo;
            ViewBag.Codigo = Codigo;
            ViewBag.Editar = Editar;
            ViewBag.PacienteId = pAvaliacao.PacienteId;
            ViewBag.CodigoConsulta = Codigo;
            ViewBag.Codigo = Codigo;
            pAvaliacao.DataAvaliacao = DateTime.Now;


            var paciente = SvcPaciente.BuscarPaciente(pAvaliacao.PacienteId);
            pAvaliacao.Paciente = paciente;
            SvcMemoryCache.BuscarEntidade<Consulta>(_memoryCache, Codigo).Paciente = paciente;
            SvcMemoryCache.BuscarEntidade<Consulta>(_memoryCache, Codigo).Avaliacao = pAvaliacao;

            if (!Editar)
            {
                var ultimaDietaDoPaciente = SvcDieta.ListarDietasPaciente(pAvaliacao.PacienteId)?.OrderBy(d => d.Data).LastOrDefault();
                if (ultimaDietaDoPaciente != null)
                {

                    ultimaDietaDoPaciente.CodigoDieta = Codigo;
                    ultimaDietaDoPaciente.PlanosAlimentares.ToList().ForEach(p =>
                    {

                        var codigoPlano = $"{Codigo}_plano";
                        var codigoObs = $"{Codigo}_obs";
                        p.CodigoDieta = Codigo;
                        p.ObservacaoPlano.CodigoDieta = Codigo;

                        var quantidadeAlimentos = p.QuantidadeAlimentos.Where(h => h.Hora == p.HoraAlimentos).ToList();

                        if (quantidadeAlimentos.Any())
                        {
                            quantidadeAlimentos.ForEach(c =>
                            {
                                c.CodigoDieta = Codigo;
                            });
                            //SvcMemoryCache.ArmazenarRangeEntidade<QuantidadeAlimento>(quantidadeAlimentos, _memoryCache, codigoPlano);
                        }

                        if (p.ObservacaoPlano != null)
                        {
                            p.ObservacaoPlano.CodigoDieta = Codigo;
                            //SvcMemoryCache.AmarzenarEntidade<ObservacaoPlano>(p.ObservacaoPlano, _memoryCache, codigoObs);
                        }

                    });
                }

                SvcMemoryCache.BuscarEntidade<Consulta>(_memoryCache, Codigo).Dieta = ultimaDietaDoPaciente != null ? ultimaDietaDoPaciente : new Dieta()
                {
                    CodigoDieta = Codigo,
                    Paciente = paciente,
                    PacienteId = pAvaliacao.PacienteId
                };
            }
            ViewBag.Editar = Editar;
            ViewBag.CodigoDieta = Codigo;
            ViewBag.Codigo = Codigo;

            return PartialView("PartiaisConsulta/PartiaisAvaliacao/_FomularioAvaliacao", SvcMemoryCache.BuscarEntidade<Consulta>(_memoryCache, Codigo).Avaliacao);
        }

        [HttpPost]
        public ActionResult SegundoPassoAvaliacao(AvaliacaoFisica pAvaliacao, string Codigo, bool Editar = false)
        {
            var avaliacoesPaciente = SvcAvaliacao.ListarAvaliacoesPorPaciente(pAvaliacao.PacienteId);
            ViewBag.AvaliacaoAnterior = avaliacoesPaciente.LastOrDefault();
            ViewBag.CodigoConsulta = Codigo;
            ViewBag.Codigo = Codigo;
            pAvaliacao.DataAvaliacao = DateTime.Now;


            var paciente = SvcPaciente.BuscarPaciente(pAvaliacao.PacienteId);
            pAvaliacao.Paciente = paciente;
            SvcMemoryCache.BuscarEntidade<Consulta>(_memoryCache, Codigo).Paciente = paciente;
            SvcMemoryCache.BuscarEntidade<Consulta>(_memoryCache, Codigo).Avaliacao = pAvaliacao;

            if (!Editar)
            {
                var ultimaDietaDoPaciente = SvcDieta.ListarDietasPaciente(pAvaliacao.PacienteId)?.OrderBy(d => d.Data).LastOrDefault();
                if (ultimaDietaDoPaciente != null)
                {

                    ultimaDietaDoPaciente.CodigoDieta = Codigo;
                    ultimaDietaDoPaciente.PlanosAlimentares.ToList().ForEach(p =>
                    {

                        var codigoPlano = $"{Codigo}_plano";
                        var codigoObs = $"{Codigo}_obs";
                        p.CodigoDieta = Codigo;
                        p.ObservacaoPlano.CodigoDieta = Codigo;

                        var quantidadeAlimentos = p.QuantidadeAlimentos.Where(h => h.Hora == p.HoraAlimentos).ToList();

                        if (quantidadeAlimentos.Any())
                        {
                            quantidadeAlimentos.ForEach(c =>
                            {
                                c.CodigoDieta = Codigo;
                            });
                            //SvcMemoryCache.ArmazenarRangeEntidade<QuantidadeAlimento>(quantidadeAlimentos, _memoryCache, codigoPlano);
                        }

                        if (p.ObservacaoPlano != null)
                        {
                            p.ObservacaoPlano.CodigoDieta = Codigo;
                            //SvcMemoryCache.AmarzenarEntidade<ObservacaoPlano>(p.ObservacaoPlano, _memoryCache, codigoObs);
                        }

                    });
                }
                var consulta = SvcMemoryCache.BuscarEntidade<Consulta>(_memoryCache, Codigo);
                if (consulta ==null||consulta?.Dieta ==null || consulta.Dieta.PlanosAlimentares ==null|| consulta?.Dieta?.PlanosAlimentares?.Count < 1)
                    SvcMemoryCache.BuscarEntidade<Consulta>(_memoryCache, Codigo).Dieta = ultimaDietaDoPaciente != null ? ultimaDietaDoPaciente : new Dieta()
                    {
                        CodigoDieta = Codigo,
                        Paciente = paciente,
                        PacienteId = pAvaliacao.PacienteId
                    };
            }
            ViewBag.Editar = Editar;
            ViewBag.CodigoDieta = Codigo;
            ViewBag.Codigo = Codigo;
            return PartialView("PartiaisConsulta/_ConteudoConsulta", SvcMemoryCache.BuscarEntidade<Consulta>(_memoryCache, Codigo));

        }

        [HttpPost]
        public ActionResult TerceiroPassoDieta(int PacienteId, string Codigo)
        {
            SvcMemoryCache.BuscarEntidade<Consulta>(_memoryCache, Codigo).DietaId = Codigo;

            var avaliacoesPaciente = SvcAvaliacao.ListarAvaliacoesPorPaciente(PacienteId);
            ViewBag.AvaliacaoAnterior = avaliacoesPaciente.LastOrDefault();
            ViewBag.Codigo = Codigo;
            ViewBag.CodigoConsulta = Codigo;
            ViewBag.CodigoDieta = Codigo;
            ViewBag.Editar = true;
            return PartialView("PartiaisConsulta/_ConteudoConsulta", SvcMemoryCache.BuscarEntidade<Consulta>(_memoryCache, Codigo));

        }


        [HttpPost]
        public ActionResult PassoAnterior(string Codigo, int Passo, bool Editar = false)
        {

            if (Passo == 3)
            {
                var consulta = SvcMemoryCache.BuscarEntidade<Consulta>(_memoryCache, Codigo);
                var avaliacoesPaciente = SvcAvaliacao.ListarAvaliacoesPorPaciente(consulta.Paciente.Id);
                var ultimaAvaliacao = avaliacoesPaciente.Any() ? avaliacoesPaciente.Select(p => p.NumAvaliacao).Max() + 1 : 1;
                var avalicaoPaciente = new AvaliacaoFisica { PacienteId = consulta.Paciente.Id, Paciente = consulta.Paciente, NumAvaliacao = ultimaAvaliacao, DataAvaliacao = DateTime.Now };
                ViewBag.AvaliacaoAnterior = avaliacoesPaciente.LastOrDefault();

            }
            ViewBag.Codigo = Codigo;
            ViewBag.CodigoConsulta = Codigo;
            ViewBag.CodigoDieta = Codigo;
            ViewBag.Editar = Editar;
            return View("PartiaisConsulta/_ConteudoConsulta", SvcMemoryCache.BuscarEntidade<Consulta>(_memoryCache, Codigo));

        }

        [HttpPost]
        public ActionResult FinalizarConsulta(string Codigo, int? PacienteId)
        {
            int idCosnulta = 0;
            SvcConsulta.RegistrarConsulta(Codigo, SvcMemoryCache.BuscarEntidade<Consulta>(_memoryCache, Codigo), idCosnulta);
            return RedirectToAction("Index", new { Id = PacienteId, ConsultaId = idCosnulta });
        }

        //Montagem da dieta
        [HttpPost]
        public ActionResult AdicionarAlimento(DtoParametrosMontarDieta pMontar)
        {
            if (JaExiste(pMontar.ConvertParamToQuantidadeAlimento(), _memoryCache))
                throw new ApplicationException("Já existe esse alimento para esse horario./");

            List<ObservacaoPlano> obsPlano = new List<ObservacaoPlano>();
            List<QuantidadeAlimento> listaAlimentos = new List<QuantidadeAlimento>();
            var consulta = SvcMemoryCache.BuscarEntidade<Consulta>(_memoryCache, pMontar.CodigoDieta);
            if (consulta?.Dieta?.PlanosAlimentares?.Count > 0)
            {
                consulta.Dieta.PlanosAlimentares.ToList()
                .ForEach(pl =>
                {

                    listaAlimentos.AddRange(pl.QuantidadeAlimentos.Where(qa => qa.Hora == pl.HoraAlimentos && qa.CodigoDieta == pl.CodigoDieta));
                    if (!string.IsNullOrEmpty(pl.ObservacaoPlano?.HorarioReferencia))
                        obsPlano.Add(pl.ObservacaoPlano);
                });
            }


            listaAlimentos.Add(pMontar.ConvertParamToQuantidadeAlimento());

            var horaSAlimento = listaAlimentos.GroupBy(p => p.Hora).ToList();
            List<PlanoAlimentar> planosAlimentos = new List<PlanoAlimentar>();
            horaSAlimento.ForEach(p =>
            {
                var alimentos = listaAlimentos.Where(h => h.Hora == p.Key).ToList();
                planosAlimentos.Add(new PlanoAlimentar
                {
                    CodigoDieta = pMontar.CodigoDieta,
                    HoraAlimentos = p.Key,
                    QuantidadeAlimentos = listaAlimentos.Where(h => h.Hora == p.Key).ToList(),
                    ObservacaoPlano = obsPlano?.FirstOrDefault(h => h?.HorarioReferencia == p.Key)
                });
            });
            ViewBag.Codigo = pMontar.CodigoDieta;
            ViewBag.CodigoDieta = pMontar.CodigoDieta;
            ViewBag.Editar = pMontar.Editar;
            SvcMemoryCache.BuscarEntidade<Consulta>(_memoryCache, pMontar.CodigoDieta).Dieta.PlanosAlimentares = planosAlimentos.ToList();
            return PartialView("PartiaisConsulta/PartiaisDieta/_ListaAlimentos", planosAlimentos);
        }

        [HttpPost]
        public ActionResult CadastrarObservacao(ObservacaoPlano pObs, bool Editar)
        {
            if (pObs != null && pObs.HorarioReferencia != null)
            {

                //Teste 
                List<ObservacaoPlano> obsPlano = new List<ObservacaoPlano>();
                List<QuantidadeAlimento> listaAlimentos = new List<QuantidadeAlimento>();
                var consulta = SvcMemoryCache.BuscarEntidade<Consulta>(_memoryCache, pObs.CodigoDieta);
                if (consulta?.Dieta?.PlanosAlimentares?.Count > 0)
                {
                    consulta.Dieta.PlanosAlimentares.ToList()
                   .ForEach(pl =>
                   {

                       listaAlimentos.AddRange(pl.QuantidadeAlimentos.Where(qa => qa.Hora == pl.HoraAlimentos && qa.CodigoDieta == pl.CodigoDieta));
                       if (!string.IsNullOrEmpty(pl.ObservacaoPlano?.HorarioReferencia))
                           obsPlano.Add(pl.ObservacaoPlano);
                   });
                }


                if (obsPlano.Where(h => h.HorarioReferencia == pObs.HorarioReferencia).Any())
                    obsPlano.FirstOrDefault(p => p.HorarioReferencia == pObs.HorarioReferencia).Anotacoes = pObs.Anotacoes;
                else
                    obsPlano.Add(pObs);

                obsPlano.Add(pObs);

                var horaSAlimento = listaAlimentos.GroupBy(p => p.Hora).ToList();

                List<PlanoAlimentar> planosAlimentos = new List<PlanoAlimentar>();
                horaSAlimento.ForEach(p =>
                {
                    planosAlimentos.Add(new PlanoAlimentar
                    {
                        CodigoDieta = pObs.CodigoDieta,
                        HoraAlimentos = p.Key,
                        QuantidadeAlimentos = listaAlimentos.Where(h => h.Hora == p.Key).ToList(),
                        ObservacaoPlano = obsPlano.FirstOrDefault(h => h.HorarioReferencia == p.Key)
                    });
                });
                ViewBag.Codigo =pObs.CodigoDieta; 
                ViewBag.Editar = Editar;

                SvcMemoryCache.BuscarEntidade<Consulta>(_memoryCache, pObs.CodigoDieta).Dieta.PlanosAlimentares = planosAlimentos.ToList();
                return PartialView("PartiaisConsulta/PartiaisDieta/_ListaAlimentos", planosAlimentos);

            }
            return PartialView("PartiaisConsulta/PartiaisDieta/_ListaAlimentos");
        }

        [HttpPost]
        public ActionResult RemoverItemDieta(string CodigoDieta, string Hora, int AlimentoId, EN_TipoDietaAlimentos Tipo, bool Editar)
        {
            List<ObservacaoPlano> obsPlano = new List<ObservacaoPlano>();
            List<QuantidadeAlimento> listaAlimentos = new List<QuantidadeAlimento>();
            var consulta = SvcMemoryCache.BuscarEntidade<Consulta>(_memoryCache, CodigoDieta);

            if (consulta?.Dieta?.PlanosAlimentares?.Count > 0)
            {
                consulta.Dieta.PlanosAlimentares.ToList()
                .ForEach(pl =>
                {

                    listaAlimentos.AddRange(pl.QuantidadeAlimentos.Where(qa => qa.Hora == pl.HoraAlimentos && qa.CodigoDieta == pl.CodigoDieta));
                    if (!string.IsNullOrEmpty(pl.ObservacaoPlano?.HorarioReferencia))
                        obsPlano.Add(pl.ObservacaoPlano);
                });
            }
            var itemParaRemover = listaAlimentos.FirstOrDefault(p => p.CodigoDieta == CodigoDieta && p.Hora == Hora && p.AlimentoId == AlimentoId && p.Tipo == Tipo);

            listaAlimentos.Remove(itemParaRemover);

            var horaSAlimento = listaAlimentos.GroupBy(p => p.Hora).ToList();
            List<PlanoAlimentar> planosAlimentos = new List<PlanoAlimentar>();
            horaSAlimento.ForEach(p =>
            {
                var alimentos = listaAlimentos.Where(h => h.Hora == p.Key).ToList();
                planosAlimentos.Add(new PlanoAlimentar
                {
                    CodigoDieta = CodigoDieta,
                    HoraAlimentos = p.Key,
                    QuantidadeAlimentos = listaAlimentos.Where(h => h.Hora == p.Key).ToList(),
                    ObservacaoPlano = obsPlano?.FirstOrDefault(h => h?.HorarioReferencia == p.Key)
                });
            });
            ViewBag.Codigo = CodigoDieta;
            ViewBag.Editar = Editar;
            SvcMemoryCache.BuscarEntidade<Consulta>(_memoryCache, CodigoDieta).Dieta.PlanosAlimentares = planosAlimentos.ToList();

            return PartialView("PartiaisConsulta/PartiaisDieta/_ListaAlimentos", planosAlimentos);
        }

        public static bool JaExiste(QuantidadeAlimento pQuantidadeAlimento, IMemoryCache memoryCache)
        {

            //test

            List<ObservacaoPlano> obsPlano = new List<ObservacaoPlano>();
            List<QuantidadeAlimento> listaAlimentos = new List<QuantidadeAlimento>();
            var consulta = SvcMemoryCache.BuscarEntidade<Consulta>(memoryCache, pQuantidadeAlimento.CodigoDieta);

            if (consulta?.Dieta?.PlanosAlimentares?.Count > 0)
            {
                consulta?.Dieta?.PlanosAlimentares.ToList()
                    .ForEach(pl =>
                    {

                        listaAlimentos.AddRange(pl.QuantidadeAlimentos.Where(qa => qa.Hora == pl.HoraAlimentos && qa.CodigoDieta == pl.CodigoDieta));
                        if (!string.IsNullOrEmpty(pl.ObservacaoPlano?.HorarioReferencia))
                            obsPlano.Add(pl.ObservacaoPlano);
                    });
            }



            //
            bool jaExiste = listaAlimentos
                             .Any(p => p.CodigoDieta == pQuantidadeAlimento.CodigoDieta &&
                                      p.Hora == pQuantidadeAlimento.Hora &&
                                      p.AlimentoId == pQuantidadeAlimento.AlimentoId &&
                                      p.Tipo == pQuantidadeAlimento.Tipo);
            return jaExiste;


            /*bool existe = SvcMemoryCache.ListarEntidade<QuantidadeAlimento>(memoryCache, pQuantidadeAlimento.CodigoDieta)
                .Any(p => p.CodigoDieta == pQuantidadeAlimento.CodigoDieta &&
                          p.Hora == pQuantidadeAlimento.Hora &&
                          p.AlimentoId == pQuantidadeAlimento.AlimentoId &&
                          p.Tipo == pQuantidadeAlimento.Tipo);

            return existe;*/
        }
    }
}
