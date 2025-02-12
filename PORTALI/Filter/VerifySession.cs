using PORTALI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PORTALI.Filter
{
    public class VerifySession : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Obtenemos la sesión
            var oUser = (Entity.SessionLoginEntity)HttpContext.Current.Session["PropertiesEntity"];

            // Si la sesión es nula, redirigimos al login
            if (oUser == null)
            {
                // Evitamos redirigir desde el controlador de login
                if (!(filterContext.Controller is AccountController) || filterContext.ActionDescriptor.ActionName != "Login")
                {
                    filterContext.HttpContext.Response.Redirect("~/Account/Login");
                }
            }
            else
            {
                // Si el usuario ya está logueado y está en Login, redirigimos al Dashboard
                if (filterContext.Controller is AccountController && filterContext.ActionDescriptor.ActionName == "Login")
                {
                    filterContext.HttpContext.Response.Redirect("~/Dashboard/Index");
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }

}