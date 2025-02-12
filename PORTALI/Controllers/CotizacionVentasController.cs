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

        public ActionResult BuscarProductos(string NombreProducto, bool Promos)
        {
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }

            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            List<BuscarProductoEntity> _listado = new List<BuscarProductoEntity>();
            _listado = DALPortalInventario.ListadoProductos(NombreProducto, sessions.WhsCode, Promos);
            return PartialView("_listadoProductos", _listado);
        }

        public ActionResult Detalles(string ItemCode)
        {
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }

            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            DetalleBusquedaProductosEntity detalle = new DetalleBusquedaProductosEntity();
            detalle.Encabezado = DALPortalInventario.DetalleSingle(ItemCode.Replace("'","-").Replace("Enter",""), sessions.WhsCode);
            detalle.Encabezado.Descuento = sessions.Descto;
            detalle.Tiendas = DALPortalInventario.ListadoProductosStockTiendas(ItemCode.Replace("'", "-").Replace("Enter", ""), sessions.WhsCode);
            detalle.Alternativos = DALPortalInventario.ListaProductosAltenativos(ItemCode.Replace("'", "-").Replace("Enter", ""), sessions.WhsCode);
            detalle.VentaCruzada = DALPortalInventario.ListaProductosVentaCruzada(ItemCode.Replace("'", "-").Replace("Enter", ""), sessions.WhsCode);
            detalle.ListPrecios = DALPortalInventario.ListaPreciosVentas(ItemCode);
            return View(detalle);
        }
    }
}