using ClosedXML.Excel;
using DAL;
using Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PORTALI.Controllers
{
    public class ReporteDaniadoTablaController : Controller
    {
        private readonly DALReporteDaniadoTabla _dal = new DALReporteDaniadoTabla();

        // GET: ReporteDaniadoTabla
        //public ActionResult TablaDaniado()
        //{
        //    return View();
        //}

        public async Task<ActionResult> TablaDaniado(DateTime? fechaInicio, DateTime? fechaFin)
        {
            var fechaI = fechaInicio ?? DateTime.Today.AddDays(-30);
            var fechaF = fechaFin ?? DateTime.Today;

            var tiendas = await _dal.ObtenerDatosTiendas(fechaI, fechaF);
            var CD_Tecnopark = await _dal.ObtenerDatosCD(fechaI, fechaF);

            var filas = tiendas.Select(x => new ReporteDaniadoFilaEntity
            {
                Region = x.Region,
                Localizacion = x.Localizacion,
                CodTienda = x.CodTienda,
                NombreTienda = x.Tienda,
                Venta = x.Venta,
                TotalDaniado = x.TotalDaniado,
                PorcentajeDaniado = x.PorcentajeDaniadoAlmacen,

                DanadoSalidaCD = 0,
                DanadoNotaCredito = 0,
                DanadoRecuperado = 0,
                PorcentajeNC = 0,
                PorcentajeAlmacen = 0,

                EsBodegaCentral = false
            }).ToList();

            filas.AddRange(CD_Tecnopark.Select(c => new ReporteDaniadoFilaEntity
            {
                Region = c.Region,
                Localizacion = c.Localizacion,
                CodTienda = c.CodTienda,
                NombreTienda = "Bodega Central",
                Venta = c.Venta,
                TotalDaniado = c.MontoTotalDaniado,
                PorcentajeDaniado = c.PorcentajeTotalDaniado,

                DanadoSalidaCD = c.Danado_Salida_CD,
                DanadoNotaCredito = c.Danado_NotaCredito,
                DanadoRecuperado =  c.Danado_Recuperado,
                PorcentajeNC = c.PorcentajeNC,
                PorcentajeAlmacen = c.PorcentajeAlmacen,

                EsBodegaCentral = true
            }));

            var regiones = filas
                .GroupBy(x => new { x.Region, x.Localizacion })
                .Select(g => new ReporteDaniadoRegionEntity
                {
                    Region = g.Key.Region,
                    NombreRegion = g.Key.Localizacion,
                    Filas = g.ToList(),
                    Totales = new ReporteDaniadoTotalesEntity
                    {
                        Venta = g.Sum(x => x.Venta),
                        TotalDaniado = g.Sum(x => x.TotalDaniado)
                    }
                })
                .OrderBy(r => r.Region)
                .ToList();

            var model = new ReporteDaniadoTablaViewModel
            {
                FechaInicio = fechaI,
                FechaFin = fechaF,
                Regiones = regiones,
                TotalesGlobales = new ReporteDaniadoTotalesEntity 
                {
                    Venta = filas.Sum(x=> x.Venta),
                    TotalDaniado = filas.Sum(x => x.TotalDaniado)
                }
            };

            return View(model);
        }


        // ---------------- EXCEL ----------------
        public async Task<ActionResult> ExportarExcel(DateTime fechaInicio, DateTime fechaFin)
        {
            var model = (ReporteDaniadoTablaViewModel)
                (await TablaDaniado(fechaInicio, fechaFin) as ViewResult).Model;

            using (var wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("Reporte Dañado");
                int row = 1;

                ws.Cell(row, 1).Value = "Region";
                ws.Cell(row, 2).Value = "Tienda";
                ws.Cell(row, 3).Value = "Venta";
                ws.Cell(row, 4).Value = "Total Dañado";
                ws.Cell(row, 5).Value = "% Dañado";
                ws.Cell(row, 6).Value = "Salida CD";
                ws.Cell(row, 7).Value = "Nota Crédito";
                ws.Cell(row, 8).Value = "Recuperado";
                ws.Cell(row, 9).Value = "% NC";
                ws.Cell(row, 10).Value = "% Almacen";

                row++;

                foreach (var region  in model.Regiones)
                {
                    foreach (var f in region.Filas)
                    {
                        ws.Cell(row, 1).Value = region.NombreRegion;
                        ws.Cell(row, 2).Value = f.NombreTienda;
                        ws.Cell(row, 3).Value = f.Venta;
                        ws.Cell(row, 4).Value = f.TotalDaniado;
                        ws.Cell(row, 5).Value = f.PorcentajeDaniado;
                        ws.Cell(row, 6).Value = f.DanadoSalidaCD;
                        ws.Cell(row, 7).Value = f.DanadoNotaCredito;
                        ws.Cell(row, 8).Value = f.DanadoRecuperado;
                        ws.Cell(row, 9).Value = f.PorcentajeNC;
                        ws.Cell(row, 10).Value = f.PorcentajeAlmacen;
                        row++;

                    }
                }

                ws.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "ReporteDaniado.xlsx");
                }

            }
        }
    }
}