using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PORTALI.Controllers
{
    public class CerrarSesionController : Controller
    {
        // GET: CerrarSesion
        public ActionResult LongOff()
        {
            Session["PropertiesEntity"] = null;
            return RedirectToAction("Login", "Account");
        }
    }
}