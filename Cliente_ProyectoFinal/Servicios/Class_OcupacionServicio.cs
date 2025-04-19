using Cliente_ProyectoFinal.ApiUrls;
using Cliente_ProyectoFinal.Models;
using Cliente_ProyectoFinal.Models.Ocupaciones;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace Cliente_ProyectoFinal.Servicios
{
    public class Class_OcupacionServicio
    {
        public async Task<List<Class_Ocupaciones>> ObtenerOcupacionAsync(string token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync(Class_Url.ReadUrl + "Ocupacion/ListaOcupaciones");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var jobject = JObject.Parse(json);
                    var productos = jobject["value"]?.ToObject<List<Class_Ocupaciones>>();
                    return productos ?? new List<Class_Ocupaciones>();
                }

                return new List<Class_Ocupaciones>();
            }
        }

        public async Task<string> CrearOcupacionnAsync(Class_Ocupaciones habi, string token)
        {


            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var json = JsonConvert.SerializeObject(habi);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(Class_Url.CreateUrl + "Ocupacion/AgregarOcupacion", content);
                var responseBody = await response.Content.ReadAsStringAsync();


                if (response.IsSuccessStatusCode)
                {
                    return null; // No error, todo bien
                }
                else
                {
                    // Deserializa y devuelve el mensaje de error
                    var errorResponse = JsonConvert.DeserializeObject<ApiResponse<object>>(responseBody);
                    return errorResponse?.mensaje ?? "Error desconocido al crear la ocupacion.";
                }
            }
        }
        public async Task<bool> ActualizarOcupacionAsync(int id, Class_Ocupaciones ocupacion, string token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var json = JsonConvert.SerializeObject(ocupacion);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(Class_Url.UpdateUrl + $"Ocupaciones/ActualizarOcupacion/{id}", content);
                return response.IsSuccessStatusCode;
            }
        }
    }
}
