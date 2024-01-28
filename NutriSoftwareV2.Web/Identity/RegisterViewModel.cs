using System.ComponentModel.DataAnnotations;

namespace NutriSoftwareV2.Web.Identity
{
    public class RegisterViewModel
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string PassWord { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confimre a senha")]
        [Compare("PassWord", ErrorMessage ="As senhas não conferem")]
        public string ConirmPassword { get; set; }

        public UsuarioSistema? UsuarioSistema { get; set; }
    }
}
