using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using System.Threading.Tasks;
using Entity;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;

namespace PORTALI.Controllers
{
    public class KPISItController : Controller
    {
        // GET: KPISIt
        public async Task<ActionResult> Index()
        {
            string json = System.IO.File.ReadAllText(@"C:\Users\MarianoJoséMolinaDáv\Documents\JsonTickets\tickets2.json");
            TicketExport data = JsonConvert.DeserializeObject<TicketExport>(json);

            //var ticketsPorAsignado = data.Tickets
            //    .GroupBy(t => t.assigned_to)
            //    .Select(g => new {  key = g.Key,
            //                        cantidad = g.Count(),
            //                        PromedioCierre = g.Average(x => x.close_time_secs),
            //                        PromedioRespuesta = g.Average(x => x.first_response_secs)
            //    })
            //    .Join(data.Users, a => a.key, b => b.Import_Id)
            //    .ToList();

            //        var ticketsPorAsignado = data.Tickets
            //.GroupBy(t => t.assigned_to)
            //.Select(g => new
            //{
            //    key = g.Key,
            //    cantidad = g.Count(),
            //    PromedioCierre = g.Average(x => x.close_time_secs),
            //    PromedioRespuesta = g.Average(x => x.first_response_secs)
            //})
            //.Join(
            //    data.Users,
            //    a => a.key,
            //    b => b.Import_Id,
            //    (a, b) => new
            //    {
            //        b.First_Name,
            //        //b.email,
            //        a.cantidad,
            //        a.PromedioCierre,
            //        a.PromedioRespuesta
            //    }
            //)
            //.ToList();

            //var ticketPromedioSLA2 = data.Tickets
            //    .Average(x => x.first_response_secs);

            //var dias = (ticket.Created_At - DateTime.Parse(ticket.Updated_At));

            return View();
        }

        public JsonResult ObtenerKPIs(DateTime? fechaInicio, DateTime? fechaFin)
        {
            List<TicketEntity> tickets = DALPortalGenerales.ObtenerTickets(fechaInicio, fechaFin);

            List<TicketAnualEntity> ticketsMensual = DALPortalGenerales.TicketsMensual();

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

            return Json(new
            {
                Asignado = ticketsPorAsignado,
                Prioridad = ticketsPorPrioridad,
                Categoria = ticketsPorCategoria,
                CategoriaUsuario = ticketsPorCategoriaUsuario
            }, JsonRequestBehavior.AllowGet);
        }
    }
}