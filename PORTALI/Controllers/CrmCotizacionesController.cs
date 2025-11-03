using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entity;
using DAL;

namespace PORTALI.Controllers
{
    public class CrmCotizacionesController : Controller
    {
        // GET: CrmCotizaciones
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult getDataMain(DateTime fechaI, DateTime fechaF)
        {
            List<CrmCotizacionesEntity> lista = DALCrmCotizacionesAnalisis.CrmCotizaciones(fechaI, fechaF);

            return Json(lista, JsonRequestBehavior.AllowGet);
        }
    }
}