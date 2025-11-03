using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entity;
using DAL;
using System.Web.Routing;
using System.Web.Security;

namespace PORTALI.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Login(LoginEntity loginEntity)
        {
            SessionLoginEntity sessionLoginEntity = new SessionLoginEntity();
            if (ModelState.IsValid)
            {
                sessionLoginEntity = DALPortalLogin.SessionLogin(loginEntity, Server.MapPath("~/ImgUp/"));
                if (sessionLoginEntity == null)
                {
                    return Json(new { success = false, data = "N", message = "Usuario o contraseña incorrectos" });
                }
                else if (sessionLoginEntity.UserId == 0)
                {
                    return Json(new { success = false, data = "N", message = "Usuario o contraseña incorrectos" });
                }
                else
                {                    
                    if (sessionLoginEntity.CambiarContrasena == "Y")
                    {
                        return Json(new { success = true, data = "Y", message = "Cambio de contraseña" });
                    }

                    Session["PropertiesEntity"] = sessionLoginEntity;
                    FormsAuthentication.SetAuthCookie(sessionLoginEntity.UserCode, false);
                    string redirectUrl = sessionLoginEntity.Nivel == 2 ? Url.Action("DashboardGerenteVentas", "DashboardGerenteVentas") : Url.Action("Cotizacion", "CotizacionVentas");
                    return Json(new { success = true, data = "N", redirectUrl });
                }
            }
            return Json(new { success = false, data = "N", message = "Error al iniciar sesión." });
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult CambioDeContrasena(string Usuario, string NewPass)
        {
            try
            {
                if (ValidarContrasena(Usuario, NewPass))
                {
                    return Json(new { success = false, data = "N", message = "La nueva contraseña debe ser diferente a la que tiene registrada." });
                }

                bool resultado = DALPortalLogin.CambioContrasena(Usuario, NewPass);
                if (resultado)
                {
                    return Json(new { success = true, data = "S", message = "Contraseña cambiada correctamente." });
                }
                else
                {
                    return Json(new { success = false, data = "N", message = "No se pudo cambiar la contraseña." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = "N", message = ex.Message });
            }
        }

        public bool ValidarContrasena(string Usuario, string NewPass) 
        {
            try
            {
                return DALPortalLogin.ValidarMismaContrasena(Usuario, NewPass);
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}