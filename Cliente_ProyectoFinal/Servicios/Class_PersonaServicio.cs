using Cliente_ProyectoFinal.ApiUrls;
using Cliente_ProyectoFinal.Models.Usuario;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Cliente_ProyectoFinal.Servicios
{
    public class Class_PersonaServicio
    {
        public async Task<bool> ActualizarPersonaAsync(string cedula, class_User persona, string token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var json = JsonConvert.SerializeObject(persona);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(Class_Url.UpdateUrl + $"Personas/ActualizarPersona/{cedula}", content);
                return response.IsSuccessStatusCode;
            }
        }
    }
}
