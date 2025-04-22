using Microsoft.AspNetCore.Mvc;
using Cliente_ProyectoFinal.Servicios;
using Cliente_ProyectoFinal.Models.Credito;
using System.Security.Cryptography.X509Certificates;
using Cliente_ProyectoFinal.Models.MovimientoCredito;

namespace Cliente_ProyectoFinal.Controllers
{
    [ServiceFilter(typeof(class_VerificarTokenFiltro))]


    public class MovimientoCreditoController : Controller
    {

        private readonly class_MovimientoCreditoServicio _movcreditoService;
        
        public MovimientoCreditoController(class_MovimientoCreditoServicio movcreditoService)
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

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            string token = HttpContext.Session.GetString("Token");

            var movimiento = await _movcreditoService.BuscarMovimientoPorIdAsync(id, token);
            if (movimiento == null)
                return NotFound();

            return View(movimiento);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(int id, Class_MovimientoCredito movimiento)
        {
            if (ModelState.IsValid)
            {
                string token = HttpContext.Session.GetString("Token");
                bool exito = await _movcreditoService.ActualizarMovimientoAsync(id, movimiento, token);

                if (exito)
                {
                    TempData["Mensaje"] = "Movimiento actualizado correctamente.";
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "Error al actualizar el movimiento.");
            }

            return View(movimiento);
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                string token = HttpContext.Session.GetString("Token");
                bool eliminado = await _movcreditoService.EliminarMovimientoAsync(id, token);

                if (eliminado)
                {
                    TempData["Mensaje"] = "Movimiento eliminado correctamente.";
                }
                else
                {
                    TempData["Error"] = "No se pudo eliminar el movimiento.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ocurrió un error al eliminar el movimiento.";
            }

            return RedirectToAction("Index");
        }

    }
}
