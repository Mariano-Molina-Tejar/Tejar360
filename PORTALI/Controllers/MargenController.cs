using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entity;
using DAL;

namespace PORTALI.Controllers
{
    public class MargenController : Controller
    {
        // GET: Margen
        public ActionResult Margen(string whsCode)
        {
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }

            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            MargenesUtilidadEntity margenesUtilidadEntity = new MargenesUtilidadEntity();
            margenesUtilidadEntity.ListaTiendas = DALPortalGenerales.getAllTiendas(sessions.WhsCode, sessions.UserCode);
            
            if (!string.IsNullOrEmpty(whsCode))
            {
                margenesUtilidadEntity.ListaMargenes = DALPortalMargenUtilidad
                    .ListadoMargenAsesores(sessions.UserCode, sessions.WhsCode)
                    .Where(m => m.WhsCode == whsCode)
                    .ToList();
            }
            else
            {
                margenesUtilidadEntity.ListaMargenes = DALPortalMargenUtilidad.ListadoMargenAsesores(sessions.UserCode, sessions.WhsCode);
            }

            return View(margenesUtilidadEntity);
        }

        [HttpPost]
        public ActionResult Margen(string whsCode, bool isAjax = false)
        {
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }

            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            MargenesUtilidadEntity margenesUtilidadEntity = new MargenesUtilidadEntity();
            margenesUtilidadEntity.ListaTiendas = DALPortalGenerales.getAllTiendas(sessions.WhsCode, sessions.UserCode);

            // Filtrar según el whsCode recibido
            if (!string.IsNullOrEmpty(whsCode))
            {
                margenesUtilidadEntity.ListaMargenes = DALPortalMargenUtilidad
                    .ListadoMargenAsesores(sessions.UserCode, sessions.WhsCode)
                    .Where(m => m.WhsCode == whsCode)
                    .ToList();
            }
            else
            {
                margenesUtilidadEntity.ListaMargenes = DALPortalMargenUtilidad.ListadoMargenAsesores(sessions.UserCode, sessions.WhsCode);
            }

            // Si es una solicitud AJAX, devolver solo el cuerpo de la tabla
            if (isAjax)
            {
                return PartialView("_TablaMargenes", margenesUtilidadEntity.ListaMargenes);
            }

            return View(margenesUtilidadEntity);
        }

        [HttpPost]
        public JsonResult Detalle(int SlpCode)
        {
            if (Session["PropertiesEntity"] == null)
            {
                return Json(new { success = false, message = "Sesión expirada. Por favor, inicie sesión nuevamente." });
            }
            Session["SlpCode"] = SlpCode;
            return Json(new { success = true, redirectUrl = Url.Action("DetalleView", "Margen") });
        }

        public ActionResult DetalleView()
        {
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }

            var slpCode = (int)Session["SlpCode"];
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            List<ReporteDetalleProductoMargenEntity> listado = new List<ReporteDetalleProductoMargenEntity>();
            listado = DALPortalMargenUtilidad.ListadoProductosConMargen(slpCode, "");
            // Pasar los datos a la vista
            return View("DetalleProductos", listado);
        }

    }
}