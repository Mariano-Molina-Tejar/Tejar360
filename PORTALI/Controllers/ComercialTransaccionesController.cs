using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entity;
using DAL;

namespace PORTALI.Controllers
{
    public class ComercialTransaccionesController : Controller
    {
        // GET: ComercialTransacciones
        public ActionResult Index()
        {

            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
                
            return View();
        }

        public ActionResult ObtenerTransacciones(DateTime FechaI, DateTime FechaF, string WhsCode, string Region)
        {
            List<TransaccionesEntity> lista = new List<TransaccionesEntity>();
            lista = DALComercial.TransaccionesDiarias(FechaI, FechaF, WhsCode,Region);
            return Json(lista, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ObtenerTransaccionesAsesores(DateTime FechaI, DateTime FechaF, string WhsCode)
        {
            List<VendedorMeta> lista = new List<VendedorMeta>();

            if (WhsCode != "-1")
            {
                lista = DALComercial.TransaccionesPorAsesor(FechaI, FechaF, WhsCode);
            }

            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Top5(int Tipo, DateTime FechaI, DateTime FechaF, string WhsCode, string Region)
        {
            List<Top5TiendasEntity> lista = new List<Top5TiendasEntity>();
            lista = DALComercial.Top5Tiendas(FechaI, FechaF, Tipo, Region);
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Encabezado(DateTime FechaI, DateTime FechaF, string WhsCode, string Region)
        {
            ComercialTransaccionesEntity encabezado = new ComercialTransaccionesEntity();
            encabezado = DALComercial.EncabezadoTransacciones(FechaI, FechaF, WhsCode, Region);
            return Json(encabezado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListadoTiendas(int IdRegion, int UserId)
        {
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            List<ListaFiltroTiendasEntity> lista = DALFiltrosGenerales.ListaDeTiendas(UserId, IdRegion);
            return Json(lista, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListadoRegion(int IdUser)
        {
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            List<ListaFiltroRegionesEntity> lista = DALFiltrosGenerales.ListaDeRegiones(IdUser);
            return Json(lista, JsonRequestBehavior.AllowGet);
        }
    }
}