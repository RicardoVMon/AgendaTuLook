namespace AgendaTuLookWeb.Servicios
{
	public interface IPictures
	{
		public Task<string> GuardarImagen(IFormFile imagen, string carpeta);
		public void EliminarImagen(string rutaImagen);
	}
}
