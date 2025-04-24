using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;

namespace AgendaTuLookWeb.Servicios
{
	public class Seguridad : ISeguridad
	{

		private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClient;

        public Seguridad(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
			_configuration = configuration;
            _httpClient = httpClientFactory;
        }
		public string Encrypt(string texto)
		{
			byte[] iv = new byte[16];
			byte[] array;

			using (Aes aes = Aes.Create())
			{
				aes.Key = Encoding.UTF8.GetBytes(_configuration.GetSection("Variables:llaveCifrado").Value!);
				aes.IV = iv;

				ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
					{
						using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
						{
							streamWriter.Write(texto);
						}

						array = memoryStream.ToArray();
					}
				}
			}
			return Convert.ToBase64String(array);
		}

        //verifica el ReCaptcha
        public async Task<bool> VerificarReCaptcha(string response, string secret)
        {
            if (response != null && secret != null)
            {
                using (var http = _httpClient.CreateClient())
                {
                    //Esta url verifica si el token es valido
                    var url = "https://www.google.com/recaptcha/api/siteverify";

                    var parametros = new Dictionary<string, string>
                     {
                         { "secret", secret },
                         { "response", response }
                     };
                    var content = new FormUrlEncodedContent(parametros);

                    var confirmacion = await http.PostAsync(url, content);

                    if (confirmacion.IsSuccessStatusCode)
                    {

                        var StringResponse = await confirmacion.Content.ReadAsStringAsync();

                        //convertimos la respuesta en un json para poder evaluarla
                        var jsonResponse = JsonNode.Parse(StringResponse);
                        if (jsonResponse != null)
                        {
                            var success = (bool?)jsonResponse["success"];

                            if (success != null && success == true) return true;
                        }


                    }
                    else
                    {
                        Console.WriteLine("Error HTTP: " + confirmacion.StatusCode);
                    }

                }
            }
            //si todo fallo el token no es valido o no entro al metodo
            return false;

        }
    }
}

