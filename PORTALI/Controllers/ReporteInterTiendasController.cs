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
    public class ReporteInterTiendasController : Controller
    {
        private readonly DALReporteInterTiendas _dalReporteInterTiendas;

        public ReporteInterTiendasController()
        {
            _dalReporteInterTiendas = new DALReporteInterTiendas();
        }

        private async Task CargarListasDesplegables()
        {
            var camiones = await _dalReporteInterTiendas.ObtenerTransportes();
            ViewBag.Camiones = new SelectList(camiones, "IdCamion", "Camion");
            //ViewBag.Camiones = new SelectList(await _dalReporteInterTiendas.ObtenerTransportes(), "IdCamion", "Camion");
        }

        //GET: ReporteInterTiendas
        public async Task<ActionResult> Index()
        {
            await CargarListasDesplegables();
            ViewBag.Resultado = new List<ReporteInterTiendasEntity>();
            return View(new FiltroReporteInterTiendas());
        }

        //POST: ReporteInterTiendas (carga tabla principal)
        [HttpPost]
        public async Task<ActionResult> Index(FiltroReporteInterTiendas filtro)
        {
            await CargarListasDesplegables();
            var resultado = await _dalReporteInterTiendas.ObtenerInterTiendas(filtro);
            ViewBag.Resultado = resultado;
            return View(filtro);
        }

        //GET (AJAX): Cargar tabla secundaria según ruta
        [HttpPost]
        public async Task<ActionResult> ObtenerDetallePorRuta(int ruta)//, DateTime? fechaInicio, DateTime? fechaFin)
        {
            var filtro = new FiltroReporteInterTiendas
            {

                //FechaInicio = fechaInicio,
                //FechaFin = fechaFin
                FechaInicio = null,
                FechaFin = null,
                Ruta = ruta
            };

            var detalle = await _dalReporteInterTiendas.ObtenerEstatusRuta(filtro);

            //Obtenemos la tabla principal para esa ruta
            var filtroPrincipal = new FiltroReporteInterTiendas
            {
                Ruta = ruta
            };

            var principal = await _dalReporteInterTiendas.ObtenerInterTiendas(filtroPrincipal);

            //Calculamos el promedio de dias transcurricos
            double promedio = 0;
            if (principal != null && principal.Any())
            {
                promedio = Math.Abs(Math.Round(principal.Average(x => x.DiasTranscurridos), 2));
            }

            var viewModel = new DetalleRutaViewModel
            {
                Detalles = detalle,
                PromedioDiasTranscurridos = promedio,
                Ruta = filtro.Ruta,
                FechaInicio = filtro.FechaInicio,
                FechaFin = filtro.FechaFin
            };

            //ViewBag.Ruta = filtro.Ruta;
            //ViewBag.FechaInicio = filtro.FechaInicio;
            //ViewBag.FechaFin = filtro.FechaFin;

            return PartialView("_DetalleEstatusRuta", viewModel); //Vista parcial que se mostrara dinamicamente
        }

        //POST: Permite exportar a excel
        [HttpPost]
        public async Task<ActionResult> ExportarExcel(FiltroReporteInterTiendas filtro)
        {
            var datos = await _dalReporteInterTiendas.ObtenerInterTiendas(filtro);

            using (var wb = new ClosedXML.Excel.XLWorkbook())
            {
                var ws = wb.Worksheets.Add("InterTiendas");

                // Encabezados
                ws.Cell(1, 1).Value = "No Orden Venta";
                ws.Cell(1, 2).Value = "Fecha OV";
                ws.Cell(1, 3).Value = "Fecha Entrega";
                ws.Cell(1, 4).Value = "No Orden Compra";
                ws.Cell(1, 5).Value = "Fecha OC";
                ws.Cell(1, 6).Value = "No Entrada Mercancía";
                ws.Cell(1, 7).Value = "Fecha Recibido Bodega";
                ws.Cell(1, 8).Value = "Fecha Carga";
                ws.Cell(1, 9).Value = "Días Transcurridos";
                ws.Cell(1, 10).Value = "Estado Entrega";
                ws.Cell(1, 11).Value = "Ruta";
                ws.Cell(1, 12).Value = "Bodega Origen";
                ws.Cell(1, 13).Value = "Ruta Compartida";
                ws.Cell(1, 14).Value = "Destino";
                ws.Cell(1, 15).Value = "Fecha Descarga";
                ws.Cell(1, 16).Value = "Camion";

                int row = 2;
                foreach (var item in datos)
                {
                    ws.Cell(row, 1).Value = item.NoOrdenVenta;
                    ws.Cell(row, 2).Value = item.FechaOV.ToShortDateString();
                    ws.Cell(row, 3).Value = item.FechaEntrega.ToShortDateString();
                    ws.Cell(row, 4).Value = item.NoOrdenCompra;
                    ws.Cell(row, 5).Value = item.FechaOC.ToShortDateString();
                    ws.Cell(row, 6).Value = item.NoEntradaMercancia;
                    ws.Cell(row, 7).Value = item.FechaRecibidoBodega.ToShortDateString();
                    ws.Cell(row, 8).Value = item.FechaCarga.ToShortDateString();
                    ws.Cell(row, 9).Value = item.DiasTranscurridos;
                    ws.Cell(row, 10).Value = item.EstadoEntrega;
                    ws.Cell(row, 11).Value = item.Ruta;
                    ws.Cell(row, 12).Value = item.BodegaOrigen;
                    ws.Cell(row, 13).Value = item.RutaCompartida;
                    ws.Cell(row, 14).Value = item.Destino;
                    ws.Cell(row, 15).Value = item.FechaDescarga.ToShortDateString();
                    ws.Cell(row, 16).Value = item.Camion;
                    row++;
                }

                using (var stream = new System.IO.MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;

                    string fileName = $"ReporteInterTiendas_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                    return File(stream.ToArray(),
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                fileName);
                }
            }
        }


        [HttpPost]
        public async Task<ActionResult> ExportarExcelDetalle(FiltroReporteInterTiendas filtro)
        {
            var datos = await _dalReporteInterTiendas.ObtenerEstatusRuta(filtro);
            

            using (var wb = new ClosedXML.Excel.XLWorkbook())
            {
                var ws = wb.Worksheets.Add("EstatusRuta");

                // Encabezados
                ws.Cell(1, 1).Value = "No Ruta";
                ws.Cell(1, 2).Value = "Estado Ruta";
                ws.Cell(1, 3).Value = "Fecha Inicio";
                ws.Cell(1, 4).Value = "Hora Inicio";
                ws.Cell(1, 5).Value = "Fecha Fin";
                ws.Cell(1,6).Value = "Hora Fin";

                int row = 2;
                foreach (var item in datos)
                {
                    ws.Cell(row, 1).Value = item.NoRuta;
                    ws.Cell(row, 2).Value = item.EstadoRuta;
                    ws.Cell(row, 3).Value = item.FechaInicio.ToShortDateString();
                    ws.Cell(row, 4).Value = item.HoraInicio;
                    ws.Cell(row, 5).Value = item.FechaFin.ToShortDateString();
                    ws.Cell(row, 6).Value = item.HoraFin;
                    row++;
                }

                using (var stream = new System.IO.MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;

                    string fileName = $"ReporteInterTiendasEstatusRuta_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                    return File(stream.ToArray(),
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                fileName);
                }
            }
        }


    }
}
