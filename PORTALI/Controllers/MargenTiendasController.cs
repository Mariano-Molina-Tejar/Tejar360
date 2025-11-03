using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entity;
using DAL;

namespace PORTALI.Controllers
{
    public class MargenTiendasController : Controller
    {
        // GET: MargenTiendas
        public ActionResult Detalle()
        {
            return View();
        }

        public ActionResult CargarData(DateTime FechaI, DateTime FechaF)
        {
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            if (sessions == null)
            {
                return Json(new { success = false, message = "Sesión expirada" });
            }

            var lista = DALMargenTiendas.ListadoDetalleMargen(FechaI, FechaF);
            return Json(new { success = true, data = lista });
        }
    }
}