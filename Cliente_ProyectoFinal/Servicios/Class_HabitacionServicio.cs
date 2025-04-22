using Cliente_ProyectoFinal.ApiUrls;
using Cliente_ProyectoFinal.Models;
using Cliente_ProyectoFinal.Models.Habitaciones;
using Cliente_ProyectoFinal.Models.MovimientoCredito;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace Cliente_ProyectoFinal.Servicios
{
    public class Class_HabitacionServicio
    {

        public async Task<List<Class_Habitaciones>> ObtenerHabitacionAsync(string token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync(Class_Url.ReadUrl + "Habitacion/ListaHabitaciones");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var jobject = JObject.Parse(json);
                    var productos = jobject["value"]?.ToObject<List<Class_Habitaciones>>();
                    return productos ?? new List<Class_Habitaciones>();
                }

                return new List<Class_Habitaciones>();
            }
        }

        public async Task<string> CrearHabitacionAsync(Class_Habitaciones habi, string token)
        {


            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var json = JsonConvert.SerializeObject(habi);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(Class_Url.CreateUrl + "Habitaciones/AgregarHabitacion", content);
                var responseBody = await response.Content.ReadAsStringAsync();


                if (response.IsSuccessStatusCode)
                {
                    return null; // No error, todo bien
                }
                else
                {
                    // Deserializa y devuelve el mensaje de error
                    var errorResponse = JsonConvert.DeserializeObject<ApiResponse<object>>(responseBody);
                    return errorResponse?.mensaje ?? "Error desconocido al crear la habitacion.";
                }
            }
        }

        public async Task<Class_Habitaciones> BuscarHabitacionPorIdAsync(int id, string token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync(Class_Url.ReadUrl + $"Habitacion/BuscarHabitacionPorId/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var jobject = JObject.Parse(json);
                    return jobject["value"]?.ToObject<Class_Habitaciones>();
                }

                return null;
            }
        }

        public async Task<bool> ActualizarHabitacionAsync(int id, Class_Habitaciones habitacion, string token)
        {
            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var json = JsonConvert.SerializeObject(habitacion);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(Class_Url.UpdateUrl + $"Habitaciones/ActualizarHabitacion/{id}", content);
                return response.IsSuccessStatusCode;
            }
        }
        public async Task<bool> EliminarHabitacionAsync(int id, string token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.DeleteAsync(Class_Url.DeleteUrl + $"Habitaciones/EliminarHabitacion/{id}");
                return response.IsSuccessStatusCode;
            }
        }



    }
}
