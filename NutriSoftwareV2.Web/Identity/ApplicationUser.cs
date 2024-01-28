using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata;
using NutriSoftwareV2.Web.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriSoftwareV2.Web.Identity
{
    public class ApplicationUser : IdentityUser
    {

        //[Column("USR_CPF")]
        //public string CPF { get; set; }

        public int UsuarioSistemaId { get; set; }
        public virtual UsuarioSistema UsuarioSistema { get; set; }
    }
}
