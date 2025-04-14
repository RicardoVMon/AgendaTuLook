using AgendaTuLookWeb.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Security.Claims;
using System.Net.Http.Headers;
using AgendaTuLookWeb.Servicios;

namespace AgendaTuLookWeb.Controllers
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class AutenticacionController : Controller
    {

        private readonly IHttpClientFactory _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ISeguridad _seguridad;

        public AutenticacionController(IHttpClientFactory httpClient, IConfiguration configuration, ISeguridad seguridad)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _seguridad = seguridad;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UsuarioModel model)
        {

            if (model.RecaptchaToken != null)
            {
                var privatekey = _configuration.GetSection("RecaptchaSettings:private_key").Value;

                var tokenVerificado = await _seguridad.VerificarReCaptcha(model.RecaptchaToken, privatekey);
                //si el resultado es true hace login
                if (tokenVerificado)
                {
                    model.Contrasennia = _seguridad.Encrypt(model.Contrasennia!);
                    using (var http = _httpClient.CreateClient())
                    {
                        var url = _configuration.GetSection("Variables:urlWebApi").Value + "Autenticacion/Login";
                        var response = http.PostAsJsonAsync(url, model).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;

                            if (result != null)
                            {
                                if (result.Indicador)
                                {
                                    var datosResult = JsonSerializer.Deserialize<UsuarioModel>((JsonElement)result.Datos!);

                                    HttpContext.Session.SetString("Token", datosResult!.Token!);
                                    HttpContext.Session.SetString("UsuarioId", datosResult.UsuarioId.ToString()!);
                                    HttpContext.Session.SetString("Correo", datosResult.Correo!.ToString()!);
                                    HttpContext.Session.SetString("Nombre", datosResult.Nombre!.ToString());
                                    HttpContext.Session.SetString("RolId", datosResult.RolId!.ToString()!);
                                    return RedirectToAction("Index", "Home");
                                }
                            }

                            TempData["Mensaje"] = result!.Mensaje;
                            return View(model);
                        }
                    }


                }
                else
                {
					TempData["Mensaje"] = "No se pudo verificar el ReCaptcha correctamente, por favor, inténtalo de nuevo.";
                    return View();
                }

            }
			TempData["Mensaje"] = "Por favor, verifica el ReCaptcha para continuar.";
            return View();
        }

        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registro(UsuarioModel model)
        {

            if (model.RecaptchaToken != null)
            {
                var privatekey = _configuration.GetSection("RecaptchaSettings:private_key").Value;

                var tokenVerificado = await _seguridad.VerificarReCaptcha(model.RecaptchaToken, privatekey);
                //si el resultado es true hace login
                if (tokenVerificado)
                {
                    model.Contrasennia = _seguridad.Encrypt(model.Contrasennia!);

                    using (var http = _httpClient.CreateClient())
                    {
                        var url = _configuration.GetSection("Variables:urlWebApi").Value + "Autenticacion/Registro";
                        var response = http.PostAsJsonAsync(url, model).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;

                            if (!result!.Indicador)
                            {
                                TempData["Mensaje"] = result.Mensaje;
                                return View(model);
                            }

                            return RedirectToAction("Login", "Autenticacion");

                        }
                    }

                }
                else
                {
					TempData["Mensaje"] = "No se pudo verificar el ReCaptcha correctamente, por favor, inténtalo de nuevo.";
					return View();
                }

            }
			TempData["Mensaje"] = "Por favor, verifica el ReCaptcha para continuar.";
			return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Autenticacion");
        }

        #region Cambio Contraseña

        [HttpGet]
        public IActionResult RecuperarContrasennia()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RecuperarContrasennia(UsuarioModel model)
        {
            using (var http = _httpClient.CreateClient())
            {
                var url = _configuration.GetSection("Variables:urlWebApi").Value + "Autenticacion/RecuperarContrasennia";
                var response = await http.PostAsJsonAsync(url, model);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<RespuestaModel>();

                    if (result!.Indicador)
                    {
                        TempData["Mensaje"] = result.Mensaje;
                        HttpContext.Session.Clear();
                        HttpContext.Session.SetString("CorreoVerificacion", model.Correo!);
                        return RedirectToAction("CodigoVerificacion", "Autenticacion");
                    }
                    else
                    {
                        TempData["Mensaje"] = result.Mensaje;
                        return View();
                    }
                }
            }

            TempData["Mensaje"] = "Hubo un error al procesar la solicitud.";
            return View();
        }

        [HttpGet]
        public IActionResult CodigoVerificacion()
        {
            if (HttpContext.Session.GetString("CorreoVerificacion") == null)
            {
                return RedirectToAction("RecuperarContrasennia", "Autenticacion");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CodigoVerificacion(UsuarioModel model)
        {
            using (var http = _httpClient.CreateClient())
            {
                model.Correo = HttpContext.Session.GetString("CorreoVerificacion");
                var url = _configuration.GetSection("Variables:urlWebApi").Value + "Autenticacion/CodigoVerificacion";
                var response = await http.PostAsJsonAsync(url, model);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<RespuestaModel>();

                    if (result!.Indicador)
                    {
                        HttpContext.Session.SetString("CodigoValido", "true");
                        TempData["Mensaje"] = result.Mensaje;

                        return RedirectToAction("CambiarContrasennia", "Autenticacion");
                    }
                    else
                    {
                        TempData["Mensaje"] = result.Mensaje;
                        return View();
                    }
                }
            }

            TempData["Mensaje"] = "Hubo un error al procesar la solicitud.";
            return View();
        }

        [HttpGet]
        public IActionResult CambiarContrasennia()
        {
            if (HttpContext.Session.GetString("CorreoVerificacion") == null || HttpContext.Session.GetString("CodigoValido") == null)
            {
                return RedirectToAction(HttpContext.Session.GetString("CorreoVerificacion") == null ? "RecuperarContrasennia" : "CodigoVerificacion", "Autenticacion");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CambiarContrasennia(UsuarioModel model)
        {

            if (model.NuevaContrasennia != model.ConfirmarContrasennia)
            {
                TempData["Mensaje"] = "Las contraseñas no coinciden.";
                return View();
            }

            // Encriptar la nueva contraseña antes de enviarla al API
            model.Correo = HttpContext.Session.GetString("CorreoVerificacion");
            model.NuevaContrasennia = _seguridad.Encrypt(model.NuevaContrasennia!);

            using (var http = _httpClient.CreateClient())
            {
                var url = _configuration.GetSection("Variables:urlWebApi").Value + "Autenticacion/CambiarContrasenna";
                var response = await http.PostAsJsonAsync(url, model);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<RespuestaModel>();

                    if (result!.Indicador)
                    {
                        TempData["Mensaje"] = result.Mensaje;
                        HttpContext.Session.Clear();
                        return RedirectToAction("Login", "Autenticacion");
                    }
                    else
                    {
                        TempData["Mensaje"] = result.Mensaje;
                        return View();
                    }
                }
            }

            TempData["Mensaje"] = "Hubo un error al procesar la solicitud.";
            return View();
        }

        #endregion Cambio Contraseña

        #region Google

        [HttpGet]
        public async Task<IActionResult> GoogleCallback()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (result?.Principal == null)
            {
                return RedirectToAction("Login", "Autenticacion");
            }

            var claims = result.Principal.Claims;
            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var googleId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            // Call the API to register/authenticate the user with Google info
            using (var http = _httpClient.CreateClient())
            {
                var url = _configuration.GetSection("Variables:urlWebApi").Value + "Autenticacion/LoginWithGoogle";
                var model = new UsuarioModel
                {
                    Correo = email,
                    Nombre = name,
                    GoogleId = googleId
                };

                var response = await http.PostAsJsonAsync(url, model);

                if (response.IsSuccessStatusCode)
                {
                    var respuesta = await response.Content.ReadFromJsonAsync<RespuestaModel>();
                    if (respuesta != null && respuesta.Indicador)
                    {
                        var datosResult = JsonSerializer.Deserialize<UsuarioModel>((JsonElement)respuesta.Datos!);

                        HttpContext.Session.SetString("Token", datosResult!.Token!);
                        HttpContext.Session.SetString("UsuarioId", datosResult!.UsuarioId.ToString()!);
                        HttpContext.Session.SetString("Correo", datosResult!.Correo!);
                        HttpContext.Session.SetString("ProveedorAuth", datosResult!.ProveedorAuth!);
                        HttpContext.Session.SetString("Nombre", datosResult!.Nombre!);
                        HttpContext.Session.SetString("RolId", datosResult!.RolId.ToString()!);

                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            return RedirectToAction("Login", "Autenticacion");
        }

        [HttpGet]
        public async Task<IActionResult> LoginGoogle()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleCallback")
            };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public async Task<IActionResult> DesvincularGoogle(String correo)
        {
            using (var http = _httpClient.CreateClient())
            {
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
                var url = _configuration.GetSection("Variables:urlWebApi").Value + "Autenticacion/DesvincularGoogle?correo=" + correo;
                var response = await http.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<RespuestaModel>();

                    if (result!.Indicador)
                    {
                        TempData["successMessage"] = result.Mensaje;
                        HttpContext.Session.SetString("ProveedorAuth", "Email");
                        return RedirectToAction("EditarPerfilUsuario", "Usuarios", new { Id = HttpContext.Session.GetString("UsuarioId") });
                    }
                    TempData["errorMessage"] = result.Mensaje;
                    return RedirectToAction("EditarPerfilUsuario", "Usuarios", new { Id = HttpContext.Session.GetString("UsuarioId") });
                }
                return View();
            }
        }

        #endregion Google
    }
}