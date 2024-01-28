using Microsoft.AspNetCore.Mvc;
using NutriSoftwareV2.Negocio.Domain;
using NutriSoftwareV2.Web.Components.ComponentModel;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NutriSoftwareV2.Web.Components
{

    public class DietaViewComponent : ViewComponent
    {
        public DietaViewComponent()
        {

        }
        public async Task<IViewComponentResult> InvokeAsync(ComponenteDietaModel pComp)
        {
            return View(pComp);
        }
    }
}
