//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Text;
//using System.Threading.Tasks;
//using System.Web;
//using System.Web.Mvc;
//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;
//using DAL;
//using DinkToPdf;
//using Entity;
//using Microsoft.AspNetCore.Http;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using SelectPdf;

//namespace PORTALI.Controllers
//{
//    public class SAPOcController : Controller
//    {
//        public ActionResult Oc(int? DocEntry)
//        {
//            if (Session["PropertiesEntity"] == null)
//            {
//                return View();
//            }

//            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
//            PortalMantSolicitudCompraEntity entidad = new PortalMantSolicitudCompraEntity();
//            entidad.ListaSucursales = DALPortalGenerales.ListadoSucursales();
//            entidad.ListadoDeptos = DALPortalGenerales.ListadoDeptos(sessions.Depto);
//            entidad.FechaHoy = DALPortalGenerales.DateTimeSystem().FechaHoy;
//            entidad.UserCode = sessions.UserCode;
//            return View(entidad);
//        }

//        [HttpPost]
//        public ActionResult EditSolicitud(int DocEntry)
//        {
//            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
//            SolicitudCompraEncabezadoEntity editarSol = new SolicitudCompraEncabezadoEntity();
//            editarSol = DALPortalSolicitudCompra.EditarSolicitud(sessions.UserCode, DocEntry);
//            editarSol.DetalleTabla = DALPortalSolicitudCompra.EditarSolicitudDetalle(DocEntry);
//            return Json(editarSol, JsonRequestBehavior.AllowGet);
//        }

//        public ActionResult SolicitudCompra()
//        {
//            if (Session["PropertiesEntity"] == null)
//            {
//                return View();
//            }
//            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
//            List<PortalSolicitudEncabezadoEntity> data = DALPortalSolicitudCompra.EncabezadoSolicitudCompra(sessions.Depto, "", "", 0);
//            return View(data);
//        }

//        #region BUSCA EL PRODUCTO
//        public ActionResult BuscarProducto(string textBuscar, string CardCode = null)
//        {
//            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
//            List<BusquedaDetalleProductoEntity> listado = new List<BusquedaDetalleProductoEntity>();
//            if (!string.IsNullOrEmpty(textBuscar))
//            {
//                listado = DALPortalGenerales.BusquedaDetalleProducto(textBuscar, sessions.Depto, CardCode);
//            }
//            return PartialView("_ListaProductos", listado);
//        }
//        #endregion

//        public ActionResult getDocNum(int IdBP)
//        {
//            PortalMantSolicitudCompraEntity entidad = new PortalMantSolicitudCompraEntity();
//            entidad.DocNum = DALPortalGenerales.DocNum(IdBP, 1470000113);
//            return Json(entidad, JsonRequestBehavior.AllowGet);
//        }

//        #region CREA LA SOLICITUD TEMPORAL
//        [HttpPost]
//        public ActionResult AddSolicitud()
//        {
//            try
//            {
//                if (Session["PropertiesEntity"] == null)
//                {
//                    return View();
//                }

//                var archivos = Request.Files;
//                List<HttpPostedFileBase> listaArchivos = new List<HttpPostedFileBase>();
//                for (int i = 0; i < archivos.Count; i++)
//                {
//                    listaArchivos.Add(archivos[i]);
//                }

//                var jsonData = Request.Form["jsonData"];
//                SolicitudCompraEncabezadoEntity solicitudCompraEncabezadoEntity = JsonConvert.DeserializeObject<SolicitudCompraEncabezadoEntity>(jsonData);
//                if (solicitudCompraEncabezadoEntity.DetalleTabla.Count == 1 && solicitudCompraEncabezadoEntity.DetalleTabla[0].ItemCode == "")
//                {
//                    solicitudCompraEncabezadoEntity.DetalleTabla = null;
//                }

//                if(listaArchivos.Count > 0)
//                {
//                    solicitudCompraEncabezadoEntity.Files = listaArchivos;
//                }

//                var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
//                string contenido = DALPortalSolicitudCompra.CrearSolicitud("SolicitudCompra/Add", solicitudCompraEncabezadoEntity);                
//                Reply datos = JsonConvert.DeserializeObject<Reply>(contenido);

//                if (datos.result == 1)
//                {
//                    var emailService = DALPortalGenerales.EnviarCorreo("[AUTORIZACION PARA SOLICITUD DE COMPRA #" + datos.data.ToString() + "]",
//                        "bayron.lopez@eltejar.com.gt", "DEPARTAMENTO DE " + sessions.DeptoName.ToString().ToUpper(), "Se ha creado una solicitud de compra, se necesita su autorización.", "", false);
//                }

//                if (datos.result == 101)
//                {
//                    List<string> lst = JsonConvert.DeserializeObject<List<string>>(datos.data.ToString());
//                    datos.data = lst;
//                }

//                var jasons = Json(datos, JsonRequestBehavior.AllowGet);
//                return jasons;
//            }
//            catch (Exception ex)
//            {
//                string mensa = ex.Message;
//                throw;
//            }

