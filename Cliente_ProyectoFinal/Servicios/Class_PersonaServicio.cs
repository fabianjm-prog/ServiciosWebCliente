using Cliente_ProyectoFinal.ApiUrls;
using Cliente_ProyectoFinal.Models.Usuario;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace Cliente_ProyectoFinal.Servicios
{
    public class Class_PersonaServicio
    {

        public async Task<List<class_User>> ObtenerPersonasAsync(string token)
        {

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync(Class_Url.ReadUrl + "Personas/ListaPersonas");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var jobject = JObject.Parse(json);
                    var personas = jobject["value"]?.ToObject<List<class_User>>();
                    return personas ?? new List<class_User>();
                }

                return new List<class_User>();
            }
        }
        public async Task<class_User> BuscarPersonaAsync(string CedulaP, string token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync(Class_Url.ReadUrl + $"Personas/BuscarPersona/{CedulaP}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var jobject = JObject.Parse(json);
                    var persona = jobject["value"]?.ToObject<class_User>();
                    return persona;
                }
                return null;
            }
        }
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





