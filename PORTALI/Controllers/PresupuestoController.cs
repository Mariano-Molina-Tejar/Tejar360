using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Entity;
using DAL;
using Newtonsoft.Json;

namespace PORTALI.Controllers
{
    public class PresupuestoController : Controller
    {
        // GET: Presupuesto
        DALPortalPresupuesto presupuestoServices = new DALPortalPresupuesto();
        public async Task<ActionResult> Index(int? mes, int? acctCode)
        {
            List<DetallePresupuestoN1> detallePresupuesto = new List<DetallePresupuestoN1>();

            try
            {
                var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
                if (sessions == null)
                {
                    return Json(new { success = false, message = "Sesión expirada" });
                }
                mes = !mes.HasValue ? DateTime.Now.Month : mes;

                int mesSeleccionado = mes ?? DateTime.Now.Month;
                List<AreasPresupuesto> areas = await presupuestoServices.ObtenerAreasPresupuesto(sessions.CodeEmpleado);
                if (areas == null || areas.Count == 0)
                {
                    return RedirectToAction("Cotizacion", "CotizacionVentas"); // puedes mandar a otra vista, o devolver la misma pero vacía
                }

                int? AreaSeleccionada = acctCode ?? areas[0].AcctCode;
                ViewBag.DetalleFactura = new List<DetalleDocumento>();
                ViewBag.MesSeleccionado = mesSeleccionado;
                ViewBag.AreaSeleccionada = AreaSeleccionada;
                ViewBag.Areas = areas ?? new List<AreasPresupuesto>();
                ViewBag.SaldoFururo = await presupuestoServices.ObtenerSaldoFuturo(AreaSeleccionada,mes);
                detallePresupuesto = await presupuestoServices.ObtenrDetallePresupuestoN1(mes, AreaSeleccionada);
            }
            catch (Exception ex)
            {
                detallePresupuesto.Add(new DetallePresupuestoN1 { ErrorMessage = ex.Message });
            }

            return View(detallePresupuesto);
        }
        public async Task<JsonResult> ObtenerDetalleCuentasN2(int cuenta, int? mes)
        {
            List<DetallePresupuestoN1> detallePresupuesto = new List<DetallePresupuestoN1>();
            mes = !mes.HasValue ? DateTime.Now.Month : mes;

            try
            {
                var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
                if (sessions == null)
                {
                    return Json(new { success = false, message = "Sesión expirada" });
                }

                detallePresupuesto = await presupuestoServices.ObtenrDetallePresupuestoN2(cuenta, mes);
                return Json(new { success = true, data = detallePresupuesto });
            }
            catch (Exception ex)
            {
                detallePresupuesto.Add(new DetallePresupuestoN1 { ErrorMessage = ex.Message });
                return Json(new { success = false, message = ex.Message });
            }
        }
        public async Task<JsonResult> ObtenerDetalleCuentasN3(string cuenta, int? mes)
        {
            List<DetallePresupuestoN3> detallePresupuesto = new List<DetallePresupuestoN3>();
            mes = !mes.HasValue ? DateTime.Now.Month : mes;

            try
            {
                var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
                if (sessions == null)
                {
                    return Json(new { success = false, message = "Sesión expirada" });
                }


                detallePresupuesto = await presupuestoServices.ObtenrDetallePresupuestoN3(cuenta, mes);
                return Json(new { success = true, data = detallePresupuesto });

            }
            catch (Exception ex)
            {
                detallePresupuesto.Add(new DetallePresupuestoN3 { ErrorMessage = ex.Message });
                return Json(new { success = false, message = ex.Message });

            }
        }
        public JsonResult GuardarNotas(NotasPto nota)
        {
            try
            {
                string url = "/Presupuesto/SetNote/";
                var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
                if (sessions == null)
                {
                    return Json(new { success = false, message = "Sesión expirada" });
                }

                string contenido = DAL_API.NotasPpto(url, nota);
                Reply datos = JsonConvert.DeserializeObject<Reply>(contenido);

                return Json(new { success = true, data = datos.message });
            }
            catch (Exception ex)
            {
                //detallePresupuesto.Add(new DetallePresupuestoN3 { ErrorMessage = ex.Message });
                return Json(new { success = false, message = ex.Message });
            }
        }

        public JsonResult BorrarNotas(int Code)
        {
            try
            {
                string url = "/Presupuesto/DeleteNote/";
                var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
                if (sessions == null)
                {
                    return Json(new { success = false, message = "Sesión expirada" });
                }
                object Codigo = new { Code = Code };
                string contenido = DAL_API.NotasPpto(url, Codigo);
                Reply datos = JsonConvert.DeserializeObject<Reply>(contenido);

                return Json(new { success = true, data = datos.message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public async Task<JsonResult> VerDetalleFactura(int DocNum, string CodigoCuenta, int IdDocumento)
        {
            try
            {
                string url = "/Presupuesto/DeleteNote/";
                var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
                if (sessions == null)
                {
                    return Json(new { success = false, message = "Sesión expirada" });
                }

                List<DetalleDocumento> DetalleFactura = await presupuestoServices.ObtenrDetalleFactura(DocNum, CodigoCuenta, IdDocumento);

                ViewBag.DetalleFactura = DetalleFactura;
                return Json(new { success = true, data = DetalleFactura });
            }
            catch (Exception ex)
            {
                //detallePresupuesto.Add(new DetallePresupuestoN3 { ErrorMessage = ex.Message });
                return Json(new { success = false, message = ex.Message });

            }
        }
    }
}