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
    public class ReporteEstadoCuentaImportacionController : BaseReporteController
    {
        private readonly DALReporteEstadoCuentaProveedor _reporteDal;

        public ReporteEstadoCuentaImportacionController()
        {
            _reporteDal = new DALReporteEstadoCuentaProveedor();
        }


        // GET: ReporteEstadoCuentaImportacion
        public async Task<ActionResult> Index(string fechaInicio, string fechaFin, string proveedor)
        {
            var filtroBase = ObtenerFiltroConFechas(fechaInicio, fechaFin);

            var filtro = new FiltroEstadoCuentaEntity
            {
                FechaInicio = filtroBase.FechaInicio,
                FechaFin = filtroBase.FechaFin,
                Proveedor = proveedor
            };
            //Consultar resultados con los filtros por defecto
            var resultado = await _reporteDal.ObtenerEstadoCuentaImportacion(filtro);
            ViewBag.Resultado = resultado;

            var proveedores = await _reporteDal.ObtenerProveedoresEstadoCuenta();
            ViewBag.Proveedores = new SelectList(proveedores, "Codigo", "Nombre", filtro.Proveedor);
            

            return View(filtro);
        }

        //POST: ReporteEstadoCuentaImportacion
        [HttpPost]
        public async Task<ActionResult> Index(FiltroEstadoCuentaEntity filtro)
        {
            filtro.Validar();

            if (!ModelState.IsValid)
            {
                ViewBag.Resultado = new List<ReporteEstadoCuentaImportacionEntity>();
                return View(filtro);
            }

            ViewBag.FechaInicio = filtro.FechaInicio.ToString("yyyy-MM-dd");
            ViewBag.FechaFin = filtro.FechaFin.ToString("yyyy-MM-dd");
            ViewBag.Proveedor = filtro.Proveedor;

            var resultado = await _reporteDal.ObtenerEstadoCuentaImportacion(filtro);
            ViewBag.Resultado = resultado;

            var proveedores = await _reporteDal.ObtenerProveedoresEstadoCuenta();
            ViewBag.Proveedores = new SelectList(proveedores, "Codigo", "Nombre", filtro.Proveedor);


            return View(filtro);
        }

        [HttpPost]
        public async Task<ActionResult> ExportarExcel(FiltroEstadoCuentaEntity filtro)
        {
            var datos = await _reporteDal.ObtenerEstadoCuentaImportacion(filtro);

            using (var wb = new ClosedXML.Excel.XLWorkbook())
            {
                var ws = wb.Worksheets.Add("EstadoCuentaImportacion");
                ws.Cell(1, 1).Value = "Codigo";
                ws.Cell(1, 2).Value = "Proveedor";
                ws.Cell(1, 3).Value = "FechaOC";
                ws.Cell(1, 4).Value = "OrdenCompra";
                ws.Cell(1, 5).Value = "MontoOC";
                ws.Cell(1, 6).Value = "FechaFacProv";
                ws.Cell(1, 7).Value = "FacProv";
                ws.Cell(1, 8).Value = "MontoFP";
                ws.Cell(1, 9).Value = "Serie";
                ws.Cell(1, 10).Value = "Factura";
                ws.Cell(1, 11).Value = "Fecha Pago";
                ws.Cell(1, 12).Value = "Pago";
                ws.Cell(1, 13).Value = "Monto Pagado";

                int row = 2;
                foreach (var item in datos)
                {
                    ws.Cell(row, 1).Value = item.Codigo;
                    ws.Cell(row, 2).Value = item.Nombre;
                    ws.Cell(row, 3).Value = item.FechaOC;
                    ws.Cell(row, 4).Value = item.OrdenCompra;
                    ws.Cell(row, 5).Value = item.MontoOC;
                    ws.Cell(row, 6).Value = item.FechaFacProv;
                    ws.Cell(row, 7).Value = item.FacProv;
                    ws.Cell(row, 8).Value = item.MontoFP;
                    ws.Cell(row, 9).Value = item.Serie;
                    ws.Cell(row, 10).Value = item.Factura;
                    ws.Cell(row, 11).Value = item.FechaPago;
                    ws.Cell(row, 12).Value = item.Pago;
                    ws.Cell(row, 13).Value = item.MontoPagado;

                    row++;
                }

                using (var stream = new System.IO.MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    string fileName = $"EstadoCuentaImportacion_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
    }
}