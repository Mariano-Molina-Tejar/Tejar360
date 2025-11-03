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
            // ✅ Verificamos si tiene AllowAnonymous
            var skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) ||
                                    filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);

            if (skipAuthorization)
            {
                // Si tiene AllowAnonymous, no hacemos nada (dejamos pasar)
                base.OnActionExecuting(filterContext);
                return;
            }

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
                // Si ya está logueado e intenta entrar a Login, redirigimos al Dashboard
                if (filterContext.Controller is AccountController && filterContext.ActionDescriptor.ActionName == "Login")
                {
                    filterContext.HttpContext.Response.Redirect("~/Dashboard/Index");
                }
            }

            base.OnActionExecuting(filterContext);
        }
        //public override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    // Obtenemos la sesión
        //    var oUser = (Entity.SessionLoginEntity)HttpContext.Current.Session["PropertiesEntity"];

        //    // Si la sesión es nula, redirigimos al login
        //    if (oUser == null)
        //    {
        //        // Evitamos redirigir desde el controlador de login
        //        if (!(filterContext.Controller is AccountController) || filterContext.ActionDescriptor.ActionName != "Login")
        //        {
        //            filterContext.HttpContext.Response.Redirect("~/Account/Login");
        //        }
        //    }
        //    else
        //    {
        //        // Si el usuario ya está logueado y está en Login, redirigimos al Dashboard
        //        if (filterContext.Controller is AccountController && filterContext.ActionDescriptor.ActionName == "Login")
        //        {
        //            filterContext.HttpContext.Response.Redirect("~/Dashboard/Index");
        //        }
        //    }

        //    base.OnActionExecuting(filterContext);
        //}
    }

}