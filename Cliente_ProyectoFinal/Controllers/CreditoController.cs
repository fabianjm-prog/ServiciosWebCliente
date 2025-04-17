using Microsoft.AspNetCore.Mvc;
using Cliente_ProyectoFinal.Servicios;
using Cliente_ProyectoFinal.Models.Credito;
using System.Security.Cryptography.X509Certificates;


namespace Cliente_ProyectoFinal.Controllers
{

    [ServiceFilter(typeof(class_VerificarTokenFiltro))]

    public class CreditoController : Controller
    {


        private readonly class_CreditoServicio _creditoService;

        public CreditoController(class_CreditoServicio creditoService)
        {
            _creditoService = creditoService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate, private";
                Response.Headers["pragma"] = "no-cache";
                Response.Headers["Expires"] = "-1";

                string token = HttpContext.Session.GetString("Token");

                List<Class_Credito> creditos = await _creditoService.ObtenerCreditoAsync(token);

                return View(creditos);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al obtener los creditos");
                return View(new List<Class_Credito>());
            }



        }
        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Class_Credito credito)
        {
            if (ModelState.IsValid)
            {
                string token = HttpContext.Session.GetString("Token");
                credito.fecha_creacion = DateTime.Now;
                credito.estado = "Activo";

                var mensajeError = await _creditoService.CrearCreditoAsync(credito, token);

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

            return View(credito);
        }








    }
}
