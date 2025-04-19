using Cliente_ProyectoFinal.ApiUrls;
using Cliente_ProyectoFinal.Models.Habitaciones;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Cliente_ProyectoFinal.Servicios
{
    public class Class_HabitacionServicio
    {
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
    }
}
