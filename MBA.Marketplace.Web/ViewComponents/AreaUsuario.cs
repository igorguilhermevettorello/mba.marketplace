using MBA.Marketplace.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace MBA.Marketplace.Web.ViewComponents
{
    public class AreaUsuarioViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Verifica se existe um cookie com o token de acesso
            var token = HttpContext.Request.Cookies["AccessToken"];
            var areaUsuario = new AreaUsuarioModel();

            if (!string.IsNullOrEmpty(token))
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
