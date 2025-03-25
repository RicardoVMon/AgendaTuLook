using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace AgendaTuLookWeb.Servicios
{
	public class Seguridad : ISeguridad
	{

		private readonly IConfiguration _configuration;

		public Seguridad(IConfiguration configuration)
		{
			_configuration = configuration;
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
	}
}
