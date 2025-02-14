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
                                if (cotizacion.Borrador == "Y")
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

            string comentario = DataPdfCarrito.Encabezado.Notas?.ToString() ?? "";
            string asesor = GetAsesor(DataPdfCarrito.Encabezado.SlpName.ToString());

            //var htmlFilePath = Server.MapPath("~/Html/CotizacionVenta.html");
            var htmlFilePath = Server.MapPath("~/Html/cotizacionP1.html");
            var htmlFilePath2 = Server.MapPath("~/Html/cotizacionP2.html");

            var htmlContent = System.IO.File.ReadAllText(htmlFilePath);
            var htmlContent2 = System.IO.File.ReadAllText(htmlFilePath2);

            htmlContent = htmlContent.Replace("@DocNum", DataPdfCarrito.Encabezado.DocNum.ToString());
            htmlContent = htmlContent.Replace("@Nit", DataPdfCarrito.Encabezado.Nit.ToString());
            htmlContent = htmlContent.Replace("@FechaFactura", DataPdfCarrito.Encabezado.DocDate.ToString("dd/MM/yyyy"));
            htmlContent = htmlContent.Replace("@Hora", DataPdfCarrito.Encabezado.Hora);
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
            htmlContent = htmlContent.Replace("@Email_Cliente", DataPdfCarrito.Encabezado.CorreoCliente);
            htmlContent = htmlContent.Replace("@Telefono_Cliente", DataPdfCarrito.Encabezado.TelefonoCliente);
            htmlContent = htmlContent.Replace("@Direccion_Cliente", DataPdfCarrito.Encabezado.DomicilioCliente);
            htmlContent2 = htmlContent2.Replace("@Observaciones", comentario);
            htmlContent2 = htmlContent2.Replace("@Asesor", asesor);
            htmlContent2 = htmlContent2.Replace("@Codigo", DataPdfCarrito.Encabezado.SlpCode.ToString());
            htmlContent2 = htmlContent2.Replace("@Iva", DataPdfCarrito.Encabezado.Impuesto.ToString());



            string tabla = "";
            for (int i = 0; i < DataPdfCarrito.Detalle.Count; i++)
            {
                tabla += @"<tr>" +
                    "<td class= 'containerImg'><img src='" + DataPdfCarrito.Detalle[i].ImagenUrl.ToString() + "' title='Imagen' class='ItemImg' />" +
                    "<h5 style='display: inline; margin-left:5px;'>" + DataPdfCarrito.Detalle[i].Dscription.ToString().ToUpper() + " </h5> </td>" +
                    "<td style='text-align:right'>" + DataPdfCarrito.Detalle[i].Quantity.ToString("N0").ToUpper() + "</td>" + // Formatea la cantidad sin decimales
                    "<td style='text-align:right'>" + DataPdfCarrito.Detalle[i].Price.ToString("F2") + "</td>" + // Formatea el precio con dos decimales
                    "<td style='text-align:right'>" + DataPdfCarrito.Detalle[i].LineTotal.ToString("F2") + "</td></tr>"; // Formatea el total con dos decimales
            }

            //"<td style='text-align:left'> " + DataPdfCarrito.Detalle[i].Dscription.ToString().ToUpper() + "</td>" +
            htmlContent = htmlContent.Replace("@Detalle", tabla);

            var htmlContentTotal = htmlContent + "<div style='page-break-before: always;'></div>" + htmlContent2;


            // Configuración del convertidor
            HtmlToPdf converter = new HtmlToPdf();


            converter.Options.PdfPageSize = PdfPageSize.Letter;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
            converter.Options.MarginTop = 20;
            converter.Options.MarginRight = 20;
            converter.Options.MarginLeft = 20;
            converter.Options.MarginBottom = 20;

            converter.Options.DisplayFooter = true;
            converter.Footer.Height = 70;
            converter.Footer.DisplayOnEvenPages = false;
            converter.Footer.Add(new PdfHtmlSection(foot(), string.Empty));
           
            PdfDocument pdfDocument = converter.ConvertHtmlString(htmlContentTotal);
            

            var outputPath = Server.MapPath("~/Pdf/CotizacionVenta.pdf");
            pdfDocument.Save(outputPath);
            pdfDocument.Close();
          
            var fileStream = System.IO.File.ReadAllBytes(outputPath);
            return File(fileStream, "application/pdf");
        }

        private string foot()
        {
            return $@"
                <style>
                    .logo img {{
                    max-width: 100%;
                     height: auto;
                }}
                </style>
                    <div class=""logo"">
                         <img src=""https://eltejar.gt/wp-content/uploads/2025/02/PARTE-INFERIOR-PAG-1-08.png"" />
                    </div>
";

        }
        static string GetAsesor(string texto)
        {
            int index = texto.IndexOf('-'); // Encuentra la posición del "-"
            if (index != -1 && index + 1 < texto.Length)
            {
                return texto.Substring(index + 1).Trim(); // Obtiene el texto después del "-" y elimina espacios extra
            }
            return string.Empty; // Retorna vacío si no se encuentra el "-"
        }
    }
}