//            //string datos = "";
//            //var jasons = Json(datos, JsonRequestBehavior.AllowGet);
//            //return jasons;
//        }
//        #endregion

//        [HttpPost]
//        public ActionResult SubirData(IEnumerable<HttpPostedFileBase> files)
//        {
//            SubirArchivos(files);
//            var datos = "";
//            var jasons = Json(datos, JsonRequestBehavior.AllowGet);
//            return jasons;
//        }
//        protected string SubirArchivos(IEnumerable<HttpPostedFileBase> files)
//        {
//            if (files != null && files.Any())
//            {
//                foreach (var file in files)
//                {
//                    if (file != null && file.ContentLength > 0)
//                    {
//                        var path = Path.Combine("\\172.31.99.76\\SAPDocuments\\Anexos\\", Guid.NewGuid().ToString());                    
//                        file.SaveAs(path);
//                    }
//                }
//                return "Archivos subidos correctamente.";
//            }
//            return "Error al cargar los adjuntos";
//        }

//        public ActionResult DetallePendientes()
//        {
//            if (Session["PropertiesEntity"] == null)
//            {
//                return View();
//            }
//            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
//            //List<PortalSolicitudEncabezadoEntity> data = DALPortalSolicitudCompra.EncabezadoSolicitudCompra(sessions.UserCode, 0);            
//            List<PortalSolicitudEncabezadoEntity> data = new List<PortalSolicitudEncabezadoEntity>();
//            return Json(data, JsonRequestBehavior.AllowGet);
//        }

//        public ActionResult CargaDataExcel(string DataExcelJson)
//        {
//            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
//            GetAllProductsEntity Carga = new GetAllProductsEntity();
//            Carga = DALPortalSolicitudCompra.CargaExcelData(DataExcelJson, sessions.Depto);
//            return Json(Carga, JsonRequestBehavior.AllowGet);
//        }

//        [HttpGet]
//        public ActionResult getCentroCostos(int IdSucursal) 
//        {
//            if (Session["PropertiesEntity"] == null)
//            {
//                return View();
//            }
//            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
//            List<PortalTiendasEntity> tiendas = DALPortalGenerales.getAlmacenes(IdSucursal);
//            var jasons = Json(tiendas, JsonRequestBehavior.AllowGet);
//            return jasons;
//        }
        
//        public ActionResult ReporteOc() 
//        {
//            try
//            {
//                // Ruta del archivo .rpt de Crystal Reports
//                string reportPath = Server.MapPath("~/Reports/OrdenCompra.rpt");

//                // Crear una instancia del reporte
//                ReportDocument reportDocument = new ReportDocument();
//                reportDocument.Load(reportPath);

//                // Si el reporte necesita conexión a la base de datos
//                // reportDocument.SetDatabaseLogon("usuario", "contraseña", "servidor", "nombreBD");

//                // Pasar el reporte a la vista parcial a través del ViewBag o el modelo
//                ViewBag.ReportDocument = reportDocument;

//                // Retornar un PartialView con el visor de Crystal Report
//                return PartialView("VistaOc", reportDocument);                
//            }
//            catch (Exception ex)
//            {
//                // Manejo de errores
//                return new HttpStatusCodeResult(500, "Error generando el reporte: " + ex.Message);
//            }

//            //// Exportar el reporte a PDF
//            //Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
//            //return File(stream, "application/pdf", "Reporte.pdf");            
//        }

//        [HttpPost]
//        public ActionResult ValidarContratosMensaje(string ItemsCode)
//        {
//            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
//            bool validar = DALPortalSolicitudCompra.ValidarContratosItems(ItemsCode);
//            return Json(validar, JsonRequestBehavior.AllowGet);
//        }
        
//        public ActionResult BuscarSocioNegocio(string NombreSocio)
//        {
//            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
//            List<SocioNegocioEntity> listadoSociosNegocio = DALPortalGenerales.ListadoSocioNegocios(NombreSocio);
//            return PartialView("_SocioDeNegocio", listadoSociosNegocio);
//        }

//        [HttpPost]
//        public ActionResult GeneratePdfAjax(int docEntry)
//        {
//            PortalPdfOrdenCompraEntity DataPdfOrdenCompra = DALPortalGenerales.DataPdfOrdenCompra(docEntry);

