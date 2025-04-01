using MBA.Marketplace.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace MBA.Marketplace.Web.Controllers
{
    [Autorizado]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
