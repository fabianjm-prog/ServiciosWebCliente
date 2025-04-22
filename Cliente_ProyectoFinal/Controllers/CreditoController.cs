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

                return View( creditos);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al obtener los creditos");
                return View(new List<Class_Credito>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> BuscarCredito(string CedulaP)
        {
            try
            {
                string token = HttpContext.Session.GetString("Token");
                var creditos = await _creditoService.BuscarCreditoAsync(CedulaP, token);

                if (creditos == null || creditos.Count == 0)
                {
                    ViewBag.Mensaje = "No se encontraron créditos con esa cédula.";
                }

                return View(creditos);
            }
            catch
            {
                ViewBag.Mensaje = "Ocurrió un error al buscar los créditos.";
                return View( new List<Class_Credito>());
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
       


        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            string token = HttpContext.Session.GetString("Token");
            var credito = await _creditoService.BuscarCreditoPorIdAsync(id, token);
            if (credito == null) return NotFound();
            return View(credito);
        }


        [HttpPost]
        public async Task<IActionResult> Editar(int id, Class_Credito credito)
        {
            if (ModelState.IsValid)
            {
                string token = HttpContext.Session.GetString("Token");
                await _creditoService.ActualizarCreditoAsync(id, credito, token);
                //return RedirectToAction(nameof(Index));
                TempData["Mensaje"] = "Crédito creado exitosamente.";
                return RedirectToAction("Index");
            }
            return View(credito);
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                string token = HttpContext.Session.GetString("Token");
                bool eliminado = await _creditoService.EliminarCreditoAsync(id, token);

                if (eliminado)
                {
                    TempData["Mensaje"] = "Crédito eliminado exitosamente.";
                }
                else
                {
                    TempData["Mensaje"] = "No se pudo eliminar el crédito.";
                }
            }
            catch
            {
                TempData["Mensaje"] = "Ocurrió un error al intentar eliminar el crédito.";
            }

            return RedirectToAction("Index");
        }








    }
}