//            var htmlFilePath = Server.MapPath("~/Html/OrdenCompra.html");
//            var htmlContent = System.IO.File.ReadAllText(htmlFilePath);
//            htmlContent = htmlContent.Replace("@Sucursal", DataPdfOrdenCompra.Sucursal.ToUpper());
//            htmlContent = htmlContent.Replace("@CardCode", DataPdfOrdenCompra.CardCode.ToUpper());
//            htmlContent = htmlContent.Replace("@CardName", DataPdfOrdenCompra.CardName.ToUpper());
//            htmlContent = htmlContent.Replace("@NitSucursal", DataPdfOrdenCompra.NitSucursal.ToUpper());
//            htmlContent = htmlContent.Replace("@CondicionesPago", DataPdfOrdenCompra.NombreGrupo);
//            htmlContent = htmlContent.Replace("@DocNum", DataPdfOrdenCompra.DocNum.ToString());
//            htmlContent = htmlContent.Replace("@Nit", DataPdfOrdenCompra.Nit);
//            htmlContent = htmlContent.Replace("@Femision", DataPdfOrdenCompra.FechaEmision.ToString("dd/MM/yyyy"));
//            htmlContent = htmlContent.Replace("@Fentrega", DataPdfOrdenCompra.FechaEntrega.ToString("dd/MM/yyyy"));
//            htmlContent = htmlContent.Replace("@GTotal", DataPdfOrdenCompra.GranTotal.ToString("#,##0.00"));
//            htmlContent = htmlContent.Replace("@Usuario", DataPdfOrdenCompra.ElaboradoPor.ToLower());
//            htmlContent = htmlContent.Replace("@Direccion", "");
//            htmlContent = htmlContent.Replace("@Correo", DataPdfOrdenCompra.Email.ToLower());
//            htmlContent = htmlContent.Replace("@Telefono", DataPdfOrdenCompra.Telefono.ToLower());
//            htmlContent = htmlContent.Replace("@Comentario", DataPdfOrdenCompra.Comentario);
//            htmlContent = htmlContent.Replace("@Entrega", DataPdfOrdenCompra.DirEntrega);

//            string tabla = "";
//            for (int i = 0; i < DataPdfOrdenCompra.Detalle.Count; i++)
//            {
//                tabla += @"<tr>
//                 <td>" + DataPdfOrdenCompra.Detalle[i].ItemCode + "</td>" +
//                                 "<td>" + DataPdfOrdenCompra.Detalle[i].Dscription + "</td>" +
//                                 "<td>" + DataPdfOrdenCompra.Detalle[i].Almacen + "</td>" +
//                                 "<td>" + DataPdfOrdenCompra.Detalle[i].Umedida + "</td>" +
//                                 "<td class='right'>" + DataPdfOrdenCompra.Detalle[i].Quantity.ToString("#,##0.00") + " </td>" +
//                                 "<td class='right'>" + DataPdfOrdenCompra.Detalle[i].Price.ToString("#,##0.00") + "</td>" +
//                                 "<td class='right'>" + DataPdfOrdenCompra.Detalle[i].LineTotal.ToString("#,##0.00") + "</td>" +
//                             "</tr>";
//            }
//            htmlContent = htmlContent.Replace("@Detalle", tabla);

//            // Configuración del convertidor
//            HtmlToPdf converter = new HtmlToPdf();
//            converter.Options.PdfPageSize = PdfPageSize.Letter;
//            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
//            converter.Options.MarginTop = 20;
//            converter.Options.MarginRight = 20;
//            converter.Options.MarginLeft = 20;
//            converter.Options.MarginBottom = 20;

//            converter.Options.DisplayFooter = true;
//            converter.Footer.Height = 125;            

//            converter.Footer.Add(new PdfHtmlSection(@"
//                    <div style='font-size: 14px;font-family: Arial, sans-serif; text-align: justify;'>
//                        <p><strong>Notas importantes:</strong></p>
//                        <ul style='list-style-type: none; padding: 0; margin: 0;'>
//                            <li><strong>1)</strong> Este documento constituye un contrato entre comprador y vendedor. El despacho de la mercadería se considera una aceptación del proveedor a las condiciones establecidas en el mismo (Destino, producto, cantidad y precio).</li>
//                            <li><strong>2)</strong> El único documento que respalda la entrega de su producto es el Ingreso a Bodega emitido por El Tejar (Firmado y Sellado), no se retire de la bodega sin su documento.</li>
//                            <li><strong>3)</strong> La reposición de su ingreso a bodega por extravío, sin responsabilidad de El Tejar, tiene un costo de Q500.00 por gastos administrativos.</li>
//                            <li><strong>4)</strong> Para que su factura sea recibida por Tesorería para pago, debe hacer referencia a la orden de compra de El Tejar, cuadrar con su ingreso a bodega y adjuntar el ingreso y la O.C.</li>
//                            <li><strong>5)</strong> Los días crédito correrán a partir de la emisión de la contraseña.</li>
//                        </ul>
//                        <div style='text-align:center'>
//                            <p>11 avenida 10-47 zona 1, Guatemala, Guatemala</p>
//                            <p>23276200</p>
//                        </div>
//                    </div>
//                ", string.Empty));


//            PdfDocument pdfDocument = converter.ConvertHtmlString(htmlContent);
//            var outputPath = Server.MapPath("~/Pdf/OrdenCompra.pdf");
//            pdfDocument.Save(outputPath);
//            pdfDocument.Close();

//            // Devolver el archivo como respuesta al cliente
//            var fileStream = System.IO.File.ReadAllBytes(outputPath);
//            return File(fileStream, "application/pdf");
//        }
//    }
//}