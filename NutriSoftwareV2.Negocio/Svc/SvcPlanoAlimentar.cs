using Microsoft.EntityFrameworkCore;
using NutriSoftwareV2.Negocio.Data.NutriDbContext;
using NutriSoftwareV2.Negocio.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NutriSoftwareV2.Negocio.Svc
{
    public class SvcPlanoAlimentar
    {
        public static void InserirAoPlano(PlanoAlimentar pPlano) 
        {
            try 
            {
                NutriDbContext db = new NutriDbContext();
                db.PlanoAlimentar.Add(pPlano);
                db.SaveChanges();
                db.Dispose();
            } 
            catch (Exception ex) 
            {
                throw new Exception("Erro ao tentar inserir dados.");
            }
        }
        public static void InserirAoPlano(List<PlanoAlimentar> pPlanos)
        {
            try
            {
                NutriDbContext db = new NutriDbContext();
                db.PlanoAlimentar.AddRange(pPlanos);
                db.SaveChanges();
                db.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao tentar inserir dados.");
            }
        }
        
        public static List<PlanoAlimentar> ListarPlanosAlimentares()
        {

            try 
            {
                NutriDbContext db = new NutriDbContext();
                var plano = db.PlanoAlimentar
                        .Include(a => a.QuantidadeAlimentos)
                        .ThenInclude(p=>p.Alimento)
                        .Include(p=>p.QuantidadeAlimentos)
                        .ThenInclude(p=>p.TipoMedida)
                        .Include(o => o.ObservacaoPlano)
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
