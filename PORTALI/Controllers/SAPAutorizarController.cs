using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using Entity;

namespace PORTALI.Controllers
{
    public class SAPAutorizarController : Controller
    {
        // GET: SAPAutorizar
        public ActionResult AutorizarSolicitudes()
        {
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }

            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            PortalPresupuestoEntity portalPresupuestoEntity = new PortalPresupuestoEntity();
            portalPresupuestoEntity = DALPortalGenerales.Presupuesto(sessions.Depto);
            return View(portalPresupuestoEntity);
        }

        public ActionResult CargarTabla()
        {
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            List<PortalSolicitudEncabezadoEntity> dataFinal = new List<PortalSolicitudEncabezadoEntity>();
            List<PortalSolicitudEncabezadoEntity> data1 = DALPortalSolicitudCompra.DetalleSolicitudCompra(sessions.UserCode, 1);
            List<PortalSolicitudEncabezadoEntity> data2 = DALPortalSolicitudCompra.DetalleSolicitudCompra(sessions.UserCode, 2);

            if(data1 != null)
            {
                dataFinal.AddRange(data1);
            }

            if(data2 != null)
            {
                dataFinal.AddRange(data2);
            }
            return PartialView("CargarTabla", dataFinal);
        }

        #region Muestra el detalle de la cotizacion abierta
        public ActionResult AutorizarSolicitudesDetalle(int DocEntry)
        {
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            CotizacionHeadEntity cotizacionCompra = new CotizacionHeadEntity();
            cotizacionCompra.totalCotizaciones = DALPortalSolicitudCompra.TotalCotizacionesSolicitudCompra(DocEntry, sessions.Depto.ToString());
            cotizacionCompra.HeadDetail = new CotizacionCompraEntity();
            cotizacionCompra.HeadDetail.CountCoti = cotizacionCompra.totalCotizaciones.Count();
            cotizacionCompra.HeadDetail.Detalle = new List<DetalleCompraEntity>();
            cotizacionCompra.Depto = sessions.Depto;
            cotizacionCompra.DocEntrySol = DocEntry;
            return View(cotizacionCompra);
        }
        #endregion

        [HttpPost]
        public ActionResult SinCotizar(int DocEntry, int Depto)
        {
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            CotizacionCompraEntity cotizacionCompra = new CotizacionCompraEntity();
            cotizacionCompra.CountCoti = 0;
            cotizacionCompra.Detalle = DALPortalSolicitudCompra.DetalleSolicitudCompra(DocEntry, Depto);
            cotizacionCompra.DetallePresupuesto = new List<DetallePresupuestoEntity>();
            if(cotizacionCompra.Detalle.Count > 0)
            {
                cotizacionCompra.DocNumSolC = cotizacionCompra.Detalle[0].DocNum;
            }
            
            if (cotizacionCompra == null)
            {
                return View(cotizacionCompra);
            }
            return PartialView("_detalle", cotizacionCompra);
        }

        [HttpPost]
        public ActionResult BuscarDetalleBoton(int DocEntryCoti)
        {
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            CotizacionCompraEntity cotizacionCompra = new CotizacionCompraEntity();
            cotizacionCompra = DALPortalCotizacionCompra.CotizacionCompraEncabezado(DocEntryCoti);
            cotizacionCompra.Detalle = DALPortalCotizacionCompra.CotizacionCompraDetalle(DocEntryCoti);
            cotizacionCompra.DetallePresupuesto = DALPortalSolicitudCompra.DetallePresupuesto(DocEntryCoti);
            cotizacionCompra.CountCoti = 1;

            if (cotizacionCompra == null)
            {
                return View(cotizacionCompra);
            }
            return PartialView("_detalle", cotizacionCompra);
        }

        [HttpPost]
        public ActionResult ValidaAutorizacion(int DocEntry, int DocEntrySol)//NUEVA VERSION V2
        {
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            SimpleEntity datos = new SimpleEntity();
            datos = DALPortalCotizaciones.ValidacionesCotizacionesCompras(sessions.Depto, DocEntry, sessions.CodeEmpleado, DocEntrySol);
            return Json(datos, JsonRequestBehavior.AllowGet);
        }


        //AQUI MANDA LA COTIZACION A GUARDAR DESDE ESTE PUNTO YA SE ESTA AUTORIZADO.
        [HttpPost]
        public ActionResult AutorizaCotizacion(int DocEntry, int DocEntrySc)
        {
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            Reply datos = new Reply();
            string urlParametros = $"{DocEntry}/{sessions.Depto}/{DocEntrySc}/{sessions.CodeEmpleado}";
            string contenido = DALPortalSolicitudCompra.NewSolicitud("CotizacionCompra/New/", null, urlParametros);
            datos = Newtonsoft.Json.JsonConvert.DeserializeObject<Reply>(contenido);
            return Json(datos, JsonRequestBehavior.AllowGet);
        }
        #region Guardar la autorizacion
        [HttpPost]
        public ActionResult SaveData(int DocEntry, string Observaciones, string TipoBoton)
        {
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }

            string Tipo = "";
            if (TipoBoton == "btnAutorizar")
            {
                Tipo = "A";
            }
            else if (TipoBoton == "btnRechazar")
            {
                Tipo = "R";
            }

            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            string urlParametros = $"{DocEntry}?Notas={Observaciones}&Tipo={Tipo}";
            string contenido = DALPortalSolicitudCompra.NewSolicitud("SolicitudCompra/New/", null, urlParametros);
            Reply datos = Newtonsoft.Json.JsonConvert.DeserializeObject<Reply>(contenido);
            if (datos.result == 1 && datos.data != null)
            {
                if(int.Parse(datos.data.ToString()) > 0) 
                {
                    var emailService = DALPortalGenerales.EnviarCorreo("[NUEVA SOLICITUD DE COMPRA #" + datos.data.ToString() + "]",
                    "desarrollo.eltejar@gmail.com", "DEPARTAMENTO DE " + sessions.DeptoName.ToString().ToUpper(), "Se ha generado una solicitud de compra.", "", false);
                }
            }
            return Json(datos, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}