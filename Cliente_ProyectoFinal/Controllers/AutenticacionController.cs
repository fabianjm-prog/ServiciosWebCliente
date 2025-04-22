using Microsoft.AspNetCore.Mvc;

using Cliente_ProyectoFinal.Servicios;

using Cliente_ProyectoFinal.Models.Usuario;

using Microsoft.AspNetCore.Authentication;



namespace Cliente_ProyectoFinal.Controllers
{
    public class AutenticacionController : Controller
    {

        private readonly class_AutenticacionServicio _authService;

        public AutenticacionController(class_AutenticacionServicio authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Logout()
        {
            try
            {
                HttpContext.Session.Clear();

                await HttpContext.SignOutAsync();

                return RedirectToAction("Index", "Autenticacion");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ocurrio un error al intentar cerrar sesion";

                return RedirectToAction("Index", "Autenticacion");

            }
        }

        [HttpPost]


        public async Task<IActionResult> Index(class_Login Usuarios)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string token = await _authService.LoginAsync(Usuarios);
                    if (!string.IsNullOrEmpty(token))
                    {
                        HttpContext.Session.SetString("Token", token);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Credenciales Incorrectas");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ocurrio un error inesperado. Intenta de nuevo");

                }

            }
            return View(Usuarios);

        }

        [HttpPost]
        public async Task<IActionResult> Registrarse(class_User usuario)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string mensajeError = await _authService.RegistrarUsuarioAsync(usuario);

                    if (mensajeError == null)
                    {
                        TempData["Mensaje"] = "Registro existoso, Inicia sesión";
                        return RedirectToAction("Index", "Autenticacion");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = mensajeError;
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Hubo un error inesperado, por favor intentalo luego.";
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Verifica los campos, hay datos inválidos.";
            }

            return View(usuario);
        }

        [HttpGet]

        public IActionResult Registrarse()
        {
            return View();
        }






        public IActionResult Index()
        {
            return View();
        }


    }
}

