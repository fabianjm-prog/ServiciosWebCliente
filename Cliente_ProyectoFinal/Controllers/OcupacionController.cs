using Microsoft.AspNetCore.Mvc;
using Cliente_ProyectoFinal.Servicios;
using System.Security.Cryptography.X509Certificates;
using Cliente_ProyectoFinal.Models.Ocupaciones;

namespace Cliente_ProyectoFinal.Controllers
{
    [ServiceFilter(typeof(class_VerificarTokenFiltro))]
    public class OcupacionController : Controller
    {

        private readonly Class_OcupacionServicio _OcupacionService;

        public OcupacionController(Class_OcupacionServicio OcupacionService)
        {
            _OcupacionService = OcupacionService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate, private";
                Response.Headers["pragma"] = "no-cache";
                Response.Headers["Expires"] = "-1";

                string token = HttpContext.Session.GetString("Token");

                List<Class_Ocupaciones> creditos = await _OcupacionService.ObtenerOcupacionesAsync(token);

                return View(creditos);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al obtener los creditos");
                return View(new List<Class_Ocupaciones>());
            }
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Class_Ocupaciones habi)
        {
            if (ModelState.IsValid)
            {
                string token = HttpContext.Session.GetString("Token");


                var mensajeError = await _OcupacionService.CrearOcupacionnAsync(habi, token);

                if (mensajeError == null)
                {
                    TempData["Mensaje"] = "ocupacion creada exitosamente.";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", mensajeError);
                }
            }

            return View(habi);
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                string token = HttpContext.Session.GetString("Token");
                bool eliminado = await _OcupacionService.EliminarOcupacionAsync(id, token);

                if (eliminado)
                {
                    TempData["Mensaje"] = "Ocupación eliminada correctamente.";
                }
                else
                {
                    TempData["Error"] = "No se pudo eliminar la ocupación.";
                }
            }
            catch
            {
                TempData["Error"] = "Ocurrió un error al eliminar la ocupación.";
            }

            return RedirectToAction("Index");
        }
    }
}
