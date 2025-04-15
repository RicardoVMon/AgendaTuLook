namespace AgendaTuLookWeb.Servicios
{
	public interface ISeguridad
	{
		public string Encrypt(string texto);
        public Task<bool> VerificarReCaptcha(string response, string secret);
    }
}
