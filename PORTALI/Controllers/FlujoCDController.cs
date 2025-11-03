using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entity;
using DAL;
using System.Threading.Tasks;
using System.Data.SqlTypes;
using System.IO;
using ClosedXML.Excel;

namespace PORTALI.Controllers
{
    public class FlujoCDController : Controller
    {
        DALFlujoCD _dal = new DALFlujoCD();
        public async Task<ActionResult> Index(DateTime? fechaI, DateTime? fechaF, string almacen = "-1", int unidad = 1)
        {
            ViewModelFlujoCD viewModel = new ViewModelFlujoCD();
            try
            {
                fechaI = fechaI == null ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1) : fechaI;
                fechaF = fechaF == null ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1) : fechaF;

                viewModel.Flujo = await _dal.ObtenerFLujoEntradasSalidas((DateTime)fechaI, (DateTime)fechaF, almacen, unidad);
                viewModel.Almacenes = await _dal.ObtenerAlmacenes();
            }
            catch (Exception ex)
            {
                viewModel.ErrorMessage = ex.Message;
            }
            finally
            {
                ViewBag.FechaI = fechaI?.ToString("yyyy-MM-dd");
                ViewBag.FechaF = fechaF?.ToString("yyyy-MM-dd");
                ViewBag.Almacen = almacen;
                ViewBag.Unidad = unidad;
            }
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> ExportarExcel(DateTime fechaI, DateTime fechaF, string almacen, int unidad)
        {
            var datos = await _dal.ObtenerFLujoEntradasSalidas(fechaI, fechaF, almacen, unidad);

            if (!datos.Any())
                return Json(new { result = false, message = "No se encontraron datos" }, JsonRequestBehavior.AllowGet);

            if (!string.IsNullOrEmpty(datos[0].ErrorMessage))
                return Json(new { result = false, message = datos[0].ErrorMessage }, JsonRequestBehavior.AllowGet);

            using (var wb = new ClosedXML.Excel.XLWorkbook())
            {
                var ws = wb.Worksheets.Add("Flujo_Entrada_Salida");

                // 🔹 Fila 1: títulos combinados
                ws.Range("A1:A2").Merge(); // "Almacen" ocupa 2 filas
                ws.Cell("A1").Value = "Almacén";

                ws.Range("B1:C1").Merge(); // "Entradas" abarca 2 columnas
                ws.Cell("B1").Value = "Entradas";

                ws.Range("D1:E1").Merge(); // "Salidas" abarca 2 columnas
                ws.Cell("D1").Value = "Salidas";

                // 🔹 Fila 2: subencabezados
                ws.Cell("B2").Value = "Origen";
                ws.Cell("C2").Value = "Cantidad";
                ws.Cell("D2").Value = "Destino";
                ws.Cell("E2").Value = "Cantidad";

                // 🔹 Formato del encabezado
                var headerRange = ws.Range("A1:E2");
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.FromArgb(217, 225, 242);
                headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                // 🔹 Datos
                int row = 3;
                foreach (var item in datos)
                {
                    ws.Cell(row, 1).Value = item.Almacen;
                    ws.Cell(row, 2).Value = item.Origen;
                    ws.Cell(row, 3).Value = item.CantidadO;
                    ws.Cell(row, 4).Value = item.Destino;
                    ws.Cell(row, 5).Value = item.CantidadD;
                    row++;
                }

                // 🔹 Totales
                ws.Cell(row, 1).Value = "Totales:";
                ws.Range($"A{row}:B{row}").Merge();
                ws.Cell(row, 3).FormulaA1 = $"=SUM(C3:C{row - 1})";
                ws.Cell(row, 5).FormulaA1 = $"=SUM(E3:E{row - 1})";
                ws.Row(row).Style.Font.Bold = true;
                ws.Row(row).Style.Fill.BackgroundColor = XLColor.FromArgb(235, 241, 222);

                // 🔹 Ajustar tamaño
                ws.Columns().AdjustToContents();

                // 🔹 Descargar archivo
                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    string codigoUnico = Guid.NewGuid().ToString("N").Substring(0,7);
                    string fileName = $"FlujoEntradasSalidas_{codigoUnico}.xlsx";
                    return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileName);
                }
            }

        }

        //public async Task<ActionResult> DescargarExcel(List<FlujoCD_Entity> datos)
        //{
        //    using (var wb = new ClosedXML.Excel.XLWorkbook())
        //    {
        //        var ws = wb.Worksheets.Add("FLujo_Entrada_Salida");

        //        ws.Cell(1, 1).Value = "Almacen";
        //        ws.Cell(1, 2).Value = "Origen";
        //        ws.Cell(1, 3).Value = "Cantidad";
        //        ws.Cell(1, 4).Value = "Destino";
        //        ws.Cell(1, 5).Value = "Cantidad";

        //        int row = 2;
        //        foreach (var item in datos)
        //        {
        //            ws.Cell(row, 1).Value = item.Almacen;
        //            ws.Cell(row, 2).Value = item.Origen;
        //            ws.Cell(row, 3).Value = item.CantidadO;
        //            ws.Cell(row, 4).Value = item.Destino;
        //            ws.Cell(row, 5).Value = item.CantidadD;
        //            row++;
        //        }

        //        using (var stream = new MemoryStream())
        //        {
        //            wb.SaveAs(stream);
        //            stream.Position = 0;
        //            string fileName = $"FlujoEntradasSalidas{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
        //            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        //        }
        //    };
        //}
    }
}