using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using Entity;

namespace PORTALI.Controllers
{
    public class BolsonActivoController : Controller
    {
        // GET: BolsonActivo
        public ActionResult Listado()
        {
            return View();
        }

        public ActionResult CargarData(int? IdRegion = null, string WhsCode = null, int? SlpCode = null, DateTime? FechaI = null, DateTime? FechaF = null, int? ReferidoSac = null)
        {
            List<ListadoBolsonEntity> bolsonActivoEntity = new List<ListadoBolsonEntity>();
            bolsonActivoEntity = DALBolsonActivo.BolsonActivoCotizaciones(IdRegion, WhsCode, SlpCode, FechaI, FechaF, ReferidoSac);
            return Json(bolsonActivoEntity, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerDetalles(int DocEntry, int Tipo)
        {
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            List<PortalCotizacionesDetalleEntity> detalle = new List<PortalCotizacionesDetalleEntity>();
            CarritoComprasPDFEntity detail = DALPortalCarritoCompras.getAllCotizacionesPendientesDetalle(DocEntry, sessions.SlpCode, Tipo);
            detalle = detail.Detalle;
            return Json(detalle, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerHistorial(int DocEntry)
        {
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            List<CrmSeguimientoCotiEntity> detalle = new List<CrmSeguimientoCotiEntity>();
            detalle = DALBolsonActivo.HistorialSeg(DocEntry);
            return Json(detalle, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListadoRegiones()
        {
            if(Session["PropertiesEntity"] == null)
            {
                return View();
            }
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            List<ListaFiltroRegionesEntity> lista = DALFiltrosGenerales.ListaDeRegiones(sessions.UserId);
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAcciones() 
        {
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            List<AccionesEntity> lista = DALPortalGenerales.GetAllAcciones();
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListadoTiendas(int IdRegion)
        {
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            List<ListaFiltroTiendasEntity> lista = DALFiltrosGenerales.ListaDeTiendas(sessions.UserId, IdRegion);
            return Json(lista, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListaDeAsesores(int IdRegion, string WhsCode)
        {
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            List<ListaFiltroAsesoresEntity> lista = DALFiltrosGenerales.ListaDeAsesores(sessions.UserId, IdRegion, WhsCode);
            return Json(lista, JsonRequestBehavior.AllowGet);
        }        
    }
}