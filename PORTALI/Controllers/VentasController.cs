using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entity;
using DAL;
using SelectPdf;
using Newtonsoft.Json;

namespace PORTALI.Controllers
{
    public class VentasController : Controller
    {
        public ActionResult Listado()
        {
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }

            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];

            // Obtener listado de cotizaciones desde BD
            List<PortalListadoCotizacionesEntity> documents = DALPortalCarritoCompras.getAllCotizacionesVenta(sessions.SlpCode);            
            // Capturar todas las cookies con prefijo "T-OneCotizacion-"
            List<CarritoTempEntity> listadoCookie = new List<CarritoTempEntity>();
            foreach (string cookieKey in Request.Cookies.AllKeys)
            {
                if (cookieKey.StartsWith("T-OneCotizacion-"))  // Filtra solo las cookies con ese prefijo
                {
                    try
                    {
                        HttpCookie cookie = Request.Cookies[cookieKey];
                        if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
                        {
                            string cookieValue = HttpUtility.UrlDecode(cookie.Value); // Decodificar la cookie

                            // Deserializar como un solo objeto en lugar de una lista
                            var cotizacion = JsonConvert.DeserializeObject<CarritoTempEntity>(cookieValue);

                            if (cotizacion != null)
                            {
                                if(cotizacion.Borrador == "Y")
                                {
                                    PortalListadoCotizacionesEntity entidadTemp = new PortalListadoCotizacionesEntity();
                                    entidadTemp.Llave = cotizacion.Llave;
                                    entidadTemp.CardCode = cotizacion.CardCode;
                                    entidadTemp.CardName = cotizacion.CardName;
                                    entidadTemp.DocNum = cotizacion.NoCotizacion;
                                    entidadTemp.DocEntry = cotizacion.Llave;
                                    entidadTemp.SlpCode = sessions.SlpCode;
                                    entidadTemp.FacNombre = (cotizacion.FacturarNombre == "" ? "CONSUMIDOR FINAL" : cotizacion.FacturarNombre);
                                    entidadTemp.Nit = (cotizacion.FacturarNit == "" ? "CF" : cotizacion.FacturarNit);
                                    entidadTemp.DocDate = cotizacion.Fecha;
                                    entidadTemp.DocDueDate = cotizacion.Fecha;
                                    entidadTemp.DocTotal = cotizacion.Productos.Sum(p => p.LineTotal);
                                    entidadTemp.IsCookie = "Y";
                                    documents.Add(entidadTemp); // Agregar la cotización al listado
                                }                                
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al leer la cookie " + cookieKey + ": " + ex.Message);
                    }
                }
            }
            return View(documents);
        }
        
        [HttpPost]
        public ActionResult GeneratePdfAjax(int docEntry)
        {
            CarritoComprasPDFEntity DataPdfCarrito = DALPortalCarritoCompras.getDataCotizacionToPDF(docEntry);

            //var htmlFilePath = Server.MapPath("~/Html/CotizacionVenta.html");
            var htmlFilePath = Server.MapPath("~/Html/cotizacionP1.html");
            var htmlContent = System.IO.File.ReadAllText(htmlFilePath);
            htmlContent = htmlContent.Replace("@DocNum", DataPdfCarrito.Encabezado.DocNum.ToString());
            htmlContent = htmlContent.Replace("@Nit", DataPdfCarrito.Encabezado.Nit.ToString());
            htmlContent = htmlContent.Replace("@FechaFactura", DataPdfCarrito.Encabezado.DocDate.ToString("dd/MM/yyyy"));
            htmlContent = htmlContent.Replace("@FechaValida", DataPdfCarrito.Encabezado.DocDueDate.ToString("dd/MM/yyyy"));
            htmlContent = htmlContent.Replace("@CardCode", DataPdfCarrito.Encabezado.CardCode.ToUpper());
            htmlContent = htmlContent.Replace("@CardName", DataPdfCarrito.Encabezado.FacNombre.ToUpper());
            htmlContent = htmlContent.Replace("@Address", DataPdfCarrito.Encabezado.Address);            
            htmlContent = htmlContent.Replace("@DireccionTejar", DataPdfCarrito.Encabezado.DireccionTejar);
            htmlContent = htmlContent.Replace("@Notas", DataPdfCarrito.Encabezado.Notas);
            htmlContent = htmlContent.Replace("@SubTotal", DataPdfCarrito.Encabezado.SubTotal.ToString("F2"));
            htmlContent = htmlContent.Replace("@Impuestos", DataPdfCarrito.Encabezado.Impuesto.ToString("F2"));
            htmlContent = htmlContent.Replace("@Descuento", DataPdfCarrito.Encabezado.Descuento.ToString("F2"));
            htmlContent = htmlContent.Replace("@Total", DataPdfCarrito.Encabezado.DocTotal.ToString("F2"));

            string tabla = "";
            for (int i = 0; i < DataPdfCarrito.Detalle.Count; i++)
            {
                tabla += @"<tr>" +
                        "<td style='text-align:left'>" + DataPdfCarrito.Detalle[i].Dscription.ToString().ToUpper() + "</td>" +
                        "<td style='text-align:right'>" + DataPdfCarrito.Detalle[i].Quantity.ToString().ToUpper() + "</td>" +
                        "<td style='text-align:right'>" + DataPdfCarrito.Detalle[i].Price.ToString().ToUpper() + "</td>" +
                        "<td style='text-align:right'>" + DataPdfCarrito.Detalle[i].LineTotal.ToString().ToUpper() + "</td></tr>";
            }
            htmlContent = htmlContent.Replace("@Detalle", tabla);

            // Configuración del convertidor
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.PdfPageSize = PdfPageSize.Letter;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
            converter.Options.MarginTop = 20;
            converter.Options.MarginRight = 20;
            converter.Options.MarginLeft = 20;
            converter.Options.MarginBottom = 20;

            converter.Options.DisplayFooter = true;
            converter.Footer.Height = 150;

            converter.Footer.Add(new PdfHtmlSection(foot(DataPdfCarrito.Encabezado.SlpName), string.Empty));

            PdfDocument pdfDocument = converter.ConvertHtmlString(htmlContent);
            var outputPath = Server.MapPath("~/Pdf/CotizacionVenta.pdf");
            pdfDocument.Save(outputPath);
            pdfDocument.Close();

            // Devolver el archivo como respuesta al cliente
            var fileStream = System.IO.File.ReadAllBytes(outputPath);
            return File(fileStream, "application/pdf");
        }

        private string foot(string SlpName)
        {
            return $@"
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                    }}

