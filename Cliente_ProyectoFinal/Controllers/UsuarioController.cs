using Microsoft.AspNetCore.Mvc;
using Cliente_ProyectoFinal.Servicios;
using Cliente_ProyectoFinal.Models.Usuario;
using System.Security.Cryptography.X509Certificates;
using Cliente_ProyectoFinal.Models.Ocupaciones;
using Microsoft.AspNetCore.Mvc.Rendering;



namespace Cliente_ProyectoFinal.Controllers
{
    [ServiceFilter(typeof(class_VerificarTokenFiltro))]

    public class UsuarioController : Controller
    {
        private readonly Class_PersonaServicio _personaService;

        public UsuarioController(Class_PersonaServicio personaService)
        {
            _personaService = personaService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                string token = HttpContext.Session.GetString("Token");
                List<class_User> personas = await _personaService.ObtenerPersonasAsync(token);
                return View(personas);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al obtener los creditos");
                return View(new List<class_User>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> BuscarUsuario(string CedulaP)
        {
            try
            {
                string token = HttpContext.Session.GetString("Token");
                var persona = await _personaService.BuscarPersonaAsync(CedulaP, token);

                if (persona == null)
                {
                    ViewBag.Mensaje = "No se encontro datos de esta persona";
                }

                return View("Persona");
            }
            catch
            {
                ViewBag.Mensaje = "Ocurrió un error al buscar los datos de la persona.";
                return View("", new List<class_User>());
            }
        }

        
        [HttpGet]
        public IActionResult Crear()
        {
            ViewBag.Roles = new List<SelectListItem>
    {
        new SelectListItem { Text = "Administrador", Value = "1" },
        new SelectListItem { Text = "Empleado", Value = "2" },
        new SelectListItem { Text = "Cliente", Value = "3" }
    };

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(class_User habi)
        {
            if (ModelState.IsValid)
            {
                string token = HttpContext.Session.GetString("Token");


                var mensajeError = await _personaService.CrearPersonaAsync(habi, token);

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
            ViewBag.Roles = new List<SelectListItem>
    {
        new SelectListItem { Text = "Administrador", Value = "1" },
        new SelectListItem { Text = "Empleado", Value = "2" },
        new SelectListItem { Text = "Cliente", Value = "3" }
    };

            return View(habi);
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                string token = HttpContext.Session.GetString("Token");
                bool eliminado = await _personaService.EliminarUsuarioAsync(id, token);

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
        [HttpGet]
        public async Task<IActionResult> Editar(string id)
        {
            string token = HttpContext.Session.GetString("Token");

            var persona = await _personaService.BuscarPersonaPorCedulaAsync(id, token);
            if (persona == null)
                return NotFound();

            ViewBag.Roles = new List<SelectListItem>
    {
        new SelectListItem { Text = "Administrador", Value = "1" },
        new SelectListItem { Text = "Empleado", Value = "2" },
        new SelectListItem { Text = "Cliente", Value = "3" }
    };

            return View(persona);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(String id, class_User persona)
        {
            if (ModelState.IsValid)
            {

                string token = HttpContext.Session.GetString("Token");
                var exito = await _personaService.ActualizarPersonaAsync(id, persona, token);

                if (exito)
                {
                    TempData["Mensaje"] = "Persona actualizada correctamente.";
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "No se pudo actualizar la persona.");
            }
                ViewBag.Roles = new List<SelectListItem>
    {
        new SelectListItem { Text = "Administrador", Value = "1" },
        new SelectListItem { Text = "Empleado", Value = "2" },
        new SelectListItem { Text = "Cliente", Value = "3" }
    };

            

            return View(persona);
        }

    }
}
