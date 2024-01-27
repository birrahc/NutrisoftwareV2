using NutriSoftwareV2.Negocio.Data.NutriDbContext;
using NutriSoftwareV2.Negocio.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NutriSoftwareV2.Negocio.Svc
{
    public class SvcLocaisAtendimento
    {
        public static List<LocaisAtendimento> ListarLocaisAtendimento() 
        {
            try 
            {
                using (NutriDbContext db = new NutriDbContext()) 
                {
                   var locais= db.LocalAtendimento.ToList();
                    db.Dispose();
                    return locais;
                }
            }
            catch(Exception ex) 
            {
                throw new Exception("Erro ao tentar listar dados: ", ex);
            }
        }
    }
}