                    .container {{            
                        background-color: white;
                        padding: 10px;
                        border-radius: 5px;            
                    }}

                    .header {{
                        background-color: #f7923a;
                        color: white;
                        padding: 10px;
                        border-radius: 5px 5px 0 0;
                        display: flex;
                        justify-content: space-between;
                        align-items: center;
                    }}

                        .header h1 {{
                            margin: 0;
                            font-size: 24px;
                        }}

                    .info {{
                        padding: 10px;
                        font-size: 14px;
                    }}

                        .info p {{
                            margin: 5px 0;
                        }}

                    .section {{
                        margin-top: 20px;
                    }}

                        .section h3 {{
                            border-bottom: 2px solid #f7923a;
                            padding-bottom: 5px;
                        }}

                    table {{
                        width: 100%;
                        border-collapse: collapse;
                        margin-top: 10px;
                    }}

                        table th, table td {{
                            border: 1px solid #ddd;
                            padding: 10px;
                            text-align: center;
                        }}

                        table th {{
                            background-color: #f7923a;
                            color: white;
                        }}

                    .totales {{
                        margin-top: 20px;
                        text-align: right;
                    }}

                        .totales p {{
                            margin: 5px 0;
                        }}

                    .total {{
                        font-weight: bold;
                        font-size: 18px;
                    }}

                    .firmas {{
                        margin-top: 40px;
                        display: flex;
                        justify-content: space-between;
                    }}

                        .firmas div {{
                            width: 45%;
                            text-align: center;
                            border-top: 1px solid black;
                            padding-top: 5px;
                        }}

                    .footer {{
                        margin-top: 20px;
                        padding: 15px;
                        background-color: #f7923a;
                        color: white;
                        text-align: center;
                        border-radius: 0 0 10px 10px;
                    }}
                </style>

                <div class=""section"">
                    <h3> Términos y Condiciones </h3>
                    <ul>
                        <li> La información interior no es una factura y solo es una estimación de los productos / servicios.</li>
                        <li> El pago debe efectuarse antes de la entrega de los productos / servicios.</li>
                    </ul>
                </div>            
                <div class=""footer"">
                    <p>Si tienes alguna pregunta sobre este presupuesto, ponte en contacto con</p>
                    <p><strong>{SlpName}</strong></p>
                    <p>¡Gracias por tu compra!</p>
                </div>";

        }
    }
}