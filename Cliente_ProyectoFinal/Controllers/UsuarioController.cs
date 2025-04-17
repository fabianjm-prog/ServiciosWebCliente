using Microsoft.AspNetCore.Mvc;

namespace Cliente_ProyectoFinal.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
