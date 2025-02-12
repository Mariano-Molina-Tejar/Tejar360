using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entity;
using DAL;
using PORTALI.Filter;

namespace PORTALI.Controllers
{
    [VerifySession]
    public class RankingV2Controller : Controller
    {
        // GET: RankingV2
        public ActionResult Index()
        {
            return View();
            //var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            //List<RankingV2Entity> rankingV2Entities = new List<RankingV2Entity>();
            //rankingV2Entities = DALPortalRankingTrimestral.RankingV2(sessions.SlpCode);
            //return View(rankingV2Entities);
        }
        public JsonResult GetRankingData()
        {
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            var rankingV2Entities = DALPortalRankingTrimestral.RankingV2(sessions.SlpCode);
            return Json(rankingV2Entities, JsonRequestBehavior.AllowGet);
        }
    }
}