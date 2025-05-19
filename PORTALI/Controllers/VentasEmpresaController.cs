using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entity;
using DAL;

namespace PORTALI.Controllers
{
    public class VentasEmpresaController : Controller
    {
        // GET: VentasEmpresa
        public ActionResult VentasEmpresa()
        {
            //CargarData(Convert.ToDateTime("2025-03-01"), Convert.ToDateTime("2024-03-31"));
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

            var lista = DALPortalVentasEmpresa.getAllData(FechaI, FechaF, sessions.UserId);
            return Json(new { success = true, data = lista });
        }

        [HttpPost]
        public JsonResult DetalleUsuarios(DateTime FechaI, DateTime FechaF, string WhsCode)
        {

            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            if (sessions == null)
            {
                return Json(new { success = false, message = "Sesión expirada" });
            }

            var lista = DALPortalVentasEmpresa.getVentasAsesor(FechaI, FechaF, WhsCode);
            return Json(new { success = true, data = lista });
        }

    }
}