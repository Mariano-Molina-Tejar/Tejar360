using DAL;
using Entity;
using Entity.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PORTALI.Controllers
{
    public class ReporteEstadoCuentaLocalController : BaseReporteController
    {
        private readonly DALReporteEstadoCuentaLocal _dalEstadoCuentaLocal;

        public ReporteEstadoCuentaLocalController()
        {
            _dalEstadoCuentaLocal = new DALReporteEstadoCuentaLocal();
        }

        private async Task CargarListasDesplegables()
        {
            var proveedores = await _dalEstadoCuentaLocal.ObtenerProveedores();
            var destino = await _dalEstadoCuentaLocal.ObtenerTiendas();
            ViewBag.Proveedores = new SelectList(await _dalEstadoCuentaLocal.ObtenerProveedores(), "Codigo", "Nombre");
            ViewBag.Tiendas = new SelectList(await _dalEstadoCuentaLocal.ObtenerTiendas(), "CodDestino", "Destino");
        }

        // GET: ReporteEstadoCuentaLocal
        public async Task<ActionResult> Index(string fechaInicio, string fechaFinal)
        {
            //var filtroBase = ObtenerFiltroConFechas(fechaInicio, fechaFinal);
            //var filtro = new FiltroEstadoCuentaLocal
            //{
            //    FechaInicio = filtroBase.FechaInicio,
            //    FechaFin = filtroBase.FechaFin
            //};

            var filtro = new FiltroEstadoCuentaLocal();

            DateTime tempInicio;
            if (DateTime.TryParse(fechaInicio, out tempInicio))
            {
                filtro.FechaInicio = tempInicio;
                ViewBag.FechaInicio = tempInicio.ToString("yyyy-MM-dd");
            }
            else
            {
                ViewBag.FechaInicio = ""; // Campo en blanco si no se ingresó fecha
            }

            DateTime tempFin;
            if (DateTime.TryParse(fechaFinal, out tempFin))
            {
                filtro.FechaFin = tempFin;
                ViewBag.FechaFin = tempFin.ToString("yyyy-MM-dd");
            }
            else
            {
                ViewBag.FechaFin = ""; // Campo en blanco si no se ingresó fecha
            }

            await CargarListasDesplegables();
            ViewBag.Resultado = new List<ReporteEstadoCuentaLocalEntity>();
            

            return View(filtro);
        }

        [HttpPost]
        public async Task<ActionResult> Index(FiltroEstadoCuentaLocal filtro)
        {
            
            if (!ModelState.IsValid)
            {
                ViewBag.Resultado = new List<ReporteEstadoCuentaLocalEntity>();
                //await CargarProveedores();
                return View(filtro);
            }

            ViewBag.FechaInicio = filtro.FechaInicio?.ToString("yyyy-MM-dd");
            ViewBag.FechaFina = filtro.FechaFin?.ToString("yyyy-MM-dd");

            var resultado = await _dalEstadoCuentaLocal.ObtenerEstadosCuentaLocal(filtro);
            ViewBag.Resultado = resultado;

            await CargarListasDesplegables();
            return View(filtro);

        }
        [HttpPost]
        public async Task<ActionResult>ExportarExcel(FiltroEstadoCuentaLocal filtro)
        {
            var datos = await _dalEstadoCuentaLocal.ObtenerEstadosCuentaLocal(filtro);
            using (var wb = new ClosedXML.Excel.XLWorkbook())
            {
                var ws = wb.Worksheets.Add("EstadoCuentaLocal");
                ws.Cell(1, 1).Value = "Ruta";
                ws.Cell(1, 2).Value = "Origen";
                ws.Cell(1, 3).Value = "Destino";
                ws.Cell(1, 4).Value = "Codigo";
                ws.Cell(1, 5).Value = "Nombre";
                ws.Cell(1, 6).Value = "FechaOC";
                ws.Cell(1, 7).Value = "NumOC";
                ws.Cell(1, 8).Value = "MontoOC";
                ws.Cell(1, 9).Value = "FechaFacProv";
                ws.Cell(1, 10).Value = "MontoFP";
                ws.Cell(1, 11).Value = "NumFP";
                ws.Cell(1, 12).Value = "DetalleFP";
                ws.Cell(1, 13).Value = "FechaPago";
                ws.Cell(1, 14).Value = "NumPago";
                ws.Cell(1, 15).Value = "MontoPago";
                ws.Cell(1, 16).Value = "DetallePago";

                int row = 2;
                foreach (var item in datos)
                {
                    ws.Cell(row, 1).Value = item.Ruta;
                    ws.Cell(row, 2).Value = item.Origen;
                    ws.Cell(row, 3).Value = item.Destino;
                    ws.Cell(row, 4).Value = item.Codigo;
                    ws.Cell(row, 5).Value = item.Nombre;
                    ws.Cell(row, 6).Value = item.FechaOC.ToShortDateString();
                    ws.Cell(row, 7).Value = item.NumOC;
                    ws.Cell(row, 8).Value = item.MontoOC;
                    ws.Cell(row, 9).Value = item.FechaFacProv.ToShortDateString();
                    ws.Cell(row, 10).Value = item.MontoFP;
                    ws.Cell(row, 11).Value = item.NumFP;
                    ws.Cell(row, 12).Value = item.DetalleFP;
                    ws.Cell(row, 13).Value = item.FechaPago.ToShortDateString();
                    ws.Cell(row, 14).Value = item.NumPago;
                    ws.Cell(row, 15).Value = item.MontoPago;
                    ws.Cell(row, 16).Value = item.CodDestino;
                    row++;
                }

                using (var stream = new System.IO.MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    string fileName = $"EstadoCuentaLocal_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        

    }
}