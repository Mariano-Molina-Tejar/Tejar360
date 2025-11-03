using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entity;
using DAL;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PORTALI.Controllers
{
    public class VentasEmpresaController : Controller
    {
        DALPortalPresupuesto _dal = new DALPortalPresupuesto();
        // GET: VentasEmpresa
        public ActionResult VentasEmpresa()
        {
            //CargarData(Convert.ToDateTime("2025-03-01"), Convert.ToDateTime("2024-03-31"));
            return View();
        }

        public async Task<ActionResult> ReporteGerencia()
        {
            // Fecha de ayer
            DateTime ayer = DateTime.Today.AddDays(
            DateTime.Today.DayOfWeek == DayOfWeek.Monday ? -2 : -1
                        );

            // Fecha de inicio del mes actual
            DateTime inicioMes = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            if (inicioMes > ayer)
            {
                inicioMes = inicioMes.AddMonths(-1);
            }

            List<ReporteFinanciero> reporte = new List<ReporteFinanciero>();
            List<ReporteFinanciero> reporteCD = new List<ReporteFinanciero>();
            List<ReporteFinanciero> reporteINTER = new List<ReporteFinanciero>();

            reporte = await _dal.ObtenerRepoteFInanciero();
            reporteCD = await _dal.ObtenerRepoteFInancier_cd();
            reporteINTER = await _dal.ObtenerRepoteFInancier_inter();

            ViewBag.Ayer = ayer.ToString("dd-MM-yyyy");
            ViewBag.Inicio = inicioMes.ToString("dd-MM-yyyy");
            ViewBag.ReporteCD = reporteCD;
            ViewBag.ReporteInter = reporteINTER;
            return View(reporte);
        }


        [HttpPost]
        public JsonResult CargarData(DateTime FechaI, DateTime FechaF)
        {
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            if (sessions == null)
            {
                return Json(new { success = false, message = "Sesión expirada" });
            }
                
            var lista = DALPortalVentasEmpresa.getAllData(FechaI, FechaF, sessions.UserId);
            return Json(new { success = true, data = lista });
        }

        [HttpPost]
        public JsonResult DetalleUsuarios(DateTime FechaI, DateTime FechaF, string WhsCode)
        {

            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            if (sessions == null)
            {
                return Json(new { success = false, message = "Sesión expirada" });
            }

            var lista = DALPortalVentasEmpresa.getVentasAsesor(FechaI, FechaF, WhsCode);
            return Json(new { success = true, data = lista });
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SendMail(string Body)
        {
            try
            {
                object Reporte = new
                {
                    Title = "Reporte de ventas diario",
                    Body = Body
                };

                string url = "/Correo/EnvioReporte/";
                var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
                if (sessions == null)
                {
                    return Json(new { success = false, message = "Sesión expirada" });
                }

                string contenido = DAL_API.EnvioCorreoReporteVentasEmpresa(url, Reporte);

                Reply datos = JsonConvert.DeserializeObject<Reply>(contenido);

                return Json(new { success = true, data = datos.message });
            }
            catch (Exception ex)
            {
                //detallePresupuesto.Add(new DetallePresupuestoN3 { ErrorMessage = ex.Message });
                return Json(new { success = false, message = ex.Message });
            }
        }
    }

}