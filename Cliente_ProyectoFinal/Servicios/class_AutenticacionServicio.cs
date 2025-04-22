using System.Text;
using Newtonsoft.Json;
using Cliente_ProyectoFinal.Models.Usuario;
using Cliente_ProyectoFinal.ApiUrls;
using System.Net;
using Cliente_ProyectoFinal.Models;



namespace Cliente_ProyectoFinal.Servicios
{
    public class class_AutenticacionServicio
    {



        public async Task<string> RegistrarUsuarioAsync(class_User usuario)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(usuario);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(Class_Url.LoginUrl + "Acceso/Registrarse", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return null; // Sin error
                }
                else
                {
                    var errorResponse = JsonConvert.DeserializeObject<ApiResponse<object>>(responseContent);
                    return errorResponse?.mensaje ?? "Error desconocido al registrarse.";
                }
            }
        }

        public async Task<string> LoginAsync(class_Login usuario)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(usuario);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(Class_Url.LoginUrl + "Acceso/Login", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    dynamic jsonResponse = JsonConvert.DeserializeObject(responseContent);
                    return jsonResponse.token;
                }

                return null;
            }
        }


    }


}
