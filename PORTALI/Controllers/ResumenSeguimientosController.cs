using DAL;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PORTALI.Controllers
{
    public class ResumenSeguimientosController : Controller
    {
        // GET: ResumenSeguimientos
        public ActionResult Seguimientos()
        {
            return View();
        }
        public JsonResult CargarData(int? IdRegion = null, string WhsCode = null)
        {
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            if (sessions == null)
            {
                return Json(new { success = false, message = "Sesión expirada" });
            }

            var lista = DALResumenSeguimiento.ResumenSeguimiento(IdRegion, WhsCode, sessions.UserId);
            return Json(new { success = true, data = lista });
        }

        public JsonResult Detalle(string WhsCode)
        {
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            if (sessions == null)
            {
                return Json(new { success = false, message = "Sesión expirada" });
            }

            var lista = DALResumenSeguimiento.ResumenSeguimientoDetalle(WhsCode);
            return Json(new { success = true, data = lista });
        }
    }
}