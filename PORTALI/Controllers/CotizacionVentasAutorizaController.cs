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
            List<PortalListadoCotizacionesEntity> listado = DALPortalCarritoCompras.getAllCotizacionesPendientes(sessions.UserId);

            return View(listado);
        }

        public ActionResult Detalle(int DocEntry)
        {            
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }

            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            CarritoComprasPDFEntity detail = DALPortalCarritoCompras.getAllCotizacionesPendientesDetalleAutorizacion(DocEntry);
            return View(detail);
        }

        [System.Web.Http.HttpPost]
        public ActionResult EnviarAutorizacionSAP(int DocEntry, string Estado, string Notas)
        {
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }

            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];

            AutorizaCotizacionEntity autorizaCotizacionEntity = new AutorizaCotizacionEntity();
            autorizaCotizacionEntity.DocEntry = DocEntry;
            autorizaCotizacionEntity.TipoAutoriza = (Estado == "A" ? 1 : 0);
            autorizaCotizacionEntity.Notas = Notas;
            autorizaCotizacionEntity.Usuario = sessions.UserCode;
            
            string contenido = DAL_API.CrearCotizacionVenta("CarritoCompras/GenerarCotizacionAutorizada/", autorizaCotizacionEntity);
            Reply datos = JsonConvert.DeserializeObject<Reply>(contenido);
            return Json(datos, JsonRequestBehavior.AllowGet);
        }
    }
}