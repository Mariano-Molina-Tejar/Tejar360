using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using Entity;

namespace PORTALI.Controllers
{
    public class BilleteraComisionLigaController : Controller
    {
                
        public ActionResult ComisionLiga()
        {
            try
            {
                if (Session["PropertiesEntity"] == null)
                {
                    return View();
                }
                var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
                PortalComisionAsesoresEntity portalComisionAsesoresEntity = new PortalComisionAsesoresEntity(); 
                portalComisionAsesoresEntity = DALPortalBonoLiga.BonificacionLiga(0, 0, sessions.SlpCode, sessions.UserCode);
                portalComisionAsesoresEntity.MesActual = int.Parse(DateTime.Now.Month.ToString());
                portalComisionAsesoresEntity.AnioActual = int.Parse(DateTime.Now.Year.ToString());
                Session["BonoComisionEntity"] = portalComisionAsesoresEntity;
                return View(portalComisionAsesoresEntity);
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult ComisionLiga(int ListadoAsesores, int lstAnio, int ListaMeses)
        {
            try
            {
                if (Session["PropertiesEntity"] == null)
                {
                    return View();
                }
                
                PortalComisionAsesoresEntity portalComisionAsesoresEntity = new PortalComisionAsesoresEntity();
                portalComisionAsesoresEntity = DALPortalBonoLiga.BonificacionLiga(lstAnio, ListaMeses, ListadoAsesores, "manager");
                portalComisionAsesoresEntity.MesActual = ListaMeses;
                portalComisionAsesoresEntity.AnioActual = lstAnio;
                Session["BonoComisionEntity"] = portalComisionAsesoresEntity;
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }
    }
}