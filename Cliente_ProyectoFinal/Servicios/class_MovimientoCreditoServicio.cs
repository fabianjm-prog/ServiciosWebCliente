using Cliente_ProyectoFinal.ApiUrls;
using Cliente_ProyectoFinal.Models.Credito;
using Cliente_ProyectoFinal.Models;
using Cliente_ProyectoFinal.Models.MovimientoCredito;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Cliente_ProyectoFinal.Servicios
{
    public class class_MovimientoCreditoServicio
    {

        public async Task<List<Class_MovimientoCredito>> ObtenermovCreditoAsync(string token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync(Class_Url.ReadUrl + "MovimientoCreditos/ListaMovimientos");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var jobject = JObject.Parse(json);
                    var productos = jobject["value"]?.ToObject<List<Class_MovimientoCredito>>();
                    return productos ?? new List<Class_MovimientoCredito>();
                }

                return new List<Class_MovimientoCredito>();
            }
        }

        public async Task<string> CrearMovCreditoAsync(Class_MovimientoCredito movcredito, string token)
        {


            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var json = JsonConvert.SerializeObject(movcredito);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(Class_Url.CreateUrl + "MovimientoCredito/AgregarMovCredito", content);
                var responseBody = await response.Content.ReadAsStringAsync();


                if (response.IsSuccessStatusCode)
                {
                    return null; // No error, todo bien
                }
                else
                {
                    // Deserializa y devuelve el mensaje de error
                    var errorResponse = JsonConvert.DeserializeObject<ApiResponse<object>>(responseBody);
                    return errorResponse?.mensaje ?? "Error desconocido al crear el movimiento.";
                }
            }
        }

        public async Task<Class_MovimientoCredito> BuscarMovimientoPorIdAsync(int id, string token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync(Class_Url.ReadUrl + $"MovimientoCreditos/BuscarMovimientoPorId/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var jobject = JObject.Parse(json);
                    return jobject["value"]?.ToObject<Class_MovimientoCredito>();
                }

                return null;
            }
        }

        public async Task<bool> ActualizarMovimientoAsync(int id, Class_MovimientoCredito movimiento, string token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var json = JsonConvert.SerializeObject(movimiento);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(Class_Url.UpdateUrl + $"MovimientosCredito/ActualizarMovimiento/{id}", content);
                return response.IsSuccessStatusCode;
            }
        }
    }
}
