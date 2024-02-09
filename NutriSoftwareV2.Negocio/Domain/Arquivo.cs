using NutriSoftwareV2.Negocio.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutriSoftwareV2.Negocio.Domain
{
    using System;

    public class Arquivo
    {
        public int Id { get; set; }
        public string? NomeDocumento { get; set; }
        public string? ContentType { get; set; }
        public long? TamanhoArquivo { get; set; }
        public string? FileName { get; set; }
        public EN_TipoDocumentoArquivo TipoDocumento { get; set; }

        // Armazena os dados do arquivo como uma string base64 no banco de dados
        [NotMapped]
        public byte[] ArquivoByte { get => Convert.FromBase64String(DadosArquivo); }

        // Propriedade auxiliar para converter a string base64 em array de bytes
        public string DadosArquivo{ get; set; }

        public int? PacienteId { get; set; }
        public Paciente? Paciente { get; set; }
    }

}
