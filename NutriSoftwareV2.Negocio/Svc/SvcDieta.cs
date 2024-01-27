using Microsoft.EntityFrameworkCore;
using NutriSoftwareV2.Negocio.Data.NutriDbContext;
using NutriSoftwareV2.Negocio.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NutriSoftwareV2.Negocio.Svc
{
    public class SvcDieta
    {
        public static void SalvarDieta(string CodigoDieta, int PacienteId, List<QuantidadeAlimento> pQuantidadeAlimentos, List<ObservacaoPlano> pObsPlano, Consulta pConsulta=null)
        {
            using var context = new NutriDbContext();
            using var transaction = context.Database.BeginTransaction();

            try
            {
                context.QuandadeAlimento.AddRange(pQuantidadeAlimentos);
                context.SaveChanges();

                context.ObservacaoPlano.AddRange(pObsPlano);
                context.SaveChanges();


                List<PlanoAlimentar> planos = new List<PlanoAlimentar>();
                var horarios = pQuantidadeAlimentos.GroupBy(p => p.Hora).ToList();
                horarios.ForEach(p => {

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
                    PacienteId = PacienteId,
                    Data = DateTime.Now
                };
                context.Dieta.Add(dieta);
                context.SaveChanges();

                if(pConsulta!=null) 
                {
                    pConsulta.DietaId = CodigoDieta;
                    pConsulta.DataConsulta = !pConsulta.DataConsulta.HasValue ? DateTime.Now : pConsulta.DataConsulta;
                    context.Consulta.Update(pConsulta);
                    context.SaveChanges();
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Erro ao tentar inserir dados.",ex);
            }
        }
        public static List<Dieta> ListarDietas()
        {

            try
            {
                NutriDbContext db = new NutriDbContext();
                var plano = db.Dieta
                        .Include(p => p.Paciente)
                        .Include(a => a.PlanosAlimentares)
                        .ThenInclude(p => p.QuantidadeAlimentos)
                        .ThenInclude(p => p.Alimento)
                        .Include(a => a.PlanosAlimentares)
                        .ThenInclude(p => p.QuantidadeAlimentos)
                        .ThenInclude(p => p.TipoMedida)
                        .Include(a => a.PlanosAlimentares)
                        .ThenInclude(o => o.ObservacaoPlano)
                        .ToList();
                db.Dispose();
                return plano;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao tentar Listar dados.", ex);
            }
        }

        public static List<Dieta> ListarDietasPaciente(int PacienteId)
        {

            try
            {
                NutriDbContext db = new NutriDbContext();
                var plano = db.Dieta
                        .Include(p => p.Paciente)
                        .Include(a => a.PlanosAlimentares)
                        .ThenInclude(p => p.QuantidadeAlimentos)
                        .ThenInclude(p => p.Alimento)
                        .Include(a => a.PlanosAlimentares)
                        .ThenInclude(p => p.QuantidadeAlimentos)
                        .ThenInclude(p => p.TipoMedida)
                        .Include(a => a.PlanosAlimentares)
                        .ThenInclude(o => o.ObservacaoPlano)
                        .Where(p=>p.PacienteId == PacienteId)
                        .ToList();
                db.Dispose();
                return plano;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao tentar Listar dados.", ex);
            }
        }

        
    }
}
