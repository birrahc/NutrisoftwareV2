using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Text;

namespace NutriSoftwareV2.Web.Identity
{
    
    public class Login
    {

        [Required(ErrorMessage ="O Email é obrigatório")]
        [EmailAddress(ErrorMessage ="Email inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage ="A senha é obrigatória")]
        [Display(Name ="Senha")]
        [DataType(DataType.Password)]
        public string PassWord { get; set; }

        [Display(Name ="Lembrar me")]
        public bool Relembrar { get; set; }

        
       
    }
}
