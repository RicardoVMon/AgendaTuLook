using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace AgendaTuLookWeb.Models
{
	public class FiltroSesion : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			var token = context.HttpContext.Session.GetString("Token");

			if (string.IsNullOrEmpty(token))
			{
				context.Result = new RedirectToRouteResult(new { controller = "Autenticacion", action = "Login" });
			}
			base.OnActionExecuting(context);
		}

	}
}
