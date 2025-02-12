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
    public class DashboardController : Controller
    {
        // GET: Dashboard
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            DashboardAsesoresEntity dashboardAsesoresEntity = new DashboardAsesoresEntity();
            dashboardAsesoresEntity = DALPortalLaLigaDashboardAsesor.DashboardAsesores(sessions.SlpCode, Convert.ToDateTime("2024-12-01"), Convert.ToDateTime("2024-12-31"));
            return View(dashboardAsesoresEntity);
        }

        public async Task<JsonResult> DataMensualAsync()
        {
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            if (sessions == null)
            {
                return null;
            }

            if (sessions.SlpCode != -1 || sessions.SlpCode != 0)
            {
                var datosMensuales = await DALPortalLaLigaDashboardAsesor.DashboardVentasPorMesAsync();
                var datos = new
                {
                    labels = datosMensuales.Select(d => d.labels).ToArray(),
                    data = datosMensuales.Select(d => d.data).ToArray()
                };
                return Json(datos, JsonRequestBehavior.AllowGet);
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DataSemanal()
        {
            var datosMensuales = DALPortalLaLigaDashboardAsesor.DashboardVentasPorSemana();
            var datos = new
            {
                labels = datosMensuales.Select(d => d.labels).ToArray(), // Nombres de los meses o etiquetas
                data = datosMensuales.Select(d => d.data).ToArray() // Valores numéricos
            };
            return Json(datos, JsonRequestBehavior.AllowGet);
        }

        [HttpGet] // Opcional, pero recomendado para claridad
        public JsonResult GetDetalleProductos()
        {
            if (Session["PropertiesEntity"] == null)
            {
                return Json(new { success = false, message = "Sesión expirada" }, JsonRequestBehavior.AllowGet);
            }

            try
            {   
                var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
                var listado = DALPortalMargenUtilidad.ListadoProductosConMargen(sessions.SlpCode, "");

                return Json(new { success = true, data = listado }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}