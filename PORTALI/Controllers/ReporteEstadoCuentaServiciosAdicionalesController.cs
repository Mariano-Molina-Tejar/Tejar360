using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DAL;
using Entity;
using Entity.Filtros;

namespace PORTALI.Controllers
{
    public class ReporteEstadoCuentaServiciosAdicionalesController : BaseReporteController
    {
        private readonly DALReporteEstadoCuentaServiciosAdicionales _dalServiciosAdicionales;

        public ReporteEstadoCuentaServiciosAdicionalesController()
        {
            _dalServiciosAdicionales = new DALReporteEstadoCuentaServiciosAdicionales();
        }

        private async Task CargarProveedores()
        {
            //Llamamos al DAL que obtiene Proveedores (sp separado del principal)
            var dalProv = new DALReporteEstadoCuentaServiciosAdicionales();
            var proveedores = await dalProv.ObtenerProveedores(); //Metodo que devuelve lista de proveedores
            ViewBag.Proveedores = new SelectList(proveedores, "Codigo", "Nombre");
        }

        // GET: ReporteEstadoCuentaServiciosAdicionales
        public async Task<ActionResult> Index(string fechaInicio, string fechaFinal)
        {
            var filtroBase = ObtenerFiltroConFechas(fechaInicio, fechaFinal);
            var filtro = new FiltroServiciosAdicionalesEntity
            {
                FechaInicio = filtroBase.FechaInicio,
                FechaFin = filtroBase.FechaFin
            };

            ViewBag.Resultado = new List<ReporteEstadoCuentaServiciosAdicionales>();
            await CargarProveedores();
            return View(filtro);
        }

        [HttpPost]
        public async Task<ActionResult>  Index(FiltroServiciosAdicionalesEntity filtro)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Resultado = new List<ReporteEstadoCuentaServiciosAdicionales>();
                await CargarProveedores();
                return View(filtro);
            }

            ViewBag.FechaInicio = filtro.FechaInicio.ToString("yyyy-MM-dd");
            ViewBag.FechaFina = filtro.FechaFin.ToString("yyyy-MM-dd");

            var resultado = await _dalServiciosAdicionales.ObtenerServiciosAdicionales(filtro);
            ViewBag.Resultado = resultado;

            await CargarProveedores();
            return View(filtro);
        }

        [HttpPost]
        public async Task<ActionResult> ExportarExcel(FiltroServiciosAdicionalesEntity filtro)
        {
            var datos = await _dalServiciosAdicionales.ObtenerServiciosAdicionales(filtro);

            using (var wb = new ClosedXML.Excel.XLWorkbook())
            {
                var ws = wb.Worksheets.Add("ServiciosAdicionales");
                ws.Cell(1, 1).Value = "Nombre";
                ws.Cell(1, 2).Value = "Código";
                ws.Cell(1, 3).Value = "Fecha OC";
                ws.Cell(1, 4).Value = "Orden Compra";
                ws.Cell(1, 5).Value = "Monto OC";
                ws.Cell(1, 6).Value = "Fecha FacProv";
                ws.Cell(1, 7).Value = "Factura Proveedor";
                ws.Cell(1, 8).Value = "Monto FP";
                ws.Cell(1, 9).Value = "Serie";
                ws.Cell(1, 10).Value = "Factura";
                ws.Cell(1, 11).Value = "Fecha Pago";
                ws.Cell(1, 12).Value = "Pago";
                ws.Cell(1, 13).Value = "Monto Pagado";

                int row = 2;
                foreach (var item in datos)
                {
                    ws.Cell(row, 1).Value = item.Nombre;
                    ws.Cell(row, 2).Value = item.Codigo;
                    ws.Cell(row, 3).Value = item.FechaOC.ToShortDateString();
                    ws.Cell(row, 4).Value = item.OrdenCompra;
                    ws.Cell(row, 5).Value = item.MontoOC;
                    ws.Cell(row, 6).Value = item.FechaFacProv.ToShortDateString();
                    ws.Cell(row, 7).Value = item.FacProv;
                    ws.Cell(row, 8).Value = item.MontoFP;
                    ws.Cell(row, 9).Value = item.Serie;
                    ws.Cell(row, 10).Value = item.Factura;
                    ws.Cell(row, 11).Value = item.FechaPago.ToShortDateString();
                    ws.Cell(row, 12).Value = item.Pago;
                    ws.Cell(row, 13).Value = item.MontoPagado;
                    row++;
                }

                using (var stream = new System.IO.MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    string fileName = $"ServiciosAdicionales_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }


    }
}