using Microsoft.AspNetCore.Mvc;

namespace MBA.Marketplace.Web.Controllers
{
    public class RegistrarController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public RegistrarController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
