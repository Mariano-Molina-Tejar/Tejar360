using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using System.Threading.Tasks;
using Entity;
namespace PORTALI.Controllers
{
    public class GantProyectosITController : Controller
    {
        // GET: GantProyectosIT
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult ObtenerKPIs(DateTime? fechaInicio, DateTime? fechaFin)
        {
            List<TicketEntity> tickets = DALPortalGenerales.ObtenerTickets(fechaInicio, fechaFin);
            List<TicketAnualEntity> mensual = DALPortalGenerales.TicketsMensual();

            var ticketsPorAsignado = tickets
                .GroupBy(t => t.Asignado)
                .Select(g => new { Key = g.Key, Cantidad = g.Count(), PromedioCierre = g.Average(x => Convert.ToDouble(x.TiempoCierreMinutos)), PromedioRespuesta = g.Average(x => Convert.ToDouble(x.PrimeraRespuestaMinutos)) })
                .ToList();

            var ticketsPorPrioridad = tickets
                .GroupBy(t => t.Prioridad)
                .Select(g => new { Key = g.Key, Cantidad = g.Count() })
                .ToList();

            var ticketsPorCategoria = tickets
            .GroupBy(t => t.Categoria)
            .Select(g => new
            {
                Key = g.Key,
                Cantidad = g.Count(),
                PromedioSLA = g.Average(x => string.IsNullOrEmpty(x.PrimeraRespuestaMinutos)
                                             ? 0
                                             : Convert.ToDouble(x.PrimeraRespuestaMinutos)),
                PromedioCierre = g.Average(x => string.IsNullOrEmpty(x.TiempoCierreMinutos)
                                                ? 0
                                                : Convert.ToDouble(x.TiempoCierreMinutos))
            })
            .ToList();

            var ticketsPorCategoriaUsuario = tickets
                .GroupBy(t => new { t.Asignado, t.Categoria })
                .Select(g => new
                {
                    Usuario = g.Key.Asignado,
                    Categoria = g.Key.Categoria,
                    Cantidad = g.Count(),
                    PromedioSLA = g.Average(x => string.IsNullOrEmpty(x.PrimeraRespuestaMinutos) ? 0 : Convert.ToDouble(x.PrimeraRespuestaMinutos)),
                    PromedioCierre = g.Average(x => string.IsNullOrEmpty(x.TiempoCierreMinutos) ? 0 : Convert.ToDouble(x.TiempoCierreMinutos))
                })
                .OrderBy(x => x.Usuario)
                .ThenBy(x => x.Categoria)
                .ToList();

            var ticketsMensual = mensual;
            return Json(new
            {
                Asignado = ticketsPorAsignado,
                Prioridad = ticketsPorPrioridad,
                Categoria = ticketsPorCategoria,
                CategoriaUsuario = ticketsPorCategoriaUsuario,
                TicketsMensual = ticketsMensual
            }, JsonRequestBehavior.AllowGet); ;
        }
    }
}