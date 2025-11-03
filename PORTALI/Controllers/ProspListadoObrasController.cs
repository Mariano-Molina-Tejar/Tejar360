using DAL;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PORTALI.Controllers
{
    public class ProspListadoObrasController : Controller
    {
        // GET: ProspListadoObras
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> ObtenerVisitas()
        {
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }

            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            // Esperamos el resultado de forma asíncrona
            List<ProspListadoVisitasEntity> lista = await DALObras.ListadoVisitas(sessions.SlpCode);
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> ObtenerContactos(int NoVisita)
        {
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }

            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            // Esperamos el resultado de forma asíncrona
            List<ListadoVisitasContactosEntity> lista = await DALObras.ListadoContactos(NoVisita);
            return Json(lista, JsonRequestBehavior.AllowGet);
        }
    }
}