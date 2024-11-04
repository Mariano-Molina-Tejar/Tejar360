using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using DAL;
using Entity;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PORTALI.Controllers
{
    public class SAPOcController : Controller
    {
        public ActionResult Oc(int? DocEntry)
        {
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }

            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            PortalMantSolicitudCompraEntity entidad = new PortalMantSolicitudCompraEntity();
            entidad.ListaSucursales = DALPortalGenerales.ListadoSucursales();
            entidad.ListadoDeptos = DALPortalGenerales.ListadoDeptos(sessions.Depto);
            entidad.FechaHoy = DALPortalGenerales.DateTimeSystem().FechaHoy;
            entidad.UserCode = sessions.UserCode;
            return View(entidad);
        }

        [HttpPost]
        public ActionResult EditSolicitud(int DocEntry)
        {
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            SolicitudCompraEncabezadoEntity editarSol = new SolicitudCompraEncabezadoEntity();
            editarSol = DALPortalSolicitudCompra.EditarSolicitud(sessions.UserCode, DocEntry);
            editarSol.DetalleTabla = DALPortalSolicitudCompra.EditarSolicitudDetalle(DocEntry);
            return Json(editarSol, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SolicitudCompra()
        {
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            List<PortalSolicitudEncabezadoEntity> data = DALPortalSolicitudCompra.EncabezadoSolicitudCompra(sessions.Depto, "", "", 0);
            return View(data);
        }
        #region BUSCA EL PRODUCTO
        public ActionResult BuscarProducto(string textBuscar)
        {
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            List<BusquedaDetalleProductoEntity> listado = new List<BusquedaDetalleProductoEntity>();
            if (!string.IsNullOrEmpty(textBuscar))
            {
                listado = DALPortalGenerales.BusquedaDetalleProducto(textBuscar, sessions.Depto);
            }
            return PartialView("_ListaProductos", listado);
        }
        #endregion

        public ActionResult getDocNum(int IdBP)
        {
            PortalMantSolicitudCompraEntity entidad = new PortalMantSolicitudCompraEntity();
            entidad.DocNum = DALPortalGenerales.DocNum(IdBP, 1470000113);
            return Json(entidad, JsonRequestBehavior.AllowGet);
        }

        #region CREA LA SOLICITUD TEMPORAL
        [HttpPost]
        public ActionResult AddSolicitud()
        {
            try
            {
                if (Session["PropertiesEntity"] == null)
                {
                    return View();
                }

                var archivos = Request.Files;
                List<HttpPostedFileBase> listaArchivos = new List<HttpPostedFileBase>();
                for (int i = 0; i < archivos.Count; i++)
                {
                    listaArchivos.Add(archivos[i]);
                }

                var jsonData = Request.Form["jsonData"];
                SolicitudCompraEncabezadoEntity solicitudCompraEncabezadoEntity = JsonConvert.DeserializeObject<SolicitudCompraEncabezadoEntity>(jsonData);
                if (solicitudCompraEncabezadoEntity.DetalleTabla.Count == 1 && solicitudCompraEncabezadoEntity.DetalleTabla[0].ItemCode == "")
                {
                    solicitudCompraEncabezadoEntity.DetalleTabla = null;
                }

                if(listaArchivos.Count > 0)
                {
                    solicitudCompraEncabezadoEntity.Files = listaArchivos;
                }

                var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
                string contenido = DALPortalSolicitudCompra.CrearSolicitud("SolicitudCompra/Add", solicitudCompraEncabezadoEntity);
                Reply datos = JsonConvert.DeserializeObject<Reply>(contenido);

                if (datos.result == 1)
                {
                    var emailService = DALPortalGenerales.EnviarCorreo("[AUTORIZACION PARA SOLICITUD DE COMPRA #" + datos.data.ToString() + "]",
                        "bayron.lopez@eltejar.com.gt", "DEPARTAMENTO DE " + sessions.DeptoName.ToString().ToUpper(), "Se ha creado una solicitud de compra, se necesita su autorización.", "", false);
                }

                if (datos.result == 101)
                {
                    List<string> lst = JsonConvert.DeserializeObject<List<string>>(datos.data.ToString());
                    datos.data = lst;
                }

                var jasons = Json(datos, JsonRequestBehavior.AllowGet);
                return jasons;
            }
            catch (Exception ex)
            {
                string mensa = ex.Message;
                throw;
            }

            //string datos = "";
            //var jasons = Json(datos, JsonRequestBehavior.AllowGet);
            //return jasons;
        }
        #endregion

        [HttpPost]
        public ActionResult SubirData(IEnumerable<HttpPostedFileBase> files)
        {
            SubirArchivos(files);
            var datos = "";
            var jasons = Json(datos, JsonRequestBehavior.AllowGet);
            return jasons;
        }
        protected string SubirArchivos(IEnumerable<HttpPostedFileBase> files)
        {
            if (files != null && files.Any())
            {
                foreach (var file in files)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        var path = Path.Combine("\\172.31.99.76\\SAPDocuments\\Anexos\\", Guid.NewGuid().ToString());                    
                        file.SaveAs(path);
                    }
                }
                return "Archivos subidos correctamente.";
            }
            return "Error al cargar los adjuntos";
        }

        public ActionResult DetallePendientes()
        {
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            //List<PortalSolicitudEncabezadoEntity> data = DALPortalSolicitudCompra.EncabezadoSolicitudCompra(sessions.UserCode, 0);            
            List<PortalSolicitudEncabezadoEntity> data = new List<PortalSolicitudEncabezadoEntity>();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargaDataExcel(string DataExcelJson)
        {
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            GetAllProductsEntity Carga = new GetAllProductsEntity();
            Carga = DALPortalSolicitudCompra.CargaExcelData(DataExcelJson, sessions.Depto);
            return Json(Carga, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult getCentroCostos(int IdSucursal) 
        {
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            List<PortalTiendasEntity> tiendas = DALPortalGenerales.getAlmacenes(IdSucursal);
            var jasons = Json(tiendas, JsonRequestBehavior.AllowGet);
            return jasons;
        }
        
        public ActionResult ReporteOc() 
        {
            try
            {
                // Ruta del archivo .rpt de Crystal Reports
                string reportPath = Server.MapPath("~/Reports/OrdenCompra.rpt");

                // Crear una instancia del reporte
                ReportDocument reportDocument = new ReportDocument();
                reportDocument.Load(reportPath);

                // Si el reporte necesita conexión a la base de datos
                // reportDocument.SetDatabaseLogon("usuario", "contraseña", "servidor", "nombreBD");

                // Pasar el reporte a la vista parcial a través del ViewBag o el modelo
                ViewBag.ReportDocument = reportDocument;

                // Retornar un PartialView con el visor de Crystal Report
                return PartialView("VistaOc", reportDocument);                
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return new HttpStatusCodeResult(500, "Error generando el reporte: " + ex.Message);
            }

            //// Exportar el reporte a PDF
            //Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            //return File(stream, "application/pdf", "Reporte.pdf");            
        }
    }
}