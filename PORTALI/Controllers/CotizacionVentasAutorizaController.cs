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
            return View();
            //var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];

            //// Obtener listado de cotizaciones desde BD
            //List<PortalListadoCotizacionesEntity> listado = DALPortalCarritoCompras.getAllCotizacionesPendientes(sessions.UserId);

            //return View(listado);
        }

        public ActionResult CargarAutorizaciones() 
        {
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }

            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            List<PortalListadoCotizacionesEntity> listado = DALPortalCarritoCompras.getAllCotizacionesPendientes(sessions.UserId, sessions.SlpCode);
            return Json(listado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Detalle(int DocEntry, int? SlpCode)
        {
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }

            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            CarritoComprasPDFEntity detail = DALPortalCarritoCompras.getAllCotizacionesPendientesDetalleAutorizacion(DocEntry);
            detail.EsAsesor = "N";
            if (SlpCode > 0)
            {
                detail.EsAsesor = "Y";
            }
            return PartialView("iDetalle", detail);
        }

        [System.Web.Http.HttpPost]
        public ActionResult EnviarAutorizacionSAP(int DocEntry, int DocNum, int Estado, string Notas)
        {
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }

            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            AutorizaCotizacionEntity autorizaCotizacionEntity = new AutorizaCotizacionEntity();
            autorizaCotizacionEntity.DocEntry = DocEntry;
            autorizaCotizacionEntity.DocNum = DocNum;
            autorizaCotizacionEntity.Notas = Notas;
            autorizaCotizacionEntity.TipoAutoriza = Estado;
            autorizaCotizacionEntity.Usuario = sessions.UserCode;
            
            string contenido = DAL_API.CrearCotizacionVenta("CarritoCompras/EstadoDeAutorizacionCotizacion/", autorizaCotizacionEntity);
            Reply datos = JsonConvert.DeserializeObject<Reply>(contenido);
            return Json(datos, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Http.HttpPost]
        public ActionResult ActualizarCambiosDraft(List<Detalle> idraftEntity)
        {
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }

            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];

            DraftEntity draftEntity = new DraftEntity();
            draftEntity.Listado = idraftEntity;
            string contenido = DAL_API.CrearCotizacionVenta("CarritoCompras/UpdateCotizacionBorrado/", draftEntity);
            Reply datos = JsonConvert.DeserializeObject<Reply>(contenido);
            return Json(datos, JsonRequestBehavior.AllowGet);
        }
    }
}