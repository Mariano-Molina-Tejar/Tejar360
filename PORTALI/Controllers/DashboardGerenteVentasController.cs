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
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            PortalDashboardGtEntity portalDashboardGtEntity = new PortalDashboardGtEntity();
            portalDashboardGtEntity = DALPortalDashboardGt.getDashboardVentasGt(sessions.WhsCode, DateTime.Now, DateTime.Now);
            return View(portalDashboardGtEntity);
        }
    }
}