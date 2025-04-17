using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;


namespace Cliente_ProyectoFinal.Servicios
{
    public class class_VerificarTokenFiltro : IActionFilter
    {

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Obtiene la sesión HTTP asociada con la solicitud actual
            var session = context.HttpContext.Session;

            // Recupera el token almacenado en la sesión.
            var token = session.GetString("Token");

            // Verifica si el token no está presente o es nulo/vacío
            if (string.IsNullOrEmpty(token))
            {
                // Configura un mensaje de error en TempData para mostrarlo después del redireccionamiento.
                // Obtiene el servicio de fábrica para trabajar con TempData.
                var tempData = context.HttpContext.RequestServices
                    .GetService(typeof(ITempDataDictionaryFactory)) as ITempDataDictionaryFactory;

                // Obtiene el diccionario TempData para la solicitud actual.
                var tempDataDictionary = tempData?.GetTempData(context.HttpContext);

                // Si TempData está disponible, establece un mensaje de error.
                if (tempDataDictionary != null)
                {
                    tempDataDictionary["ErrorMessage"] = "Primero debe autenticarse.";
                }

                // Configura el resultado de la acción para redirigir al controlador de autenticación (login).
                context.Result = new RedirectToActionResult("Index", "Autenticacion", null);
            }
        }

        // Método que se ejecuta después de que una acción del controlador haya sido invocada.
        // La creo por usar IActionFilter
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Acá se puede Registrar actividad o resultados, Manejo de errores, Limpiar recursos, etc
            // Registrar información de la acción ejecutada
            var controllerName = context.RouteData.Values["controller"]?.ToString();
            var actionName = context.RouteData.Values["action"]?.ToString();
            Console.WriteLine($"Acción ejecutada: {controllerName}/{actionName} en {DateTime.Now}");
            // Manejo de errores
            if (context.Exception != null)
            {
                // Registrar el error
                Console.WriteLine($"Error en la acción: {context.Exception.Message}");
                // Opcional: Configurar una respuesta personalizada en caso de error
                context.Result = new RedirectToActionResult("Error", "Shared", null);
                // Indicar que la excepción fue manejada
                context.ExceptionHandled = true;
            }
        }

    }
}

