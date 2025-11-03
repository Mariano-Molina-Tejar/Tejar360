using DAL;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PORTALI.Controllers
{
    public class EstadoBolsonV2Controller : Controller
    {
        // GET: EstadoBolsonV2
        public ActionResult Detalle()
        {
            return View();
        }

        public JsonResult CargarData()
        {
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            if (sessions == null)
            {
                return Json(new { success = false, message = "Sesión expirada" });
            }

            var lista = DALEstadoBolsonV2.BolsonActivoCotizacionesDetalle(sessions.Region, sessions.WhsCode, sessions.UserId);
            return Json(new { success = true, data = lista });
        }
    }
}