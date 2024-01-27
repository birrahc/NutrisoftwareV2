using NutriSoftwareV2.Negocio.Data.NutriDbContext;
using Microsoft.EntityFrameworkCore;
using NutriSoftwareV2.Negocio.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NutriSoftwareV2.Negocio.Svc
{
    public class SvcPaciente
    {
        public static void CadastrarPaciente(Paciente pPaciente)
        {
            try
            {
                using (NutriDbContext db = new NutriDbContext())
                {
                    db.pacientes.Add(pPaciente);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao cadastrar paciente", ex);
            }
        }

        public static void AtualizarPaciente(Paciente pPaciente)
        {
            try
            {
                using (NutriDbContext db = new NutriDbContext())
                {
                    db.pacientes.Update(pPaciente);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Atualizar paciente", ex);
            }
        }

        public static void DeletarPaciente(int pIdPaciente)
        {
            try
            {
                using (NutriDbContext db = new NutriDbContext())
                {
                    var paciente = BuscarPacienteCompleto(pIdPaciente);
                    db.Remove(paciente);
                    db.SaveChanges();
                }
            }
            catch(Exception ex) 
            {
                throw new Exception("Erro ao remover paciente", ex);
            }
        }
        public static Paciente BuscarPacienteCompleto(int pacienteId) 
        {
            using (NutriDbContext db = new NutriDbContext())
            {
                Paciente paciente = db.pacientes.Include(c=>c.Consultas)
                                                .ThenInclude(p=>p.Dieta)
                                                .ThenInclude(p=>p.PlanosAlimentares)
                                                .ThenInclude(q=>q.QuantidadeAlimentos)
                                                .ThenInclude(a=>a.Alimento)
                                                .Include(c => c.Consultas)
                                                .ThenInclude(p => p.Dieta)
                                                .ThenInclude(p => p.PlanosAlimentares)
                                                .ThenInclude(q => q.QuantidadeAlimentos)
                                                .ThenInclude(a => a.TipoMedida)
                                                .Include(t=>t.AvaliacoesFisicas)
                                                .Include(a => a.Anotacoes)
                                                .FirstOrDefault(p=>p.Id==pacienteId);
                return paciente;
            }
        }

        public static List<Paciente> ListarPacientes() 
        {
            try
            {
                using (NutriDbContext db = new NutriDbContext())
                {
                    List<Paciente> pacientes = db.pacientes.OrderBy(n=>n.Nome).ToList();
                    return pacientes;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar pacientes", ex);
            }
        }

        public static Paciente BuscarPaciente(int pIdPaciente) 
        {
            try
            {
                using (NutriDbContext db = new NutriDbContext())
                {
                    Paciente paciente = db.pacientes.Find(pIdPaciente);
                    return paciente;
                }
            }
            catch (Exception ex) 
            {
                throw new Exception("Erro ao buscar paciente", ex);
            }
        }
        //public static Paciente BuscarPacienteCompleto(int pIdPaciente) 
        //{
        //    try
        //    {
        //        using (NutriDbContext db = new NutriDbContext())
        //        {
        //            Paciente paciente = db.pacientes
        //                .Include(p => p.Anotacoes)
        //                .FirstOrDefault(p => p.Id == pIdPaciente);

        //            /*.Include(a => a.Avaliacoes)
        //            .Include(p=>p.Pagamentos)
        //            .Include(p=>p.HorariosAgendados)
        //            .Include(p=>p.Consultas).ThenInclude(n=>n.Nutricionista)
        //            .Include(p=>p.Anotacoes)
        //            .FirstOrDefault(p => p.Id == pIdPaciente);*/

        //            return paciente;
        //        }
        //    }
        //    catch (Exception ex) 
        //    {
        //        throw new Exception("Erro ao buscar paciente", ex);
        //    }
        //}

        public static List<Paciente> PesquisarPaciente(string pNome) 
        {

            using (NutriDbContext db = new NutriDbContext())
            {

                var query = from e in db.pacientes
                            where EF.Functions.Like(e.Nome, $"%{pNome}%")
                            select e;
                return query.ToList();
            }
        }
        public static List<Paciente> testes() 
        {
            using (NutriDbContext db = new NutriDbContext())
            {
                var teste = db.pacientes.FromSqlRaw("Select * from pacientes").ToList();
                return teste;
            }
                
                //FromSql("Select * from Pacientes").ToLis();

        }

       
    }
}
