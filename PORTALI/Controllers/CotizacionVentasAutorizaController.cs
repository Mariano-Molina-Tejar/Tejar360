using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entity;
using DAL;
using Newtonsoft.Json;

namespace PORTALI.Controllers
{
    public class CotizacionVentasAutorizaController : Controller
    {
        // GET: CotizacionVentasAutoriza
        public ActionResult Autorizacion()
        {
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }

            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];

            // Obtener listado de cotizaciones desde BD
            List<PortalListadoCotizacionesEntity> listado = DALPortalCarritoCompras.getAllCotizacionesPendientes(sessions.SlpCode);

            return View(listado);
        }

        public ActionResult Detalle(int DocEntry)
        {            
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }

            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            CarritoComprasPDFEntity detail = DALPortalCarritoCompras.getAllCotizacionesPendientesDetalle(DocEntry, sessions.SlpCode);
            return View(detail);
        }

        [System.Web.Http.HttpPost]
        public ActionResult EnviarAutorizacionSAP(int DocEntry)
        {
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }

            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            string urlParametros = $"{DocEntry}";
            string contenido = DALPortalSolicitudCompra.NewSolicitud("CarritoCompras/GenerarCotizacionAutorizada/", null, urlParametros);
            
            Reply datos = JsonConvert.DeserializeObject<Reply>(contenido);
            return Json(datos, JsonRequestBehavior.AllowGet);
        }
    }
}