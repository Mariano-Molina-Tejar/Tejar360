using DAL;
using Entity;
using Entity.Filtros;
using Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PORTALI.Controllers
{
    public class ReporteBodegasController : Controller
    {

        private readonly DALReporteCheckList _dalReporteCheckList;

        public ReporteBodegasController()
        {
            _dalReporteCheckList = new DALReporteCheckList();
        }

        private async Task CargarListasDesplegables()
        {
            var area = await _dalReporteCheckList.ObtenerArea();
            var region = await _dalReporteCheckList.ObtenerRegion();
            var tienda = await _dalReporteCheckList.ObtenerSucursal();
            ViewBag.Area = new SelectList(area, "Codigo", "Nombre");
            ViewBag.Region = new SelectList(region, "Codigo", "Nombre");
            ViewBag.Tienda = new SelectList(tienda, "Codigo", "Nombre");
        }

        // GET: ReporteBodegas
        public async Task<ActionResult> ReporteBodegas()
        {
            await CargarListasDesplegables();
            ViewBag.Resultado = new List<ReporteCheckListEntity>();
            return View(new FiltroReporteCheckList());
        }

        [HttpPost]
        public async Task<ActionResult> ReporteBodegas (FiltroReporteCheckList filtro)
        {
            await CargarListasDesplegables();

            var resultado = await _dalReporteCheckList.ObtenerCheckList(filtro);

            //Agrupamos por tienda para que no se repitan en la grafica
            var agrupado = resultado
                .GroupBy(r => r.Tienda)
                .Select(g => new ReporteCheckListEntity
                {
                    Tienda = g.Key,
                    U_PuntajeAlmacenaje = g.Average(x => x.U_PuntajeAlmacenaje),
                    U_PuntajeRecepcion = g.Average(x => x.U_PuntajeRecepcion),
                    U_PuntajeDespacho = g.Average(x => x.U_PuntajeDespacho),
                    U_PuntajeDaniado = g.Average(x => x.U_PuntajeDaniado),
                    U_Puntaje5S = g.Average(x => x.U_Puntaje5S),
                    CalificacionTotal = g.Average(x => x.CalificacionTotal)
                })
                .ToList();

            //mandamos la lista a la vista
            ViewBag.Resultado = resultado;

            return View(filtro);
        }

        
       
    }
}
