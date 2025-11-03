using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entity;
using DAL;

namespace PORTALI.Controllers
{
    public class EstadoBolsonController : Controller
    {
        // GET: EstadoBolson
        public ActionResult Bolson()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CargarData()
        {
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            if (sessions == null)
            {
                return Json(new { success = false, message = "Sesión expirada" });
            }

            var lista = DALBolsonEstado.BolsonActivoCotizaciones(null, null, null, sessions.UserId);
            return Json(new { success = true, data = lista });
        }

        [HttpPost]
        public JsonResult DetalleUsuarios(string WhsCode)
        {
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            if (sessions == null)
            {
                return Json(new { success = false, message = "Sesión expirada" });
            }

            var lista = DALBolsonEstado.BolsonActivoCotizacionesDetalle(null, WhsCode, null);
            return Json(new { success = true, data = lista });
        }
        public ActionResult BolsonAsesor()
        {
            // Esta acción solo muestra la vista, no devuelve datos
            return View();
        }

        public ActionResult ObtenerBolsonAsesor()
        {
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            if (sessions == null)
            {
                return Json(new { success = false, message = "Sesión expirada" }, JsonRequestBehavior.AllowGet);
            }

            var detalle = DALBolsonEstado.BolsonActivoCotizacionesAsesor(null, null, sessions.SlpCode);
            return Json(new { success = true, data = detalle }, JsonRequestBehavior.AllowGet);
        }
    }
}