using Microsoft.EntityFrameworkCore;
using NutriSoftwareV2.Negocio.Data.NutriDbContext;
using NutriSoftwareV2.Negocio.Domain;
using NutriSoftwareV2.Negocio.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NutriSoftwareV2.Negocio.Svc
{
    public class SvcQuantidadeAlimento
    {
        public static void InserirQuantidadeAlimento(QuantidadeAlimento pQuantidadeAlimento) 
        {
            try 
            {
                NutriDbContext db = new NutriDbContext();
                db.QuandadeAlimento.Add(pQuantidadeAlimento);
                db.SaveChanges();
                db.Dispose();
            }
            catch(Exception ex) 
            {
                throw new Exception("Erro ao tentar inserir dados.");
            }

        }

        public static void InserirQuantidadeAlimento(List<QuantidadeAlimento> pQuantidadeAlimentos)
        {
            try
            {
                NutriDbContext db = new NutriDbContext();
                db.QuandadeAlimento.AddRange(pQuantidadeAlimentos);
                db.SaveChanges();
                db.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao tentar inserir dados.");
            }

        }

        public static void AtualizarQuantidadeAlimento(QuantidadeAlimento pQuantidadeAlimento)
        {
            try
            {
                NutriDbContext db = new NutriDbContext();
                db.QuandadeAlimento.Update(pQuantidadeAlimento);
                db.SaveChanges();
                db.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao tentar atualizar dados.");
            }

        }

        public static List<QuantidadeAlimento> ListarQuantidadeAlimento(QuantidadeAlimento pQuantidadeAlimento)
        {
            try
            {
                NutriDbContext db = new NutriDbContext();
                var lista = db.QuandadeAlimento.ToList();
                db.Dispose();
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao tentar listar dados.");
            }

        }
        public static void RemoverQuantidadeAlimento(QuantidadeAlimento pQuantidadeAlimento)
        {
            try
            {
                NutriDbContext db = new NutriDbContext();
                db.QuandadeAlimento.Remove(pQuantidadeAlimento);
                db.SaveChanges();
                db.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao tentar remover dados.");
            }

        }

        public static QuantidadeAlimento BuscarQuantidadeAlimento(string CodigoDieta, string Hora, int AlimentoId, EN_TipoDietaAlimentos Tipo) 
        {
            NutriDbContext db = new NutriDbContext();
            QuantidadeAlimento quantidadeAlimento = db.QuandadeAlimento
                                                    .Include(p => p.Alimento)
                                                    .Include(p => p.TipoMedida)
                                                    .FirstOrDefault(p => p.CodigoDieta == CodigoDieta && p.Hora == Hora && p.AlimentoId == AlimentoId && p.Tipo == Tipo);
            db.Dispose();
            return quantidadeAlimento;
        }




    }
}
