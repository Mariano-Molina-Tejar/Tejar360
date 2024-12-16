using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using Entity;

namespace PORTALI.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            DashboardAsesoresEntity dashboardAsesoresEntity = new DashboardAsesoresEntity();            
            dashboardAsesoresEntity = DALPortalLaLigaDashboardAsesor.DashboardAsesores(sessions.SlpCode, Convert.ToDateTime("2024-12-01"), Convert.ToDateTime("2024-12-31"));
            return View(dashboardAsesoresEntity);
        }
    }
}