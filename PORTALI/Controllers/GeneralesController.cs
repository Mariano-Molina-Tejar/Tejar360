using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Entity;
using DAL;
using System.Web.Mvc;
using DAL;

namespace PORTALI.Controllers
{
    public class GeneralesController : Controller
    {
        public JsonResult ObtenerCotizacion(int DocEntry)
        {
            CarritoComprasEntity encabezadoVentaEditarEntity = new CarritoComprasEntity();
            encabezadoVentaEditarEntity = DALCarritoCotizaciones.CotizacionCompraEncabezado(DocEntry);

            //if (encabezadoVentaEditarEntity.productos == null)
            //{
            //    List<ProductoEditarEntity> productoEditarEntity = new List<ProductoEditarEntity>();
            //    encabezadoVentaEditarEntity.productos = productoEditarEntity;
            //}

            return Json(encabezadoVentaEditarEntity, JsonRequestBehavior.AllowGet);
        }


        public JsonResult DatosServer()
        {
            return Json(DALPortalGenerales.getDatosServer(), JsonRequestBehavior.AllowGet);
        }
    }
}