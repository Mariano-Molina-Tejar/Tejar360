using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using Entity;

namespace PORTALI.Controllers
{
    public class GerenciaRankinController : Controller
    {
        // GET: GerenciaRankin
        public ActionResult Index()
        {
            try
            {
                if (Session["PropertiesEntity"] == null)
                {
                    return View();
                }
                var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
                Session["PortalGerencialRankingTrimestral"] = DALPortalRankingTrimestral.Ranking(sessions.SlpCode);
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }
    }
}