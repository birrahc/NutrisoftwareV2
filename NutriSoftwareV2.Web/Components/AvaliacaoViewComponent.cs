using Microsoft.AspNetCore.Mvc;
using NutriSoftwareV2.Web.Components.ComponentModel;
using System.Threading.Tasks;

namespace NutriSoftwareV2.Web.Components
{
    public class AvaliacaoViewComponent : ViewComponent
    {
        public AvaliacaoViewComponent()
        {

        }
        public async Task<IViewComponentResult> InvokeAsync(ComponenteAvaliacaoModel pComp)
        {
            return View(pComp.avaliacaoFisica);
        }
    }
}
