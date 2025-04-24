namespace AgendaTuLookWeb.Servicios
{
	public class Pictures: IPictures
	{
		public void EliminarImagen(string rutaImagen)
		{
			if (!string.IsNullOrEmpty(rutaImagen))
			{
				var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", rutaImagen.TrimStart('/'));
				if (System.IO.File.Exists(filePath))
				{
					System.IO.File.Delete(filePath);
				}
			}
		}

		public async Task<string> GuardarImagen(IFormFile imagen, string carpeta)
		{
			// Ruta relativa a partir de wwwroot
			var rutaRelativa = Path.Combine("img", carpeta);

			// Ruta completa usando ContentRootPath (independiente de la ubicación del repo)
			var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", rutaRelativa);

			if (!Directory.Exists(uploadsFolder))
			{
				Directory.CreateDirectory(uploadsFolder);
			}

			var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(imagen.FileName)}";
			var filePath = Path.Combine(uploadsFolder, uniqueFileName);

			using (var fileStream = new FileStream(filePath, FileMode.Create))
			{
				await imagen.CopyToAsync(fileStream);
			}

			return $"/img/{carpeta}/{uniqueFileName}";
		}
	}
}
