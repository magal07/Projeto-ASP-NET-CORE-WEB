using Microsoft.AspNetCore.Mvc;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller // este controlador recebeu a chamada do caminho /sellers
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
