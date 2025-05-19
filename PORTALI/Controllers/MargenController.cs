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

            margenesUtilidadEntity.MargenTienda = DALPortalMargenUtilidad.ReporteUtilidadTienda(sessions.UserCode,sessions.WhsCode);

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
            List<Reporte_Margen_Facturas> listado = new List<Reporte_Margen_Facturas>();
            listado = DALPortalMargenUtilidad.ListadoFacturasConMargen(slpCode, "");
            // Pasar los datos a la vista
            return View("DetalleProductos", listado);
        }

        public ActionResult CargarDetalleFactura(int DocNum, string Tipo)
        {
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }

            List<Detalle_Margen_Factura> detalleFactura = new List<Detalle_Margen_Factura>();
            detalleFactura = DALPortalMargenUtilidad.DetalleFactura(DocNum, Tipo);


            return PartialView("_DetalleFactura", detalleFactura);
        }

    }
}

