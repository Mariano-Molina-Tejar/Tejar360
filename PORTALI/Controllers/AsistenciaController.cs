using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entity;
using DAL;
using System.Threading.Tasks;
using System.Data.SqlTypes;

namespace PORTALI.Controllers
{
    public class AsistenciaController : Controller
    {
        private DALPortalRRHH _dal = new DALPortalRRHH();

        public async Task<ActionResult> Index(DateTime? fechaI, DateTime? fechaF, string nombre)
        {
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            if (sessions == null)
            {
                ViewData["Error"] = "Su sesion ha expirado!!";
                return RedirectToAction("Login", "Account");
            }

            List<AsistenciaModel> asistencia = new List<AsistenciaModel>();
            List<AsistenciaSemanalViewModel> ListaAsistenciaError = new List<AsistenciaSemanalViewModel>();

            try
            {
                fechaI = (fechaI < (DateTime)SqlDateTime.MinValue) ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1) : fechaI;
                fechaF = (fechaF < (DateTime)SqlDateTime.MinValue) ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1) : fechaF;

                fechaI = fechaI == null ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1) : fechaI;
                fechaF = fechaF == null ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1) : fechaF;
                nombre = string.IsNullOrEmpty(nombre) ? "" : nombre;

                asistencia = await _dal.ObternerAsistencia(fechaI, fechaF, nombre, sessions.UserId);

                var viewModel = asistencia
                .GroupBy(a => new { a.Empleado, a.Semana })
                .Select(g => new AsistenciaSemanalViewModel
                {
                    Empleado = $"{g.Key.Empleado}",

                    LunesSucursal = $"{g.FirstOrDefault(x => x.Dia.ToLower() == "lunes")?.Sucursal} \n {g.FirstOrDefault(x => x.Dia.ToLower() == "lunes")?.Fecha}",
                    LunesEntrada = g.FirstOrDefault(x => x.Dia.ToLower() == "lunes")?.HoraEntrada,
                    LunesSalida = g.FirstOrDefault(x => x.Dia.ToLower() == "lunes")?.HoraSalida,

                    MartesSucursal = $"{g.FirstOrDefault(x => x.Dia.ToLower() == "martes")?.Sucursal} \n{g.FirstOrDefault(x => x.Dia.ToLower() == "martes")?.Fecha}",
                    MartesEntrada = g.FirstOrDefault(x => x.Dia.ToLower() == "martes")?.HoraEntrada,
                    MartesSalida = g.FirstOrDefault(x => x.Dia.ToLower() == "martes")?.HoraSalida,

                    MiercolesSucursal = $"{g.FirstOrDefault(x => x.Dia.ToLower() == "miércoles" || x.Dia.ToLower() == "miercoles")?.Sucursal} \n {g.FirstOrDefault(x => x.Dia.ToLower() == "miércoles")?.Fecha}",
                    MiercolesEntrada = g.FirstOrDefault(x => x.Dia.ToLower() == "miércoles" || x.Dia.ToLower() == "miercoles")?.HoraEntrada,
                    MiercolesSalida = g.FirstOrDefault(x => x.Dia.ToLower() == "miércoles" || x.Dia.ToLower() == "miercoles")?.HoraSalida,

                    JuevesSucursal = $"{g.FirstOrDefault(x => x.Dia.ToLower() == "jueves")?.Sucursal}  {g.FirstOrDefault(x => x.Dia.ToLower() == "jueves")?.Fecha}",
                    JuevesEntrada = g.FirstOrDefault(x => x.Dia.ToLower() == "jueves")?.HoraEntrada,
                    JuevesSalida = g.FirstOrDefault(x => x.Dia.ToLower() == "jueves")?.HoraSalida,

                    ViernesSucursal = $"{g.FirstOrDefault(x => x.Dia.ToLower() == "viernes")?.Sucursal} \n {g.FirstOrDefault(x => x.Dia.ToLower() == "viernes")?.Fecha}",
                    ViernesEntrada = g.FirstOrDefault(x => x.Dia.ToLower() == "viernes")?.HoraEntrada,
                    ViernesSalida = g.FirstOrDefault(x => x.Dia.ToLower() == "viernes")?.HoraSalida,

                    SabadoSucursal = $"{g.FirstOrDefault(x => x.Dia.ToLower() == "sábado" || x.Dia.ToLower() == "sabado")?.Sucursal} \n {g.FirstOrDefault(x => x.Dia.ToLower() == "sábado")?.Fecha}",
                    SabadoEntrada = g.FirstOrDefault(x => x.Dia.ToLower() == "sábado" || x.Dia.ToLower() == "sabado")?.HoraEntrada,
                    SabadoSalida = g.FirstOrDefault(x => x.Dia.ToLower() == "sábado" || x.Dia.ToLower() == "sabado")?.HoraSalida
                })
                .ToList();
                return View(viewModel);
            }
            catch (Exception ex)
            {
                ListaAsistenciaError.Add(new AsistenciaSemanalViewModel { ErrorMessage = ex.Message });
                return View(ListaAsistenciaError);
            }
            finally
            {
                ViewBag.FechaI = fechaI?.ToString("yyyy-MM-dd");
                ViewBag.FechaF = fechaF?.ToString("yyyy-MM-dd");
                ViewBag.Nombre = nombre;
            }
        }
    }
}