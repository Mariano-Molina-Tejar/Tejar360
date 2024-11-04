using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entity;
using DAL;
using System.Web.Routing;

namespace PORTALI.Controllers
{
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
                    return Json(new { success = false, message = "Usuario o contraseña incorrectos" });
                }
                else if (sessionLoginEntity.UserId == 0)
                {
                    return Json(new { success = false, message = "Usuario o contraseña incorrectos" });
                }
                else
                {
                    Session["PropertiesEntity"] = sessionLoginEntity;
                    string redirectUrl = sessionLoginEntity.Nivel == 2
                        ? Url.Action("DashboardGerenteVentas", "DashboardGerenteVentas")
                        : Url.Action("Index", "Dashboard");

                    return Json(new { success = true, redirectUrl });
                }
            }
            return Json(new { success = false, message = "Error al iniciar sesión." });
        }
    }
}