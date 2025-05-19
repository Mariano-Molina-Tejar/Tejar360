using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entity;
using DAL;

namespace PORTALI.Controllers
{
    public class DashboardGerenteVentasController : Controller
    {
        // GET: DashboardGerenteVentas
        public ActionResult DashboardGerenteVentas()
        {
            CargarData();
            return View();
        }

        public JsonResult CargarData()
        {
            if (Session["PropertiesEntity"] == null)
            {
                return Json(new { error = "Sesión no encontrada" }, JsonRequestBehavior.AllowGet);
            }

            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            if (sessions.Nivel == 2)
            {
                PortalDashboardGtEntity portalDashboardGtEntity = DALPortalDashboardGt.getDashboardVentasGt(sessions.WhsCode, DateTime.Now, DateTime.Now);
                return Json(portalDashboardGtEntity, JsonRequestBehavior.AllowGet);
            }

            return Json(new { error = "No autorizado" }, JsonRequestBehavior.AllowGet);
        }

    }
}