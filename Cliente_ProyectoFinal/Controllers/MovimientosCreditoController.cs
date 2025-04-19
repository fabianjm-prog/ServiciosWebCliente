using Microsoft.AspNetCore.Mvc;
using Cliente_ProyectoFinal.Servicios;
using Cliente_ProyectoFinal.Models.Credito;
using System.Security.Cryptography.X509Certificates;
using Cliente_ProyectoFinal.Models.MovimientoCredito;

namespace Cliente_ProyectoFinal.Controllers
{
    [ServiceFilter(typeof(class_VerificarTokenFiltro))]


    public class MovimientosCreditoController : Controller
    {

        private readonly class_MovimientoCreditoServicio _movcreditoService;
        
        public MovimientosCreditoController(class_MovimientoCreditoServicio movcreditoService)
        {
            _movcreditoService = movcreditoService;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate, private";
                Response.Headers["pragma"] = "no-cache";
                Response.Headers["Expires"] = "-1";

                string token = HttpContext.Session.GetString("Token");

                List<Class_MovimientoCredito> movcreditos = await _movcreditoService.ObtenermovCreditoAsync(token);

                return View(movcreditos);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al obtener los movimientos creditos");
                return View(new List<Class_MovimientoCredito>());
            }
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Class_MovimientoCredito movcredito)
        {
            if (ModelState.IsValid)
            {
                string token = HttpContext.Session.GetString("Token");
                

                var mensajeError = await _movcreditoService.CrearMovCreditoAsync(movcredito, token);

                if (mensajeError == null)
                {
                    TempData["Mensaje"] = "Crédito creado exitosamente.";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", mensajeError);
                }
            }

            return View(movcredito);
        }

    }
}
