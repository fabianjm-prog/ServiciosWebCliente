using System.Text;
using Newtonsoft.Json;
using Cliente_ProyectoFinal.Models.Usuario;
using Cliente_ProyectoFinal.ApiUrls;



namespace Cliente_ProyectoFinal.Servicios
{
    public class class_AutenticacionServicio
    {



        public async Task<bool> RegistrarUsuarioAsync(class_User usuario)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(usuario);

                var content = new StringContent(json, Encoding.UTF8, "application/json");


                var response = await client.PostAsync(Class_Url.LoginUrl + "Acceso/Registrarse", content);


                return response.IsSuccessStatusCode;
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
