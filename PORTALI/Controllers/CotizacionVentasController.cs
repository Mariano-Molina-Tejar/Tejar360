using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entity;
using DAL;

namespace PORTALI.Controllers
{
    public class CotizacionVentasController : Controller
    {
        // GET: CotizacionVentas
        public ActionResult Cotizacion()
        {
            List<BuscarProductoEntity> _listado = new List<BuscarProductoEntity>();
            return View(_listado);
        }

        public ActionResult BuscarProductos(string NombreProducto)
        {
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }

            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            List<BuscarProductoEntity> _listado = new List<BuscarProductoEntity>();
            _listado = DALPortalInventario.ListadoProductos(NombreProducto, sessions.WhsCode);
            return PartialView("_listadoProductos", _listado);
        }
        public ActionResult Detalles(string ItemCode)
        {
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            DetalleBusquedaProductosEntity detalle = new DetalleBusquedaProductosEntity();
            detalle.Encabezado = DALPortalInventario.DetalleSingle(ItemCode, "FTR-TZ1");
            detalle.Tiendas = DALPortalInventario.ListadoProductosStockTiendas(ItemCode, sessions.WhsCode);
            return View(detalle);
        }
    }
}