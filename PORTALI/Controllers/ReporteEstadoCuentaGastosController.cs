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
    public class ReporteEstadoCuentaGastosController : BaseReporteController
    {
        private readonly DALReporteEstadoCuentaGastos _dalEstadoCuentaGastos;

        public ReporteEstadoCuentaGastosController()
        {
            _dalEstadoCuentaGastos = new DALReporteEstadoCuentaGastos();
        }

        private async Task CargarProveedores()
        {
            //Llamamos al DAL que obtiene Proveedores (sp separado del principal)
            var dalProv = new DALReporteEstadoCuentaGastos();
            var proveedores = await dalProv.ObtenerProveedores(); //Metodo que devuelve lista de proveedores
            ViewBag.Proveedores = new SelectList(proveedores, "Codigo", "Nombre");
        }


        // GET: ReporteEstadoCuentaGastos
        public async Task<ActionResult> Index(string fechaInicio, string fechaFinal)
        {
            var filtroBase = ObtenerFiltroConFechas(fechaInicio, fechaFinal);
            var filtro = new FiltroEstadoCuentaGastos
            {
                FechaInicio = filtroBase.FechaInicio,
                FechaFin = filtroBase.FechaFin
            };

            ViewBag.Resultado = new List<ReporteEstadoCuentaGastosEntity>();
            await CargarProveedores();
            return View(filtro);

        }

        [HttpPost]
        public async Task<ActionResult> Index(FiltroEstadoCuentaGastos filtro)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Resultado = new List<ReporteEstadoCuentaGastosEntity>();
                await CargarProveedores();
                return View(filtro);
            }

            ViewBag.FechaInicio = filtro.FechaInicio.ToString("yyyy-MM-dd");
            ViewBag.FechaFina = filtro.FechaFin.ToString("yyyy-MM-dd");

            var resultado = await _dalEstadoCuentaGastos.ObtenerEstadoCuentaGastos(filtro);
            ViewBag.Resultado = resultado;

            await CargarProveedores();
            return View(filtro);

        }

        [HttpPost]
        public async Task<ActionResult> ExportarExcel(FiltroEstadoCuentaGastos filtro)
        {
            var datos = await _dalEstadoCuentaGastos.ObtenerEstadoCuentaGastos(filtro);

            using (var wb = new ClosedXML.Excel.XLWorkbook())
            {
                var ws = wb.Worksheets.Add("EstadoCuentaGastos");
                ws.Cell(1, 1).Value = "Fecha";
                ws.Cell(1, 2).Value = "Codídgo Artículo";
                ws.Cell(1, 3).Value = "Artículo";
                ws.Cell(1, 4).Value = "Codigo Proveedor";
                ws.Cell(1, 5).Value = "Proveedor";
                ws.Cell(1, 6).Value = "Gasto Total";

                int row = 2;
                foreach (var item in datos)
                {
                    ws.Cell(row, 1).Value = item.Fecha;
                    ws.Cell(row, 2).Value = item.CodigoArticulo;
                    ws.Cell(row, 3).Value = item.Articulo;
                    ws.Cell(row, 4).Value = item.Codigo;
                    ws.Cell(row, 5).Value = item.Nombre;
                    ws.Cell(row, 6).Value = item.GastoTotal;
                    row++;
                }

                using (var stream = new System.IO.MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    string fileName = $"EstadoCuentaGastos_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
       
    }
}

