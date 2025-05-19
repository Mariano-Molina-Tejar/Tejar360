using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entity;
using DAL;

namespace PORTALI.Controllers
{
    public class EspecialistaController : Controller
    {
        // GET: Especialista
        public ActionResult List()
        {
            return View();
        }

        public JsonResult BuscarNit(string Nit)
        {
            ClienteSATEntity cliente = DALPortalGenerales.ConsultarNIT(Nit);

            if (cliente != null)
            {
                return Json(new { nombre = cliente.Nombre }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = "No se encontró información para el NIT ingresado." }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult BuscarDPI(string DPI)
        {
            string nombre = DALPortalGenerales.BuscarDPI(DPI);
            if (nombre != null || nombre != "")
            {
                return Json(new { nombre = nombre }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = "No se encontró información para el NIT ingresado." }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}