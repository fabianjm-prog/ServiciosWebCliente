﻿using Cliente_ProyectoFinal.ApiUrls;
using Cliente_ProyectoFinal.Models.Ocupaciones;
using Cliente_ProyectoFinal.Models;
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
                var response = await client.GetAsync(Class_Url.ReadUrl + "Persona/ListaPersonas");
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
                var response = await client.GetAsync(Class_Url.ReadUrl + $"Persona/BuscarPersona/{CedulaP}");
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
            
        public async Task<string> CrearPersonaAsync(class_User habi, string token)
        {


            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var json = JsonConvert.SerializeObject(habi);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(Class_Url.LoginUrl + "Acceso/Registrarse", content);
                var responseBody = await response.Content.ReadAsStringAsync();


                if (response.IsSuccessStatusCode)
                {
                    return null; // No error, todo bien
                }
                else
                {
                    // Deserializa y devuelve el mensaje de error
                    var errorResponse = JsonConvert.DeserializeObject<ApiResponse<object>>(responseBody);
                    return errorResponse?.mensaje ?? "Error desconocido al crear un usuario.";
                }
            }
        }
        public async Task<bool> EliminarUsuarioAsync(int id, string token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.DeleteAsync(Class_Url.DeleteUrl + $"Usuario/EliminarUsuario/{id}");
                return response.IsSuccessStatusCode;
            }
        }
        public async Task<class_User> BuscarPersonaPorCedulaAsync(string id, string token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync(Class_Url.ReadUrl + $"Persona/BuscarPersonaPorCedula/{id}");
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
        public async Task<bool> ActualizarPersonaAsync(string id, class_User persona, string token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var json = JsonConvert.SerializeObject(persona);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(Class_Url.UpdateUrl + $"Personas/ActualizarPersona/{id}", content);
                return response.IsSuccessStatusCode;
            }
        }

    }
}





