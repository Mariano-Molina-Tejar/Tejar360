using DAL;
using Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PORTALI.Controllers
{
    public class SolvenciaController : Controller
    {
        // GET: SolvenciaIT
        SolvenciaDAL _dal = new SolvenciaDAL();
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> SolvenciaIT()
        {
            try
            {
                var empleados = await _dal.GetEmployedSolvency();
                return View(empleados);

            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.Message;
                return View();
            }
        }
        public async Task<ActionResult> SolvenciaNomina()
        {
            try
            {
                var empleados = await _dal.GetEmployedSolvencyPaysheet();
                return View(empleados);

            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.Message;
                return View();
            }
        }

        public async Task<ActionResult> SolvenciaContabilidad()
        {
            try
            {
                var empleados = await _dal.GetEmployedSolvencyFinance();
                return View(empleados);

            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.Message;
                return View();
            }
        }
        public async Task<ActionResult> SolvenciaAuditoria()
        {
            try
            {
                var empleados = await _dal.GetEmployedSolvencyAudit();
                return View(empleados);

            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.Message;
                return View();
            }
        }

        public async Task<JsonResult> ObtenerSolvenciaIT(int empId)
        {
            try
            {
                var solvencia = await _dal.GetSolvencyIT(empId);

                if (solvencia.U_FechaSolvenciaIT == null && solvencia.Code != null)
                {
                    return Json(new { status = false, success = true, data = solvencia }, JsonRequestBehavior.AllowGet);
                }
                else if (solvencia.U_FechaSolvenciaIT != null)
                {
                    return Json(new { status = true, success = true, data = solvencia }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { status = true, success = true, data = solvencia }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<JsonResult> ObtenerSolvenciaContabilidad(int empId)
        {
            try
            {
                var solvencia = await _dal.GetSolvencyContabilidad(empId);

                if (solvencia.U_FechaSolvenciaFinanzas == null && solvencia.Code != null)
                {
                    return Json(new { status = false, success = true, data = solvencia }, JsonRequestBehavior.AllowGet);
                }
                else if (solvencia.U_FechaSolvenciaFinanzas != null)
                {
                    return Json(new { status = true, success = true, data = solvencia }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { status = true, success = true, data = solvencia }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<JsonResult> ObtenerSolvenciaAuditoria(int empId)
        {
            try
            {
                var solvencia = await _dal.GetSolvencyAudit(empId);

                if (solvencia.U_FechaSolvenciaAuditoria == null && solvencia.Code != null)
                {
                    return Json(new { status = false, success = true, data = solvencia }, JsonRequestBehavior.AllowGet);
                }
                else if (solvencia.U_FechaSolvenciaAuditoria != null)
                {
                    return Json(new { status = true, success = true, data = solvencia }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { status = true, success = true, data = solvencia }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<JsonResult> ObtenerSolvenciaNomina(int empId)
        {
            try
            {
                var solvencia = await _dal.GetSolvencyPaySheet(empId);

                if (solvencia.U_FechaSolvenciaNomina == null && solvencia.Code != null)
                {
                    return Json(new { status = false, success = true, data = solvencia }, JsonRequestBehavior.AllowGet);
                }
                else if (solvencia.U_FechaSolvenciaNomina != null)
                {
                    return Json(new { status = true, success = true, data = solvencia }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { status = true, success = true, data = solvencia }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> GuardarSolvenciaContabilidad(decimal cuentaEspecial, decimal viaticos, string observaciones, int solvencia)
        {
            var url = "GestionDePersonal/SolvenciaContabilidad";

            try
            {
                var solvenciaJefe = new
                {
                    Code = solvencia,
                    U_CuentaEspecial = cuentaEspecial,
                    U_LiquidacionDeViaticos = viaticos,
                    U_ObeservacionesFinanzas = observaciones,
                    U_FechaSolvenciaFinanzas = DateTime.Now,
                    U_HoraSolvenciaFinanzas = DateTime.Now.ToString("HH:mm")
                };

                var response = DAL_API.enviarDatosSL(url, solvenciaJefe);
                var reply = JsonConvert.DeserializeObject<Reply>(response);

                if (reply.result == 1)
                    return Json(new { success = true });

                return Json(new { success = false });
            }
            catch
            {
                return Json(new { success = false });
            }
        }
        public async Task<JsonResult> GuardarSolvenciaNomina(
            decimal anticipoSalarios,
            decimal prestamoBancario,
            decimal embargos,
            decimal descuentoUniforme,
            decimal devolucionISR,
            string uniformes,
            string observaciones,
            string notificacionAreas,
            int solvencia)
        {
            var url = "GestionDePersonal/SolvenciaNomina";

            try
            {
                var solvenciaJefe = new
                {
                    Code = solvencia,
                    U_AnticiposSalarios = anticipoSalarios,
                    U_PrestamoBancario = prestamoBancario,
                    U_Embargos = embargos,
                    U_DescuentoUniforme = descuentoUniforme,
                    U_DevolucionISR = devolucionISR,
                    U_Uniformes = uniformes,
                    U_NotificacionAreasInteresadas = notificacionAreas,
                    U_ObservacionesNomina = observaciones,
                    U_FechaSolvenciaNomina = DateTime.Now,
                    U_HoraSolvenciaNomina = DateTime.Now.ToString("HH:mm")
                };

                var response = DAL_API.enviarDatosSL(url, solvenciaJefe);
                var reply = JsonConvert.DeserializeObject<Reply>(response);

                if (reply.result == 1)
                    return Json(new { success = true });

                return Json(new { success = false });
            }
            catch
            {
                return Json(new { success = false });
            }
        }
        public async Task<JsonResult> GuardarSolvenciaAuditoria(decimal compraEmpleados, decimal faltantesBodega, string observaciones, int solvencia)
        {
            var url = "GestionDePersonal/SolvenciaAuditoria";

            try
            {
                var solvenciaJefe = new
                {
                    Code = solvencia,
                    U_CompraEmpleados = compraEmpleados,
                    U_FaltantesBodega = faltantesBodega,
                    U_ObservacionesAuditoria = observaciones,
                    U_FechaSolvenciaAuditoria = DateTime.Now,
                    U_HoraSolvenciaAuditoria = DateTime.Now.ToString("HH:mm")
                };

                var response = DAL_API.enviarDatosSL(url, solvenciaJefe);
                var reply = JsonConvert.DeserializeObject<Reply>(response);

                if (reply.result == 1)
                    return Json(new { success = true });

                return Json(new { success = false });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

    }
}