using Cliente_ProyectoFinal.ApiUrls;
using Cliente_ProyectoFinal.Models;
using Cliente_ProyectoFinal.Models.Ocupaciones;
using Cliente_ProyectoFinal.Models.Usuario;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace Cliente_ProyectoFinal.Servicios
{
    public class Class_OcupacionServicio
    {
        public async Task<List<Class_Ocupaciones>> ObtenerOcupacionesAsync(string token)
        {

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync(Class_Url.ReadUrl + "Ocupacion/ListaOcupaciones");
                if (response.IsSuccessStatusCode)
                {
                    
                        var json = await response.Content.ReadAsStringAsync();
                        var jobject = JObject.Parse(json);

                        var personas = jobject["value"]?.ToObject<List<Class_Ocupaciones>>();
                        return personas ?? new List<Class_Ocupaciones>();
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
                    var errorResponse = JsonConvert.DeserializeObject<ApiResponse<object>>(responseBody);
                    return errorResponse?.mensaje ?? "Error desconocido al crear la ocupacion.";
                }
            }
        }
       

        public async Task<bool> EliminarOcupacionAsync(int id, string token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.DeleteAsync(Class_Url.DeleteUrl + $"Ocupacion/EliminarOcupacion/{id}");
                return response.IsSuccessStatusCode;
            }
        }
        public async Task<Class_Ocupaciones> BuscarOcupacionPorIdAsync(int id, string token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync(Class_Url.ReadUrl + $"Ocupacion/BuscarOcupacionPorId/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var jobject = JObject.Parse(json);
                    return jobject["value"]?.ToObject<Class_Ocupaciones>();
                }

                return null;
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
