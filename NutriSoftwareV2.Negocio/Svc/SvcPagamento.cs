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
    public class SvcPagamento
    {
        public static void InserirPagamento(Pagamento pPagamento)
        {
            try
            {
                using (NutriDbContext db = new NutriDbContext())
                {
                    pPagamento.Consulta = db.Consulta.Where(c => pPagamento.ConsultasId.Any(cid => c.Id == cid)).ToList();
                    db.Pagamento.Add(pPagamento);
                    db.SaveChanges();
                    db.Dispose();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao tentar inserir dados: ", ex);
            }
        }


        public static async Task EidtarPagamentoAsync(Pagamento pPagamento)
        {
            try
            {
                using (NutriDbContext db = new NutriDbContext())
                {
                    var consulta = db.Consulta.Where(c => c.PagamentoId == pPagamento.Id && !pPagamento.ConsultasId.Any(k => c.Id == k));

                    await consulta.ForEachAsync(c => c.PagamentoId = null);
                    if (consulta != null)
                        db.Consulta.UpdateRange(consulta);

                    pPagamento.Consulta = db.Consulta.Where(c => pPagamento.ConsultasId.Any(cid => c.Id == cid)).ToList();
                    db.Pagamento.Update(pPagamento);
                    db.SaveChanges();
                    db.Dispose();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao tentar inserir dados: ", ex);
            }
        }

        public static async 
        Task
DeletarPagamento(int pPagamentoId)
        {
            try
            {

                using (NutriDbContext db = new NutriDbContext())
                {
                    var consulta = await db.Consulta.FirstOrDefaultAsync(c => c.PagamentoId == pPagamentoId);
                    if (consulta != null)
                    {
                        consulta.PagamentoId = null;
                        db.Consulta.Update(consulta);
                        await db.SaveChangesAsync();
                       
                    }
                }
                using (NutriDbContext db = new NutriDbContext())
                {
                    var pagamento = await db.Pagamento.SingleOrDefaultAsync(c => c.Id == pPagamentoId);
                    db.Pagamento.Remove(pagamento);
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao tentar remover dados");
            }
        }

        public static List<Pagamento> ListarPagamentosCompleto()
        {
            try
            {
                using (NutriDbContext db = new NutriDbContext())
                {
                    var pagamentos = db.Pagamento
                                     .Include(c => c.Consulta)
                                     .Include(l => l.LocaisAtendimento)
                                     .ToList();

                    db.Dispose();
                    return pagamentos;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao tentar listar dados: ", ex);
            }
        }
        public static List<Pagamento> ListarPagamentos()
        {
            try
            {
                using (NutriDbContext db = new NutriDbContext())
                {
                    var pagamentos = db.Pagamento
                        .Include(l => l.LocaisAtendimento)
                        .Include(c => c.Consulta)
                        .ThenInclude(pc => pc.Paciente)
                        .Where(p => p.Consulta.Count > 0)
                        .ToList();

                    db.Dispose();
                    return pagamentos;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao tentar listar dados: ", ex);
            }
        }
        public static List<Pagamento> ListarPagamentos(DateTime pDataIncial, DateTime pDataFinal, int pLocalAtendimento, EN_StatusPagamento pSituacao)
        {
            try
            {
                using (NutriDbContext db = new NutriDbContext())
                {
                    var pagamentos = db.Pagamento
                        .Include(l => l.LocaisAtendimento)
                        .Include(c => c.Consulta)
                        .ThenInclude(pc => pc.Paciente)
                        .Where(p => p.Consulta.Count > 0 && p.DataPagamento >= pDataIncial && p.DataPagamento <= pDataFinal)
                        .ToList();

                    if (pLocalAtendimento > 0)
                        pagamentos = pagamentos.Where(la => la.LocaisAtendimento.Id == pLocalAtendimento).ToList();

                    if (pSituacao > 0)
                        pagamentos = pagamentos.Where(s => s.Situacao == pSituacao).ToList();

                    db.Dispose();
                    return pagamentos;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao tentar listar dados: ", ex);
            }
        }
        public static List<Pagamento> ListarPagamentos(DateTime pDataIncial, DateTime pDataFinal, int pLocalAtendimento, EN_StatusPagamento pSituacao, int pPacienteId)
        {
            try
            {
                using (NutriDbContext db = new NutriDbContext())
                {
                    var pagamentos = db.Pagamento
                        .Include(l => l.LocaisAtendimento)
                        .Include(c => c.Consulta)
                        .ThenInclude(pc => pc.Paciente)
                        .Where(p => p.Consulta.Count > 0 && p.DataPagamento >= pDataIncial && p.DataPagamento <= pDataFinal)
                        .ToList();

                    if (pLocalAtendimento > 0)
                        pagamentos = pagamentos.Where(la => la.LocaisAtendimento.Id == pLocalAtendimento).ToList();

                    if (pSituacao > 0)
                        pagamentos = pagamentos.Where(s => s.Situacao == pSituacao).ToList();

                    if (pPacienteId > 0)
                        pagamentos = pagamentos.Where(p => p.Consulta.Any(c => c.PacienteId.Equals(pPacienteId))).ToList();

                    db.Dispose();
                    return pagamentos;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao tentar listar dados: ", ex);
            }
        }
        public static Pagamento BuscarPagamento(int pPagamentoId)
        {
            try
            {
                using (NutriDbContext db = new NutriDbContext())
                {
                    return db.Pagamento
                         .Include(l => l.LocaisAtendimento)
                         .Include(c => c.Consulta)
                         .ThenInclude(p => p.Paciente)
                         .SingleOrDefault(p => p.Id == pPagamentoId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao tentar buscar dados:", ex);
            }
        }

        public static List<Pagamento> ListarPagamentosDoPaciente(int pPacienteId)
        {
            try
            {
                using (NutriDbContext db = new NutriDbContext())
                {
                    return db.Pagamento
                        .Include(con => con.Consulta)
                        .ThenInclude(p => p.Paciente)?
                        .Where(p => p.Consulta.Any(c => c.PacienteId == pPacienteId)).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao tentar listar dados.", ex);
            }

        }


    }
}
