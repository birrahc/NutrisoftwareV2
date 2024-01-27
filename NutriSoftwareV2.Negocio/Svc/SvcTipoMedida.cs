using NutriSoftwareV2.Negocio.Data.NutriDbContext;
using NutriSoftwareV2.Negocio.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NutriSoftwareV2.Negocio.Svc
{
    public class SvcTipoMedida
    {
        public static void CadastrarTipoMedida(TipoMedida pTipoMedida)
        {
            try 
            {
                NutriDbContext db = new NutriDbContext();
                db.TipoMedidas.Add(pTipoMedida);
                db.SaveChanges();
                db.Dispose();
            } 
            catch (Exception ex) 
            {
                throw new Exception("Erro ao cadastrar tipo ", ex);
            }
        }

        public static void EditarTipoMedida(TipoMedida pTipoMedida)
        {
            try
            {
                NutriDbContext db = new NutriDbContext();
                db.TipoMedidas.Update(pTipoMedida);
                db.SaveChanges();
                db.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao editar tipo ", ex);
            }
        }

        public static void DeletarTipoMedida(TipoMedida pTipoMedida)
        {
            try
            {
                NutriDbContext db = new NutriDbContext();
                db.TipoMedidas.Remove(pTipoMedida);
                db.SaveChanges();
                db.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao remover tipo ", ex);
            }
        }

        public static List<TipoMedida> ListarTiposDeMedida()
        {
            try
            {
                NutriDbContext db = new NutriDbContext();
                var tipoMedidas = db.TipoMedidas.ToList();
                db.Dispose();
                return tipoMedidas;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao cadastrar tipo ", ex);
            }
        }

        public static TipoMedida BuscarTipoDeMedida(int pTipoMedidaId)
        {
            try
            {
                NutriDbContext db = new NutriDbContext();
                var tipoMedida = db.TipoMedidas.Find(pTipoMedidaId);
                db.Dispose();
                return tipoMedida;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao cadastrar tipo ", ex);
            }
        }
    }
}
