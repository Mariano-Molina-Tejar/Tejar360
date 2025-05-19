using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entity;
using DAL;

namespace PORTALI.Controllers
{
    public class KpiCrmController : Controller
    {
        // GET: KpiCrm
        public ActionResult Crm()
        {
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }
            return View();
        }

        public ActionResult CargarDataInicial(DateTime FechaI, DateTime FechaF)
        {
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            CrmCotiAnalisisEntity crmCotizaciones =
                DALCrmCotizacionesAnalisis.CrmCotizaciones(Convert.ToDateTime(FechaI), Convert.ToDateTime(FechaF), sessions.SlpCode);
            return Json(crmCotizaciones, JsonRequestBehavior.AllowGet);
        }
    }
}