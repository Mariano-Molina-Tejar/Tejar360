using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using Entity;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;

namespace PORTALI.Controllers
{
    public class OrdenDeCompraController : Controller
    {
        // GET: OrdenDeCompra
        public ActionResult Nuevo()
        {
            return View();
        }        
        public ActionResult ListadoProveedores(string SocioDeNegocio = "")
        {
            var resultado = DALOrdenDeCompra.ListadoProveedores(SocioDeNegocio);
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListadoProductos(string Producto = "")
        {
            var resultado = DALOrdenDeCompra.ListadoProducto(Producto);
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListadoSucursales()
        {
            var resultado = DALOrdenDeCompra.ListadoSucursales();
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListadoCentroDeCosto()
        {
            var resultado = DALOrdenDeCompra.ListadoCentroDeCosto();
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public ActionResult TipoDeCambio()
        {
            var resultado = DALOrdenDeCompra.getTipoDeCambio();
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public ActionResult TipoArticulo()
        {
            var resultado = DALOrdenDeCompra.ListadoTipoArticulo();
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Filtro(string CardCode, DateTime? FechaI, DateTime? FechaF, int? DocNum)
        {
            var resultado = DALOrdenDeCompra.Filtro(CardCode, FechaI, FechaF, DocNum);
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getOc(int DocNum)
        {
            var resultado = DALOrdenDeCompra.getOc(DocNum);
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getItemName(string ItemCode)
        {   
            var resultado = DALOrdenDeCompra.getItemName(ItemCode);
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Anular()
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<hubs.NotificacionesHub>();
            context.Clients.Group("manager").recibirNotificacion(new
            {
                titulo = "Autorización cotización",
                mensaje = "Cotizacion #2253 Cliente: BAYRON LOPEZ",
                url = "/Ordenes/Detalle/123" // opcional: link directo
            });

            return null;
        }

        public ActionResult GuardarOC(OcEncabezadoEntity ocEncabezadoEntity)
        {
            try
            {
                if (Session["PropertiesEntity"] == null)
                {
                    return View();
                }

                var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
                string ruta = "OrdenDeCompra/New/";
                ocEncabezadoEntity.Usuario = sessions.UserCode;
                //string contenido = "";
                string contenido = DAL_API.CrearCotizacionVenta(ruta, ocEncabezadoEntity);
                Reply datos = JsonConvert.DeserializeObject<Reply>(contenido);
                if (datos == null)
                {
                    return Json(new
                    {
                        data = new
                        {
                            DocNum = 0,
                            DocEntry = 0
                        },
                        result = 0,
                        message = "Ha ocurrido un error, verique con sistemas"
                    });
                }
                else if (datos.result <= 0)
                {
                    return Json(new
                    {
                        data = new
                        {
                            DocNum = 0,
                            DocEntry = 0
                        },
                        result = 0,
                        message = datos.message
                    }); ;
                }

                // Convertimos data (que es object) a un tipo dinámico para acceder a sus propiedades
                var dataObj = JsonConvert.DeserializeObject<dynamic>(datos.data.ToString());

                var context = GlobalHost.ConnectionManager.GetHubContext<hubs.NotificacionesHub>();
                context.Clients.Group("manager").recibirNotificacion("Se ha creado una nueva orden de compra");
                return Json(new
                {
                    data = new
                    {
                        DocNum = (int)dataObj.DocNum,
                        DocEntry = (int)dataObj.DocEntry
                    },
                    result = 1,
                    message = datos.message
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    data = new { DocNum = 0, DocEntry = 0 },
                    result = 0,
                    message = ex.Message
                });
            }
        }
        public ActionResult GenerarPDF(int DocNum)
        {
            var modelo = DALOrdenDeCompra.ocImprimir(DocNum);
            return new Rotativa.ViewAsPdf("Oc", modelo)
            {
                FileName = "Orden de compra " + DocNum.ToString() + ".pdf",
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = new Rotativa.Options.Margins { Top = 5, Bottom = 5 }
            };
        }
    }
}