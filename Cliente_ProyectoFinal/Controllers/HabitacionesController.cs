﻿using Microsoft.AspNetCore.Mvc;
using Cliente_ProyectoFinal.Servicios;
using Cliente_ProyectoFinal.Models.Credito;
using System.Security.Cryptography.X509Certificates;
using Cliente_ProyectoFinal.Models.Habitaciones;
using Cliente_ProyectoFinal.Models.MovimientoCredito;
using ClosedXML.Excel;

namespace Cliente_ProyectoFinal.Controllers
{
    [ServiceFilter(typeof(class_VerificarTokenFiltro))]
    public class HabitacionesController : Controller
    {

        private readonly Class_HabitacionServicio _habitacionesService;

        public HabitacionesController(Class_HabitacionServicio habitacionesService)
        {
            _habitacionesService = habitacionesService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate, private";
                Response.Headers["pragma"] = "no-cache";
                Response.Headers["Expires"] = "-1";

                string token = HttpContext.Session.GetString("Token");

                List<Class_Habitaciones> habi = await _habitacionesService.ObtenerHabitacionAsync(token);

                return View(habi);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al obtener las habitaciones");
                return View(new List<Class_Habitaciones>());
            }
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Class_Habitaciones habi)
        {
            if (ModelState.IsValid)
            {
                string token = HttpContext.Session.GetString("Token");


                var mensajeError = await _habitacionesService.CrearHabitacionAsync(habi, token);

                if (mensajeError == null)
                {
                    TempData["Mensaje"] = "habitacion creada exitosamente.";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", mensajeError);
                }
            }

            return View(habi);
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            string token = HttpContext.Session.GetString("Token");

            var habitacion = await _habitacionesService.BuscarHabitacionPorIdAsync(id, token);
            if (habitacion == null)
                return NotFound();

            return View(habitacion);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(int id, Class_Habitaciones habitacion)
        {
            if (ModelState.IsValid)
            {
                string token = HttpContext.Session.GetString("Token");
                var exito = await _habitacionesService.ActualizarHabitacionAsync(id, habitacion, token);

                if (exito)
                {
                    TempData["Mensaje"] = "Habitación actualizada correctamente.";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "No se pudo actualizar la habitación.");
                }
            }

            return View(habitacion);
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                string token = HttpContext.Session.GetString("Token");
                bool eliminado = await _habitacionesService.EliminarHabitacionAsync(id, token);

                TempData["Mensaje"] = eliminado
                    ? "Habitación eliminada correctamente."
                    : "No se pudo eliminar la habitación.";
            }
            catch
            {
                TempData["Mensaje"] = "Ocurrió un error al eliminar la habitación.";
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ExportarExcel()
        {
            string token = HttpContext.Session.GetString("Token");
            var listaHabitaciones = await _habitacionesService.ObtenerHabitacionAsync(token);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Habitaciones");

                // Encabezados
                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Número";
                worksheet.Cell(1, 3).Value = "Tipo ID";
                worksheet.Cell(1, 4).Value = "Estado";

                int fila = 2;
                foreach (var hab in listaHabitaciones)
                {
                    worksheet.Cell(fila, 1).Value = hab.habitacion_ID;
                    worksheet.Cell(fila, 2).Value = hab.numero;
                    worksheet.Cell(fila, 3).Value = hab.tipo_ID;
                    worksheet.Cell(fila, 4).Value = hab.estado;
                    fila++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;
                    return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "ReporteHabitaciones.xlsx");
                }
            }
        }

    }
}
