using NutriSoftwareV2.Negocio.Domain;
using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Text;

namespace NutriSoftwareV2.Web.Components.ComponentModel
{
    public class ComponenteDietaModel
    {
        public Dieta Dieta { get; set; }
        public bool Cabecalho { get; set; }
        public ComponenteDietaModel()
        {
            this.Cabecalho = true;
        }
    }
}
