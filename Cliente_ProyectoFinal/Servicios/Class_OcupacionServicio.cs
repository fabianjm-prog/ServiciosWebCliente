using Cliente_ProyectoFinal.ApiUrls;
using Cliente_ProyectoFinal.Models.Ocupaciones;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Cliente_ProyectoFinal.Servicios
{
    public class Class_OcupacionServicio
    {
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
