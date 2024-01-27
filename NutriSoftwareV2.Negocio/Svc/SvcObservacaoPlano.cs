using NutriSoftwareV2.Negocio.Data.NutriDbContext;
using NutriSoftwareV2.Negocio.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NutriSoftwareV2.Negocio.Svc
{
    public class SvcObservacaoPlano
    {
        public static void CadastrarObservacaoPlano(ObservacaoPlano pObsPlano)
        {
            try 
            {
                NutriDbContext db = new NutriDbContext();
                db.ObservacaoPlano.Add(pObsPlano);
                db.SaveChanges();
                db.Dispose();
            } 
            catch (Exception ex) 
            {
                throw new Exception("Erro ao tentar inserir dados.", ex);
            }
        }
        public static void CadastrarObservacaoPlano(List<ObservacaoPlano> pObsPlanos)
        {
            try
            {
                NutriDbContext db = new NutriDbContext();
                db.ObservacaoPlano.AddRange(pObsPlanos);
                db.SaveChanges();
                db.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao tentar inserir dados.", ex);
            }
        }
        public static void AtualizarObservacaoPlano(ObservacaoPlano pObsPlano)
        {
            try
            {
                NutriDbContext db = new NutriDbContext();
                db.ObservacaoPlano.Update(pObsPlano);
                db.SaveChanges();
                db.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao tentar inserir dados.", ex);
            }
        }
        public static void AtualizarObservacaoPlano(List<ObservacaoPlano> pObsPlanos)
        {
            try
            {
                NutriDbContext db = new NutriDbContext();
                db.ObservacaoPlano.UpdateRange(pObsPlanos);
                db.SaveChanges();
                db.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao tentar inserir dados.", ex);
            }
        }

        public static void DeletarObservacaoPlano(ObservacaoPlano pObsPlano)
        {
            try
            {
                NutriDbContext db = new NutriDbContext();
                db.ObservacaoPlano.Remove(pObsPlano);
                db.SaveChanges();
                db.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao tentar inserir dados.", ex);
            }
        }
        public static void DeletarObservacaoPlano(List<ObservacaoPlano> pObsPlanos)
        {
            try
            {
                NutriDbContext db = new NutriDbContext();
                db.ObservacaoPlano.RemoveRange(pObsPlanos);
                db.SaveChanges();
                db.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao tentar inserir dados.", ex);
            }
        }

        public static List<ObservacaoPlano> ListarObservacoesPlano(string pCodigoDieta) 
        {
            try 
            {
                List<ObservacaoPlano> Observacoes = new List<ObservacaoPlano>();
                NutriDbContext db = new NutriDbContext();
                Observacoes = db.ObservacaoPlano.Where(p => p.CodigoDieta == pCodigoDieta).ToList();
                db.Dispose();
                return Observacoes;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao tentar buscar dados.",ex);
            }
        }


    }
}
