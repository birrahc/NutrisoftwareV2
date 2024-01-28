using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySqlX.XDevAPI.Relational;
using NutriSoftwareV2.Negocio.Enums;
using NutriSoftwareV2.Negocio.Svc;
using NutriSoftwareV2.Negocio.Domain;

using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace NutriSoftwareV2.Web.Controllers
{
    public class PagamentoController : Controller
    {
        public IActionResult Index()
        {
            CarregarSelctLocaisAtendimento();
            CarregarSelectListPacientes();
            return View(new List<Pagamento>());
        }

        [HttpPost]
        public IActionResult ResultadoPagamento(DateTime DataIncial, DateTime DataFinal, int LocaisAtendimentoId, EN_StatusPagamento Situacao, int PacienteId)
        {
            var lista = SvcPagamento.ListarPagamentos(DataIncial, DataFinal, LocaisAtendimentoId, Situacao,PacienteId);
            ViewBag.Mensagem = lista.Count < 1 ? "Nenhum pagamento encontrado." : "";
            CarregarSelctLocaisAtendimento();
            CarregarSelectListPacientes();
            return PartialView("PartiaisPagamentos/_ResultadoPagamento", lista);
        }
        public IActionResult Create()
        {
            CarregarSelctLocaisAtendimento();
            CarregarSelectListConsultas();
            return PartialView("PartiaisPagamentos/_FormularioPagamento");
        }

        [HttpPost]
        public ActionResult Create([FromForm] Pagamento pagamento)
        {
            SvcPagamento.InserirPagamento(pagamento);
            List<Pagamento> lista = new List<Pagamento>();
            var pgt = SvcPagamento.BuscarPagamento(pagamento.Id);
            lista.Add(pgt);
            ViewBag.Pagamento = pgt;
            ViewBag.Mensagem = lista.Count < 1 ? "Nenhum pagamento encontrado." : "";
            CarregarSelctLocaisAtendimento(pgt.Id);
            return PartialView("PartiaisPagamentos/_ConteudoPagamento", lista);
        }
        public IActionResult Edit(int Id)
        {
            if (Id > 0)
            {
                var pgt = SvcPagamento.BuscarPagamento(Id);
                pgt.ConsultasId = pgt?.Consulta?.Select(i => i.Id).ToList();
                CarregarSelctLocaisAtendimento(pgt.LocaisAtendimentoId);
                CarregarSelectListPacientes(pgt?.Consulta?.FirstOrDefault()?.PacienteId);

                CarregarSelectListConsultas(pgt.ConsultasId);
                return PartialView("PartiaisPagamentos/_FormularioPagamento", pgt);
            }
            return View();
        }

        [HttpPost]
        public IActionResult Delete(int Id)
        {
            if (Id > 0)
            {
                SvcPagamento.DeletarPagamento(Id);
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Edit([FromForm] Pagamento pagamento)
        {
            SvcPagamento.EidtarPagamento(pagamento);
            List<Pagamento> lista = new List<Pagamento>();
            var pgt = SvcPagamento.BuscarPagamento(pagamento.Id);
            lista.Add(pgt);
            ViewBag.Pagamento = pgt;
            ViewBag.Mensagem = lista.Count < 1 ? "Nenhum pagamento encontrado." : "";
            CarregarSelctLocaisAtendimento(pgt.Id);
            return PartialView("PartiaisPagamentos/_ConteudoPagamento", lista);
        }
       
        private void CarregarSelctLocaisAtendimento(int? pId = null)
        {
            var locais = SvcLocaisAtendimento.ListarLocaisAtendimento();
            ViewBag.LocaisAtendimento = new SelectList(locais, "Id", "Nome", pId);

        }

        private void CarregarSelectListPacientes(int? pId = null)
        {
            var locais = SvcPaciente.ListarPacientes();
            ViewBag.Pacientes = new SelectList(locais, "Id", "Nome", pId);

        }

        private void CarregarSelectListConsultas(List<int> pIds = null)
        {
            var consultas = pIds != null ?
                SvcConsulta.ListarConsultasSemRegistroDePagamento(pIds) :
                SvcConsulta.ListarConsultasSemRegistroDePagamento();

            ViewBag.Consultas = new MultiSelectList(consultas, "Id", "Descricao", pIds);
        }
    }
}
