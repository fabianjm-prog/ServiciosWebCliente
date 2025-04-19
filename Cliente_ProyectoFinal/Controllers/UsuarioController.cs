using Microsoft.AspNetCore.Mvc;
using Cliente_ProyectoFinal.Servicios;
using Cliente_ProyectoFinal.Models.Usuario;
using System.Security.Cryptography.X509Certificates;



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

        public async Task<IActionResult> Lista_Usuarios()
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
    }
}
