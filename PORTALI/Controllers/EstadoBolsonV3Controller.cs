using DAL;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PORTALI.Controllers
{
    public class EstadoBolsonV3Controller : Controller
    {
        // GET: EstadoBolsonV3
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult CargarData(int Anio, int NoSemana, int Tipo, DateTime? FechaI, DateTime? FechaFinal)
        {
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            if (sessions == null)
            {
                return Json(new { success = false, message = "Sesión expirada" });
            }

            int? idRegion = null;
            string whsCode = null;

            if(sessions.SuperUser != "Y") 
            {
                idRegion = sessions.Region;
                whsCode = sessions.WhsCode;
            }

            var lista = DALEstadoBolsonV3.BolsonActivoV3(Anio, NoSemana, idRegion, whsCode,Tipo, FechaI, FechaFinal);
            return Json(new { success = true, data = lista });
        }

        public ActionResult CargarFechas(int Tipo, int Anio)
        {
            if (Tipo == 1) 
            {
                List<AnioEntity> anios = new List<AnioEntity>();
                anios = DALPortalGenerales.GetAllAnios(1);
                return Json(anios, JsonRequestBehavior.AllowGet);
            }
            else if(Tipo == 2)
            {
                List<SemanasEntity> semanas = new List<SemanasEntity>();
                semanas = DALPortalGenerales.getAllSemanas(2, Anio);
                return Json(semanas, JsonRequestBehavior.AllowGet);
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DetalleUsuarios(string WhsCode, int Anio, int Semana, int Tipo, DateTime? FechaI, DateTime? FechaFinal) 
        {
            List<EstadoBolsonV3Entity> anios = new List<EstadoBolsonV3Entity>();
            anios = DALEstadoBolsonV3.BolsonActivoDetalleV3(WhsCode, Anio, Semana, Tipo, FechaI, FechaFinal);
            return Json(anios, JsonRequestBehavior.AllowGet);
        }
    }
}