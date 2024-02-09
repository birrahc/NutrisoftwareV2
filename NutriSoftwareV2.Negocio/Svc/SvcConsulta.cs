using Microsoft.EntityFrameworkCore;
using NutriSoftwareV2.Negocio.Data.NutriDbContext;
using NutriSoftwareV2.Negocio.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NutriSoftwareV2.Negocio.Svc
{
    public class SvcConsulta
    {
        public static void SalvarConsulta(AvaliacaoFisica pAvaliacaoFisica, Consulta pConsulta)
        {
            using var context = new NutriDbContext();
            using var transaction = context.Database.BeginTransaction();

            try
            {
                pAvaliacaoFisica.Paciente = null;
                context.AvaliacoesFisicas.Add(pAvaliacaoFisica);
                context.SaveChanges();

                Consulta consulta = new Consulta
                {
                    AvaliacaoId = pAvaliacaoFisica.Id,
                    PacienteId = pAvaliacaoFisica.PacienteId,
                    Anotacoes = pConsulta.Anotacoes,
                    DataConsulta = DateTime.Now
                };
                pConsulta.Paciente = null;
                context.Consulta.Add(consulta);
                context.SaveChanges();

                transaction.Commit();

                pConsulta = consulta;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Erro ao tentar inserir dados.", ex);
            }
        }

        public static void SalvarConsulta(string CodigoDieta, AvaliacaoFisica pAvaliacaoFisica, List<QuantidadeAlimento> pQuantidadeAlimentos, List<ObservacaoPlano> pObsPlano, Consulta pConsulta = null)
        {
            using var context = new NutriDbContext();
            using var transaction = context.Database.BeginTransaction();

            try
            {
                pAvaliacaoFisica.Paciente = null;
                context.AvaliacoesFisicas.Add(pAvaliacaoFisica);
                context.SaveChanges();


                context.QuandadeAlimento.AddRange(pQuantidadeAlimentos);
                context.SaveChanges();

                context.ObservacaoPlano.AddRange(pObsPlano);
                context.SaveChanges();


                List<PlanoAlimentar> planos = new List<PlanoAlimentar>();
                var horarios = pQuantidadeAlimentos.GroupBy(p => p.Hora).ToList();
                horarios.ForEach(p =>
                {

                    var alimentosPorHora = pQuantidadeAlimentos.Where(a => a.Hora == p.Key).ToList();
                    var observacao = pObsPlano.FirstOrDefault(obs => obs.HorarioReferencia == p.Key);
                    var plano = new PlanoAlimentar { CodigoDieta = CodigoDieta, HoraAlimentos = p.Key, ObservacaoPlanoId = observacao?.Id };
                    planos.Add(plano);
                });

                context.PlanoAlimentar.AddRange(planos);
                context.SaveChanges();

                Dieta dieta = new Dieta
                {
                    CodigoDieta = CodigoDieta,
                    PacienteId = pAvaliacaoFisica.PacienteId,
                    Data = DateTime.Now
                };
                context.Dieta.Add(dieta);
                context.SaveChanges();


                Consulta consulta = new Consulta
                {
                    AvaliacaoId = pAvaliacaoFisica.Id,
                    PacienteId = pAvaliacaoFisica.PacienteId,
                    DietaId = dieta.CodigoDieta,
                    Anotacoes = pConsulta.Anotacoes,
                    DataConsulta = DateTime.Now
                };

                pConsulta.Paciente = null;
                context.Consulta.Add(consulta);
                context.SaveChanges();


                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Erro ao tentar inserir dados.", ex);
            }
        }

        public static List<Consulta> ListarConsultasPaciente(int pIdPaciente)
        {
            using (var db = new NutriDbContext())
            {
                return db.Consulta
                    .Include(p => p.Paciente)
                    .Include(a => a.Avaliacao)
                    .Include(d => d.DietaPlano)
                    .ThenInclude(i => i.PlanosAlimentares)
                    .ThenInclude(qa => qa.QuantidadeAlimentos)
                    .ThenInclude(al => al.Alimento)
                    .Include(d => d.DietaPlano)
                    .ThenInclude(pl => pl.PlanosAlimentares)
                    .ThenInclude(qa => qa.QuantidadeAlimentos)
                    .ThenInclude(tm => tm.TipoMedida)
                    .Include(d => d.DietaPlano)
                    .ThenInclude(pl => pl.PlanosAlimentares)
                    .ThenInclude(qa => qa.ObservacaoPlano)
                    .ToList();


            }
        }

        public static Consulta BuscarConsulta(int ConsultaId) 
        {
            using (var db = new NutriDbContext())
            {
                return db.Consulta
                    .Include(p => p.Paciente)
                    .Include(a => a.Avaliacao)
                    .Include(d => d.DietaPlano)
                    .ThenInclude(i => i.PlanosAlimentares)
                    .ThenInclude(qa => qa.QuantidadeAlimentos)
                    .ThenInclude(al => al.Alimento)
                    .Include(d => d.DietaPlano)
                    .ThenInclude(pl => pl.PlanosAlimentares)
                    .ThenInclude(qa => qa.QuantidadeAlimentos)
                    .ThenInclude(tm => tm.TipoMedida)
                    .Include(d => d.DietaPlano)
                    .ThenInclude(pl => pl.PlanosAlimentares)
                    .ThenInclude(qa => qa.ObservacaoPlano)
                    .FirstOrDefault(p => p.Id == ConsultaId);


            }

        }
        public static List<Consulta> LisatarDatasDeConsultas(int pIdPaciente)
        {
            using (var db = new NutriDbContext())
            {
               return db.Consulta.Include(p=>p.Paciente).Where(p => p.PacienteId == pIdPaciente).ToList();
            
            }
        }
        public static void RegistrarConsulta(string CodigoDieta, Consulta pConsulta, int? RetornoIdConsulta)
        {
            using var context = new NutriDbContext();
            using var transaction = context.Database.BeginTransaction();

            try
            {

                Consulta consulta = new Consulta();

                if (pConsulta.Avaliacao != null)
                {
                    AvaliacaoFisica avaliacao = pConsulta.Avaliacao;
                    avaliacao.Paciente = null;
                    context.AvaliacoesFisicas.Add(avaliacao);
                    context.SaveChanges();
                    consulta.AvaliacaoId = avaliacao.Id;
                }
                consulta.PacienteId = pConsulta.PacienteId;
                consulta.Anotacoes = pConsulta.Anotacoes;
                consulta.DataConsulta = DateTime.Now;

               
                if (pConsulta?.DietaPlano?.PlanosAlimentares?.Count > 0)
                {
                    List<QuantidadeAlimento> listaAlimentos = new List<QuantidadeAlimento>();
                    List<ObservacaoPlano> obsPlano = new List<ObservacaoPlano>();

                    pConsulta.DietaPlano.PlanosAlimentares.ToList()
                    .ForEach(p =>
                    {
                        listaAlimentos.AddRange(p.QuantidadeAlimentos.Where(a => a.Hora == p.HoraAlimentos && a.CodigoDieta == CodigoDieta));
                        if (!string.IsNullOrEmpty(p.ObservacaoPlano?.HorarioReferencia))
                            obsPlano.Add(new ObservacaoPlano { Anotacoes = p.ObservacaoPlano.Anotacoes, HorarioReferencia = p.HoraAlimentos, CodigoDieta = CodigoDieta });
                    });

                    listaAlimentos.ForEach(p =>
                    {
                        p.Alimento = null;
                        p.TipoMedida = null;
                        p.PlanoAlimentar = null;
                        p.Alimento = null;

                    });

                    context.QuandadeAlimento.AddRange(listaAlimentos);
                    context.SaveChanges();

                    context.ObservacaoPlano.AddRange(obsPlano);
                    context.SaveChanges();

                    List<PlanoAlimentar> planos = new List<PlanoAlimentar>();
                    var horarios = listaAlimentos.GroupBy(p => p.Hora).ToList();
                    horarios.ForEach(p =>
                    {

                        var alimentosPorHora = listaAlimentos.Where(a => a.Hora == p.Key).ToList();
                        var observacao = obsPlano.FirstOrDefault(obs => obs.HorarioReferencia == p.Key);
                        var plano = new PlanoAlimentar { CodigoDieta = CodigoDieta, HoraAlimentos = p.Key, ObservacaoPlanoId = observacao?.Id };
                        planos.Add(plano);
                    });


                    context.PlanoAlimentar.AddRange(planos);
                    context.SaveChanges();

                    Dieta dieta = new Dieta
                    {
                        CodigoDieta = CodigoDieta,
                        PacienteId = pConsulta.PacienteId,
                        Data = DateTime.Now
                    };
                    context.Dieta.Add(dieta);
                    context.SaveChanges();

                    consulta.DietaId = dieta.CodigoDieta;
                }

              

                //pConsulta.Paciente = null;
                context.Consulta.Add(consulta);
                context.SaveChanges();


                transaction.Commit();

                RetornoIdConsulta = consulta.Id;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Erro ao tentar inserir dados.", ex);
            }
        }

        public static List<Consulta> ListarConsultasSemRegistroDePagamento() 
        {
            try 
            {
                using (NutriDbContext db = new NutriDbContext()) 
                {
                   var consultas = db.Consulta
                                    .Include(p=>p.Paciente)
                                    .Where(p => !db.Pagamento.Any(pg=>pg.Id ==p.PagamentoId))
                                    .ToList();
                    db.Dispose();
                    return consultas;
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao tentar listar dados: ", ex);
            }
        }

        public static List<Consulta> ListarConsultasSemRegistroDePagamento(List<int> pIds)
        {
            try
            {
                List<Consulta> consultas = new List<Consulta>();
                using (NutriDbContext db = new NutriDbContext())
                {

                    consultas = db.Consulta
                        .Include(p => p.Paciente)
                        .Where(c => pIds.Any(id=>c.Id == id)).ToList();
                   
                    db.Dispose();
                    consultas.AddRange(ListarConsultasSemRegistroDePagamento());
                    return consultas;
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao tentar listar dados: ", ex);
            }
        }


    }
}
