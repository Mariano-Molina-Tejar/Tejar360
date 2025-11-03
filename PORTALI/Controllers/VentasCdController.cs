using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using Entity;

namespace PORTALI.Controllers
{
    public class VentasCdController : Controller
    {
        // GET: VentasCd
        public ActionResult Cd()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CargarData(DateTime FechaI, DateTime FechaF)
        {
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            if (sessions == null)
            {
                return Json(new { success = false, message = "Sesión expirada" });
            }

            var lista = DALPortalVentasCd.getAllData(FechaI, FechaF, sessions.UserId);
            return Json(new { success = true, data = lista });
        }

        [HttpPost]
        public JsonResult DetalleUsuarios(DateTime FechaI, DateTime FechaF, string CardCode)
        {

            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            if (sessions == null)
            {
                return Json(new { success = false, message = "Sesión expirada" });
            }

            var lista = DALPortalVentasCd.getDetalleSingle(FechaI, FechaF, CardCode);
            return Json(new { success = true, data = lista });
        }
    }
}