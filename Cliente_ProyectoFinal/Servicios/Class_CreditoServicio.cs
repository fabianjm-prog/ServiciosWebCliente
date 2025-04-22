using Cliente_ProyectoFinal.ApiUrls;
using Cliente_ProyectoFinal.Models;
using Cliente_ProyectoFinal.Models.Credito;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace Cliente_ProyectoFinal.Servicios
{
    public class class_CreditoServicio
    {

        public async Task<List<Class_Credito>> ObtenerCreditoAsync(string token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync(Class_Url.ReadUrl + "Credito/ListaCredito");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var jobject = JObject.Parse(json);
                    var productos = jobject["value"]?.ToObject<List<Class_Credito>>();
                    return productos ?? new List<Class_Credito>();
                }

                return new List<Class_Credito>();
            }
        }

        public async Task<List<Class_Credito>> BuscarCreditoAsync(string CedulaP, string token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync(Class_Url.ReadUrl + "Controller_Credito/BuscarCredito/{CedulaP}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var jobject = JObject.Parse(json);
                    var productos = jobject["value"]?.ToObject<List<Class_Credito>>();
                    return productos ?? new List<Class_Credito>();
                }
                return new List<Class_Credito>();

            }
        }

        public async Task<string> CrearCreditoAsync(Class_Credito credito, string token)
        {


            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var json = JsonConvert.SerializeObject(credito);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(Class_Url.CreateUrl + "Credito/AgregarCredito", content);
                var responseBody = await response.Content.ReadAsStringAsync();


                if (response.IsSuccessStatusCode)
                {
                    return null; // No error, todo bien
                }
                else
                {
                    // Deserializa y devuelve el mensaje de error
                    var errorResponse = JsonConvert.DeserializeObject<ApiResponse<object>>(responseBody);
                    return errorResponse?.mensaje ?? "Error desconocido al crear el crédito.";
                }
            }
        }
        public async Task<Class_Credito> BuscarCreditoPorIdAsync(int id, string token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync(Class_Url.ReadUrl + $"Credito/BuscarCreditoPorId/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var jobject = JObject.Parse(json);
                    return jobject["value"]?.ToObject<Class_Credito>();
                }

                return null;
            }
        }

        public async Task<bool> ActualizarCreditoAsync(int id, Class_Credito credito, string token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var json = JsonConvert.SerializeObject(credito);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(Class_Url.UpdateUrl + $"Credito/ActualizarCredito/{id}", content);

                return response.IsSuccessStatusCode;
            }
        }

        public async Task<bool> EliminarCreditoAsync(int creditoID, string token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.DeleteAsync(Class_Url.DeleteUrl + $"Credito/EliminarCredito/{creditoID}");

                return response.IsSuccessStatusCode;
            }
        }




    }
}

