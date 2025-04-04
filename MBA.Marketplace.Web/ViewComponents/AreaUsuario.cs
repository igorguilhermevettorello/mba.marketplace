using MBA.Marketplace.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Principal;

namespace MBA.Marketplace.Web.ViewComponents
{
    public class AreaUsuarioViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var areaUsuario = new AreaUsuarioModel();

            if (!string.IsNullOrEmpty(userId))
            {
                areaUsuario.Logado = true;
                areaUsuario.Nome = string.Empty;
            }
            else 
            {
                areaUsuario.Logado = false;
                areaUsuario.Nome = string.Empty;
            }

            return View(areaUsuario);
        }
    }
}
