using Microsoft.AspNetCore.Mvc;

namespace Cliente_ProyectoFinal.Controllers
{
    public class CreditoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> create()
        {

            return View();
        }


    }
}
