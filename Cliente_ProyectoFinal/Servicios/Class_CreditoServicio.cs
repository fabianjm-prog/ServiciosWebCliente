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
        private readonly string _baseUrl = "https://localhost:7076/api/";  //Create
                                                                           //private readonly string _baseUrl = "https://localhost:7076/api/"; //Update
                                                                           //private readonly string _baseUrl = "https://localhost:7076/api/"; //Delete
        private readonly string __baseUrl = "https://localhost:7195/api/"; //Read

        public async Task<List<Class_Credito>> ObtenerCreditoAsync(string token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync(__baseUrl + "Credito/ListaCredito");
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
                var response = await client.GetAsync(__baseUrl + "Controller_Credito/BuscarCredito/{CedulaP}");
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
                var response = await client.PostAsync(_baseUrl + "Credito/AgregarCredito", content);
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



    }
}

