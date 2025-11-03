using DAL;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PORTALI.Controllers
{
    public class CrmVentasController : Controller
    {
        // GET: CrmVentas
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CargarData(int? IdRegion = null, string WhsCode = null, int? SlpCode = null, DateTime? FechaI = null, DateTime? FechaF = null, int? ReferidoSac = null)
        {
            List<CrmVentasEntity> ventasFac = new List<CrmVentasEntity>();
            ventasFac = DALCrmVentasFacturas.CrmVentasFacturas(IdRegion, WhsCode, SlpCode, FechaI, FechaF, ReferidoSac);
            return Json(ventasFac, JsonRequestBehavior.AllowGet);
        }
    }
}