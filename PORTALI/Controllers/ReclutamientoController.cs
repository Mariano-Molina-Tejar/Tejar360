using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DAL;
using Entity;

namespace PORTALI.Controllers
{
    public class ReclutamientoController : Controller
    {
        DALReclutamiento _dal = new DALReclutamiento();
        public async Task<ActionResult> Index()
        {
            try
            {
                var SolicitudesDePersonal = await _dal.VerSolicitudesDePersonal();
                return View(SolicitudesDePersonal);
            }
            catch(Exception ex)
            {
                ViewData["Error"] = ex.GetType();
                return View();
            }
        }
    }
}