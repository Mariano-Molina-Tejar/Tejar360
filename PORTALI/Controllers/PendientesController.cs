using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PORTALI.Controllers
{
    public class PendientesController : Controller
    {
        // GET: Pendientes
        public ActionResult Index()
        {
            return View();
        }
    }
}