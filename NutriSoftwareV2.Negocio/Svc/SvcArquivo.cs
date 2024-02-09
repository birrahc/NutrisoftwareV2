using Microsoft.EntityFrameworkCore;
using NutriSoftwareV2.Negocio.Data.NutriDbContext;
using NutriSoftwareV2.Negocio.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutriSoftwareV2.Negocio.Svc
{
    public class SvcArquivo
    {
        public static void CadastrarArquivo(Arquivo pArquivo)
        {
            using (NutriDbContext db = new NutriDbContext())
            {
                db.Arquivo.Add(pArquivo);
                db.SaveChanges();
            }
        }

        public static void EditarArquivo(Arquivo pArquivo)
        {
            using (NutriDbContext db = new NutriDbContext())
            {
                db.Arquivo.Update(pArquivo);
                db.SaveChanges();
            }
        }

        public static void EditarArquivo(int pArquivoId)
        {
            using (NutriDbContext db = new NutriDbContext())
            {
                var arquivo = db.Arquivo.Find(pArquivoId);
                db.Arquivo.Remove(arquivo);
                db.SaveChanges();
            }
        }

        public static Arquivo? BuscarAquivo(int pArquivoId)
        {
            using (NutriDbContext db = new NutriDbContext())
            {
                return db.Arquivo.Include(p => p.Paciente).FirstOrDefault(p => p.Id == pArquivoId);
            }
        }

        public static Paciente ListarArquivosDoPaciente(int pPacienteId)
        {
            using (NutriDbContext db = new NutriDbContext())
            {
                return db.pacientes.Include(p => p.Arquivos).FirstOrDefault(p => p.Id == pPacienteId);
            }
        }

        public static void DeletarArquivo(int pIdArquivo)
        {
            using (NutriDbContext db = new NutriDbContext())
            {
                var arquivo= db.Arquivo.Find(pIdArquivo);
                if (arquivo != null)
                {
                    db.Arquivo.Remove(arquivo);
                    db.SaveChanges();
                }
            }
        }

    }
}
