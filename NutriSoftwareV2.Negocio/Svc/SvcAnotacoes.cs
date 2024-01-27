using NutriSoftwareV2.Negocio.Data.NutriDbContext;
using NutriSoftwareV2.Negocio.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NutriSoftwareV2.Negocio.Svc
{
    public class SvcAnotacoes
    {
        public static void CadastrarAnotacao(AnotacoesPaciente pAnotacao) 
        {
            try
            {
                using (NutriDbContext db = new NutriDbContext())
                {
                    db.AnotacosPaciente.Add(pAnotacao);
                    db.SaveChanges();
                }
            } 
            catch (Exception ex) 
            {
                throw new Exception("Erro ao cadastrar anotação.", ex);
            }
        }

        public static void EditarAnotacao(AnotacoesPaciente pAnotacao)
        {
            try
            {
                using (NutriDbContext db = new NutriDbContext())
                {
                    db.AnotacosPaciente.Update(pAnotacao);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao editar anotação.", ex);
            }
        }

        public static void DeletarAnotacao(AnotacoesPaciente pAnotacao)
        {
            try
            {
                using (NutriDbContext db = new NutriDbContext())
                {
                    db.AnotacosPaciente.Remove(pAnotacao);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao remover anotação.", ex);
            }
        }

        public static List<AnotacoesPaciente> ListarAnotacoesPaciente(int pPacienteId)
        {
            try
            {
                using (NutriDbContext db = new NutriDbContext())
                {
                    var anotacoes = db.AnotacosPaciente.Where(p => p.PacienteId == pPacienteId).ToList();
                    return anotacoes;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao cadastrar anotação.", ex);
            }
        }

        public static AnotacoesPaciente BuscarAnotacao(int pAnotacaoId)
        {
            try
            {
                using (NutriDbContext db = new NutriDbContext())
                {
                    var anotacao = db.AnotacosPaciente.Find(pAnotacaoId);
                    return anotacao;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao cadastrar anotação.", ex);
            }
        }
    }
}
