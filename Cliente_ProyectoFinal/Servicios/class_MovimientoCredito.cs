using Cliente_ProyectoFinal.ApiUrls;
using Cliente_ProyectoFinal.Models.MovimientoCredito;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Cliente_ProyectoFinal.Servicios
{
    public class class_MovimientoCredito
    {
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
