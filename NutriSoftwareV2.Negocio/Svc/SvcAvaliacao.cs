using Microsoft.EntityFrameworkCore;
using NutriSoftwareV2.Negocio.Data.NutriDbContext;
using NutriSoftwareV2.Negocio.Domain;
using NutriSoftwareV2.Negocio.Dto;
using NutriSoftwareV2.Negocio.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NutriSoftwareV2.Negocio.Svc
{
    public class SvcAvaliacao
    {
        public static void CadastrarAvaliacao(AvaliacaoFisica pAvaliacao)
        {
            try
            {
                NutriDbContext db = new NutriDbContext();
                db.AvaliacoesFisicas.Add(pAvaliacao);
                db.SaveChanges();
                db.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao cadastrar avaliação", ex);
            }
        }
        public static void EditarAvaliacao(AvaliacaoFisica pAvaliacao)
        {
            try
            {
                NutriDbContext db = new NutriDbContext();
                db.AvaliacoesFisicas.Update(pAvaliacao);
                db.SaveChanges();
                db.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao cadastrar avaliação", ex);
            }
        }
        public static void DeletarAvaliacao(int pIdAvaliacao)
        {
            try
            {
                var avaliacao = BuscarAvalicao(pIdAvaliacao);
                NutriDbContext db = new NutriDbContext();
                db.AvaliacoesFisicas.Remove(avaliacao);
                db.SaveChanges();
                db.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao deletar avaliação", ex);
            }
        }
        public static List<AvaliacaoFisica> ListarAvaliacoesPorPaciente(int pPacienteId)
        {
            try
            {
                NutriDbContext db = new NutriDbContext();
                var avaliacoes = db.AvaliacoesFisicas.Include(p=>p.Paciente).Where(p => p.PacienteId == pPacienteId).ToList();
                db.Dispose();
                return avaliacoes;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar avalições", ex);
            }
        }
        public static AvaliacaoFisica BuscarAvalicao(int pIdAvaliacao)
        {
            try
            {
                NutriDbContext db = new NutriDbContext();
                var avaliacao = db.AvaliacoesFisicas.Find(pIdAvaliacao);
                db.Dispose();
                return avaliacao;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar avaliação", ex);
            }
        }

        public static List<AvaliacaoFisica> ListarEntreAvaliacoesPaciente(ParametrosPesquisaEntreAvaliacoes pPesquisa)
        {
            try
            {
                NutriDbContext db = new NutriDbContext();
                var avaliacoes = db.AvaliacoesFisicas.Include(p => p.Paciente)
                    .Where(p => p.PacienteId == pPesquisa.PacienteId &&
                           p.NumAvaliacao >= pPesquisa.PrimeiroParamPesquisa &&
                           p.NumAvaliacao <= pPesquisa.SegundoParamPesquisa)
                    .ToList();
                db.Dispose();
                return avaliacoes;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar avalições", ex);
            }

        }
        public static double? CalculularDensidade(double? SomatoriaDc, EN_Sexo? pSexo, int? pIdade)
        {
            if (SomatoriaDc.HasValue && pSexo.HasValue && pIdade.HasValue)
            {
                switch (pSexo)
                {
                    case EN_Sexo.Masculino:
                        return (1.11200000 -
                               (0.00043499 * (SomatoriaDc.Value)) +
                               (0.00000055 * (Math.Pow(SomatoriaDc.Value, 2))) -
                               (0.0002882 * (pIdade.Value)));

                    case EN_Sexo.Feminino:
                        return (1.097 -
                              (0.00046971 * (SomatoriaDc.Value)) +
                              (0.00000056 * (Math.Pow(SomatoriaDc.Value, 2))) -
                              (0.00012828 * (pIdade.Value)));
                }

            }
            return null;
        }
        public static double CalcularPercentualGordura(double pDensidade)
        {
            return ((4.95 / pDensidade) - 4.5) * 100;
        }
        public static double CalcularMassaMuscular(double pPercentualGordura, double pPeso)
        {
            return pPeso - ((pPercentualGordura / 100) * pPeso);
        }
        public static double CalcularGordura(double pPercentualGordura, double pPeso)
        {
            return (pPercentualGordura / 100) * pPeso;
        }

    }
}
