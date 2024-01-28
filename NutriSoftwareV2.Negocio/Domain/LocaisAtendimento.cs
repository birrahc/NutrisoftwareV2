using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NutriSoftwareV2.Negocio.Domain
{
    public class LocaisAtendimento
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Cidade { get; set; }
        public string?   Telefone { get; set; }
        
    }
}
