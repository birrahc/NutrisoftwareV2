using NutriSoftwareV2.Negocio.Data.NutriDbContext;
using NutriSoftwareV2.Negocio.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NutriSoftwareV2.Negocio.Svc
{
    public class SvcAlimentoBebida
    {
        public static void CadastrarAlimento(AlimentoBebida pAlimento)
        {
            try
            {
                NutriDbContext db = new NutriDbContext();
                db.AlimentoBebidas.Add(pAlimento);
                db.SaveChanges();
                db.Dispose();
            }
            catch (Exception ex) 
            {
                throw new Exception("Erro ao cadastrar alimento", ex);
            }
        }
        public static void EditarAlimento(AlimentoBebida pAlimento)
        {
            try
            {
                NutriDbContext db = new NutriDbContext();
                db.AlimentoBebidas.Update(pAlimento);
                db.SaveChanges();
                db.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao editar alimento", ex);
            }
        }
        public static void DeletarAlimento(AlimentoBebida pAlimento)
        {
            try
            {
                NutriDbContext db = new NutriDbContext();
                db.AlimentoBebidas.Remove(pAlimento);
                db.SaveChanges();
                db.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao remover alimento", ex);
            }
        }
        public static List<AlimentoBebida> ListarAlimentos()
        {
            try
            {
                NutriDbContext db = new NutriDbContext();
                var alimentos = db.AlimentoBebidas.ToList();
                db.Dispose();
                return alimentos;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar alimentos", ex);
            }
        }

        public static List<string> BuscaAlimentos(string pAlimentoNome)
        {
            return ListarAlimentos()
                .Where(p => p.Nome.StartsWith(pAlimentoNome,StringComparison.OrdinalIgnoreCase))
                .Select(p=>p.Nome)
                .ToList();
        }

        public static AlimentoBebida BuscarAlimento(int pAlimentoId)
        {
            try
            {
                NutriDbContext db = new NutriDbContext();
                var alimento = db.AlimentoBebidas.Find(pAlimentoId);
                db.Dispose();
                return alimento;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar alimento", ex);
            }
        }
    }
}
