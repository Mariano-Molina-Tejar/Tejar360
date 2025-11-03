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
    public class BolsonClientesController : Controller
    {
        // GET: BolsonClientes
        DALBolsonClientes _dal = new DALBolsonClientes();

        public async Task<ActionResult> Index(int? mes, int? acctCode)
        {
            List<ReporteVentas> reporteVentas = new List<ReporteVentas>();
            try
            {
                var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
                if (sessions == null)
                {
                    return Json(new { success = false, message = "Sesión expirada" });
                }
                mes = !mes.HasValue ? DateTime.Now.Month : mes;
                int mesSeleccionado = mes ?? DateTime.Now.Month;

                reporteVentas = await _dal.ObtenerBolsonClientes(sessions.SlpCode.ToString());
                ViewBag.Encabezado = await _dal.ObtenerBolsonClientesEncabezado(sessions.SlpCode.ToString());
                ViewBag.MesSeleccionado = mesSeleccionado;
            }
            catch (Exception ex)
            {
                
            }

            return View(reporteVentas);
        }
    }
}