using System.Security.Permissions;

namespace NutriSoftwareV2.Web.Identity
{
    public class UsuarioSistema
    {
        
        public int? Id { get; set; }
        public string? Nome { get; set; }
        public string? SobreNome { get; set; }
        public string? CPF { get; set; }
        public string? Cargo { get; set; }
    }
}
