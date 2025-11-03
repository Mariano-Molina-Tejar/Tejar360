using DAL;
using Entity;
using Entity.Charts;
using Entity.Filtros;
using Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PORTALI.Controllers
{
    public class ReporteDañadoController : BaseReporteController
    {
        private readonly DALReporteDañado _dal = new DALReporteDañado();

      
        public async Task<ActionResult> ReporteDañado()
        {
            var filtro = new FiltroDañadoEntity();
            var hoy = DateTime.Today;
            filtro.Validar();

            var dal = new DALReporteDañado();

            //(usando el filtro nomrmal de fecha)
            var dañados = await dal.ObtenerDañados(filtro);
            var transferencias = await dal.ObtenerTransferenciaDañados(filtro);

            //// DAÑADOS y TRANSFERENCIAS para gráficas por mes (enero a mes actual)
            //var dañadosGraficas = await dal.ObtenerDañados(new FiltroDañadoEntity
            //{
            //    FechaInicio = filtro.FechaInicioGraficas,
            //    FechaFin = filtro.FechaFinGraficas
            //});
            //var transferenciasGraficas = await dal.ObtenerTransferenciaDañados(new FiltroDañadoEntity
            //{
            //    FechaInicio = filtro.FechaInicioGraficas,
            //    FechaFin = filtro.FechaFinGraficas
            //});
            //Obtiene fecha desde enero hasta hoy para graficas por mes
            var filtroCompleto = new FiltroDañadoEntity
            {
                FechaInicio = new DateTime(hoy.Year, 1, 1),
                FechaFin = hoy,
                AlmacenSeleccionado = filtro.AlmacenSeleccionado,
                TranportistaSeleccionado = filtro.TranportistaSeleccionado
            };

            //Hacemos una cosulta por tipo
            var todosDañados = await dal.ObtenerDañados(filtroCompleto);
            var todasTransferencias = await dal.ObtenerTransferenciaDañados(filtroCompleto);

            //Filtra en memoria según el filtro actual (para tarjetas y gráficas comunes
            //var dañados = todosDañados
            //    .Where(x => x.Fecha >= filtro.FechaInicio && x.Fecha <= filtro.FechaFin)
            //    .ToList();

            //var transferencias = todasTransferencias
            //    .Where(x => x.Fecha >= filtro.FechaInicio && x.Fecha <= filtro.FechaFin)
            //    .ToList();


            var almacenes = dal.ObtenerAlmacenes(dañados);
            var transportistas = dal.ObtenerTransportistas(dañados);

            var totalOV = dañados.Sum(x => x.MontoOV);

            //Grafico dañado en ruta
            var chartData = dañados
                .GroupBy(x => x.Tienda)
                .Select(g => new DañadoPorTiendaChart
                {
                    Tienda = g.Key,
                    MontoNC = g.Sum(x => x.MontoNC),
                    PorcentajeDañadoRutas = totalOV != 0 ? g.Sum(x => x.MontoNC) / totalOV : 0
                })
                .ToList();

            //Grafico dañado por Categoria
            var chartCategoria = dañados
                .GroupBy(x => x.Categoria)
                .Select(g => new DañadoPorCategoriaChart
                 {
                     Categoria = g.Key,
                     MontoNC = g.Sum(x => x.MontoNC),
                     PorcentajeDañado = totalOV != 0 ? g.Sum(x => x.MontoNC) / totalOV : 0
                 })
                 .ToList();

            //Para grafica de % Dañado por ruta
            var rutasChart = dañados
                .GroupBy(x => x.RutaOV)
                .Select(g => new DañadoPorRutaChart
                {
                    NoRutaOV = g.Key,
                    MontoNC = g.Sum(x => x.MontoNC),
                    MontoOV = g.Sum(x => x.MontoOV),
                    PorcentajeDañadoRuta = g.Sum(x => x.MontoOV) != 0
                        ? g.Sum(x => x.MontoNC) / g.Sum(x => x.MontoOV)*100
                        : 0
                }).ToList();

            //Para grafica de % dañado por transportista
            var chartTransportistas = dañados
                .GroupBy(x => x.Transportista)
                .Select(g => new DañadoPorTransportistaChart
                {
                    Transportista = g.Key,
                    MontoNC = g.Sum(x => x.MontoNC),
                    MontoOV = g.Sum(x => x.MontoOV),
                    PorcentajeDañadoTransportista = g.Sum(x => x.MontoOV) != 0
                        ? (g.Sum(x => x.MontoNC) / g.Sum(x => x.MontoOV)) * 100
                        : 0
                }).ToList();

            //para grafica montoNC por mes
            var chartMontoMeses = dañados
                .GroupBy(x => x.Fecha.ToString("MMMM", new System.Globalization.CultureInfo("es-ES")))
                .Select(g => new MontoNotaCreditoPorMesChart
                {
                    Mes = g.Key,
                    MontoNC = g.Sum(x => x.MontoNC)
                })
                .OrderBy(x => DateTime.ParseExact(x.Mes, "MMMM", new System.Globalization.CultureInfo("es-ES")).Month)
                .ToList();

            //Gráfica % Dañado en Almacén por mes
            var porcentajeDañadoAlmacenMes = transferencias
                .GroupBy(t => t.Fecha.ToString("MMMM", new System.Globalization.CultureInfo("es-ES")))
                .Select(g =>
                {
                    var mes = g.Key;
                    var totalTransferencias = g.Sum(x => x.Total);

                    var montoOV = dañados
                        .Where(d => d.Fecha.ToString("MMMM", new System.Globalization.CultureInfo("es-ES")) == mes)
                        .Sum(d => d.MontoOV);

                    return new DañadoAlmacenPorMesChart
                    {
                        Mes = mes,
                        Total = totalTransferencias,
                        MontoOV = montoOV,
                        PorcentajeMensual = montoOV != 0 ? (totalTransferencias / montoOV) * 100 : 0
                    };
                })
                .OrderBy(x => DateTime.ParseExact(x.Mes, "MMMM", new System.Globalization.CultureInfo("es-ES")).Month)
                .ToList();



            // Porcentaje de dañado en rutas por mes
            var charDañadoRutaMes = dañados
                .GroupBy(x => x.Fecha.ToString("MMMM", new System.Globalization.CultureInfo("es-ES")))
                .Select(g => new DañadoRutaPorMesChart
                {
                    Mes = g.Key,
                    MontoNC = g.Sum(x => x.MontoNC),
                    MontoOV = g.Sum(x => x.MontoOV),
                    PorcentajeDañdoRutaMensual = g.Sum(x => x.MontoOV) != 0
                       ? (g.Sum(x => x.MontoNC) / g.Sum(x => x.MontoOV)) * 100
                       : 0

                })
                 .OrderBy(x => DateTime.ParseExact(x.Mes, "MMMM", new System.Globalization.CultureInfo("es-ES")).Month)
                .ToList();

            //Para grafica Relacion de daños por procesos
            var porcentajeStackedChart = transferencias
             .GroupBy(t => t.Fecha.ToString("MM - MMMM", new CultureInfo("es-ES")))
             .Select(g =>
             {
                 var mes = g.Key;
                 var totalTransferencia = g.Sum(x => x.Total);

                 var montoOVmes = dañados
                     .Where(d => d.Fecha.ToString("MM - MMMM", new CultureInfo("es-ES")) == mes)
                     .Sum(d => d.MontoOV);

                 var montoNCmes = dañados
                     .Where(d => d.Fecha.ToString("MM - MMMM", new CultureInfo("es-ES")) == mes)
                     .Sum(d => d.MontoNC);

                 var porcentajeRuta = montoOVmes != 0 ? (montoNCmes / montoOVmes) * 100 : 0;
                 var porcentajeAlmacen = montoOVmes != 0 ? (totalTransferencia / montoOVmes) * 100 : 0;

                 return new RelacionDañadosPorProcesosChart
                 {
                     Mes = mes,
                     PorcentajeRuta = porcentajeRuta,
                     PorcentajeAlmacen = porcentajeAlmacen
                 };
             })
             .OrderBy(x => DateTime.ParseExact(x.Mes, "MM - MMMM", new CultureInfo("es-ES")).Month)
             .ToList();



            var viewModel = new ReporteDañadoViewModel
            {
                Filtro = filtro,
                Dañados = dañados,
                Transferencias = transferencias,
                Almacenes = almacenes,
                Transportistas = transportistas,

                //Totales para Tarjtea
                TotalMontoOV = dañados.Sum(x => x.MontoOV),
                TotalMontoNC = dañados.Sum(x => x.MontoNC),
                TotalTransferenciaDañado = transferencias.Sum(x => x.Total),
                //Graficos
                ChartDañadoPorTienda = chartData,
                ChartDañadoPorCategoria = chartCategoria,
                ChartDañadoPorRuta = rutasChart,
                ChartDañadoPorTransportista = chartTransportistas,
                ChartMontoNCPorMes = chartMontoMeses,
               // ChartPorcentajeDañadoAlmacenMes = porcentajePorMes,
                //PromedioDañadoAlmacen = promedioGeneralDañadoAlmacen,
                ChartPorcentajeDañadoRutaMes = charDañadoRutaMes,
                ChartPorcentajeDañadoAlmacenMes = porcentajeDañadoAlmacenMes,
                ChartRelacionDañadoProcesos = porcentajeStackedChart



            };



            return View(viewModel);
        }



        [HttpPost]
        public async Task<ActionResult> ReporteDañado(FiltroDañadoEntity filtro)
        {
            var dal = new DALReporteDañado();
            DateTime hoy = DateTime.Today;

            var dañados = await dal.ObtenerDañados(filtro);
            var transferencias = await dal.ObtenerTransferenciaDañados(filtro);

            //Obtiene fecha desde enero hasta hoy para graficas por mes
            var filtroCompleto = new FiltroDañadoEntity 
            { 
                FechaInicio = new DateTime(hoy.Year, 1,1),
                FechaFin = hoy,
                AlmacenSeleccionado = filtro.AlmacenSeleccionado,
                TranportistaSeleccionado = filtro.TranportistaSeleccionado
            };

            //Hacemos una cosulta por tipo
            var todosDañados = await dal.ObtenerDañados(filtroCompleto);
            var todasTransferencias = await dal.ObtenerTransferenciaDañados(filtroCompleto);

            //Filtra en memoria según el filtro actual (para tarjetas y gráficas comunes
            //var dañados = todosDañados
            //    .Where(x => x.Fecha >= filtro.FechaInicio && x.Fecha <= filtro.FechaFin)
            //    .ToList();

            //var transferencias = todasTransferencias
            //    .Where(x => x.Fecha >= filtro.FechaInicio && x.Fecha <= filtro.FechaFin)
            //    .ToList();


            //var filtroGraficas = new FiltroDañadoEntity
            //{
            //    FechaInicio = new DateTime(hoy.Year, 1, 1),
            //    FechaFin = hoy
            //};

            //// DAÑADOS y TRANSFERENCIAS para gráficas por mes (enero a mes actual)
            //var dañadosGraficas = await dal.ObtenerDañados(filtroGraficas);
            //var transferenciasGraficas = await dal.ObtenerTransferenciaDañados(filtroGraficas);
            //var dañadosGraficas = await dal.ObtenerDañados(new FiltroDañadoEntity
            //{
            //    FechaInicio = filtro.FechaInicioGraficas,
            //    FechaFin = filtro.FechaFinGraficas
            //});
            //var transferenciasGraficas = await dal.ObtenerTransferenciaDañados(new FiltroDañadoEntity
            //{
            //    FechaInicio = filtro.FechaInicioGraficas,
            //    FechaFin = filtro.FechaFinGraficas
            //});

            var almacenes = dal.ObtenerAlmacenes(dañados);
            var transportistas = dal.ObtenerTransportistas(dañados);

            var totalOV = dañados.Sum(x => x.MontoOV);

            //Grafica dañado en Rutas por PDV
            var chartData = dañados
                .GroupBy(x => x.Tienda)
                .Select(g => new DañadoPorTiendaChart
                {
                    Tienda = g.Key,
                    MontoNC = g.Sum(x => x.MontoNC),
                    PorcentajeDañadoRutas = totalOV != 0 ? g.Sum(x => x.MontoNC) / totalOV : 0
                })
                .ToList();

            //Grafico dañado por Categoria
            var chartCategoria = dañados
                .GroupBy(x => x.Categoria)
                .Select(g => new DañadoPorCategoriaChart
                {
                    Categoria = g.Key,
                    MontoNC = g.Sum(x => x.MontoNC),
                    PorcentajeDañado = totalOV != 0 ? g.Sum(x => x.MontoNC) / totalOV : 0
                })
                 .ToList();

            //Para Grafica % Dañado por ruta
            var rutasChart = dañados
                .GroupBy(x => x.RutaOV)
                .Select(g => new DañadoPorRutaChart
                {
                    NoRutaOV = g.Key,
                    MontoNC = g.Sum(x => x.MontoNC),
                    MontoOV = g.Sum(x => x.MontoOV),
                    PorcentajeDañadoRuta = g.Sum(x => x.MontoOV) != 0
                        ? g.Sum(x => x.MontoNC) / g.Sum(x => x.MontoOV)
                        : 0
                }).ToList();

            //Para grafica de % dañado por transportista
            var chartTransportistas = dañados
                .GroupBy(x => x.Transportista)
                .Select(g => new DañadoPorTransportistaChart
                {
                    Transportista = g.Key,
                    MontoNC = g.Sum(x => x.MontoNC),
                    MontoOV = g.Sum(x => x.MontoOV),
                    PorcentajeDañadoTransportista = g.Sum(x => x.MontoOV) != 0
                        ? (g.Sum(x => x.MontoNC) / g.Sum(x => x.MontoOV)) * 100
                        : 0
                }).ToList();

            //para grafica montoNC por mes
            var chartMontoMeses = dañados
                .GroupBy(x => x.Fecha.ToString("MMMM", new System.Globalization.CultureInfo("es-ES")))
                .Select(g => new MontoNotaCreditoPorMesChart
                {
                    Mes = g.Key,
                    MontoNC = g.Sum(x => x.MontoNC)
                })
                .OrderBy(x => DateTime.ParseExact(x.Mes, "MMMM", new System.Globalization.CultureInfo("es-ES")).Month)
                .ToList();
                    
                 // Porcentaje de dañado en rutas por mes
            var charDañadoRutaMes = dañados
                .GroupBy(x => x.Fecha.ToString("MMMM", new System.Globalization.CultureInfo("es-ES")))
                .Select(g => new DañadoRutaPorMesChart
                {
                    Mes = g.Key,
                    MontoNC = g.Sum(x => x.MontoNC),
                    MontoOV = g.Sum(x => x.MontoOV),
                    PorcentajeDañdoRutaMensual = g.Sum(x => x.MontoOV) != 0
                       ? (g.Sum(x => x.MontoNC) / g.Sum(x => x.MontoOV)) * 100
                       : 0
                    
                })
                .OrderBy(x => DateTime.ParseExact(x.Mes, "MMMM", new System.Globalization.CultureInfo("es-ES")).Month)
                .ToList();


            // Gráfica % Dañado en Almacén por mes            
            var porcentajeDañadoAlmacenMes = transferencias
                 .GroupBy(t => new
                 {
                     Mes = t.Fecha.ToString("MMMM", new CultureInfo("es-ES")),
                     MesNumero = t.Fecha.Month
                 })
                 .Select(g =>
                 {
                     var mes = g.Key.Mes;
                     var mesNumero = g.Key.MesNumero;
                     var totalTransferencias = g.Sum(x => x.Total);

                     var montoOV = dañados
                         .Where(d => d.Fecha.Month == mesNumero)
                         .Sum(d => d.MontoOV);

                     return new DañadoAlmacenPorMesChart
                     {
                         Mes = mes,
                         MesNumero = mesNumero,
                         Total = totalTransferencias,
                         MontoOV = montoOV,
                         PorcentajeMensual = montoOV != 0 ? (totalTransferencias / montoOV) * 100 : 0
                     };
                 })
                 .OrderBy(x => x.MesNumero)
                 .ToList();


            //Para grafica Relacion de daños por procesos
            var porcentajeStackedChart = transferencias
             .GroupBy(t => t.Fecha.ToString("MM - MMMM", new CultureInfo("es-ES")))
             .Select(g =>
             {
                 var mes = g.Key;
                 var totalTransferencia = g.Sum(x => x.Total);

                 var montoOVmes = dañados
                     .Where(d => d.Fecha.ToString("MM - MMMM", new CultureInfo("es-ES")) == mes)
                     .Sum(d => d.MontoOV);

                 var montoNCmes = dañados
                     .Where(d => d.Fecha.ToString("MM - MMMM", new CultureInfo("es-ES")) == mes)
                     .Sum(d => d.MontoNC);

                 var porcentajeRuta = montoOVmes != 0 ? (montoNCmes / montoOVmes) * 100 : 0;
                 var porcentajeAlmacen = montoOVmes != 0 ? (totalTransferencia / montoOVmes) * 100 : 0;

                 return new RelacionDañadosPorProcesosChart
                 {
                     Mes = mes,
                     PorcentajeRuta = porcentajeRuta,
                     PorcentajeAlmacen = porcentajeAlmacen
                 };
             })
             .OrderBy(x => DateTime.ParseExact(x.Mes, "MM - MMMM", new CultureInfo("es-ES")).Month)
             .ToList();




            var viewModel = new ReporteDañadoViewModel
            {
                Filtro = filtro,
                Dañados = dañados,
                Transferencias = transferencias,
                Almacenes = almacenes,
                //Montos
                Transportistas = transportistas,
                TotalMontoOV = dañados.Sum(x => x.MontoOV),
                TotalMontoNC = dañados.Sum(x => x.MontoNC),
                TotalTransferenciaDañado = transferencias.Sum(x => x.Total),
                //Graficas
                ChartDañadoPorTienda = chartData,
                ChartDañadoPorCategoria = chartCategoria,
                ChartDañadoPorRuta = rutasChart,
                ChartDañadoPorTransportista = chartTransportistas,
                ChartMontoNCPorMes = chartMontoMeses,
                //ChartPorcentajeDañadoAlmacenMes = porcentajePorMes,
                //PromedioDañadoAlmacen = promedioGeneralDañadoAlmacen,
                ChartPorcentajeDañadoRutaMes = charDañadoRutaMes,
                ChartPorcentajeDañadoAlmacenMes = porcentajeDañadoAlmacenMes,
                ChartRelacionDañadoProcesos = porcentajeStackedChart



            };

            return View(viewModel);
        }

    }
}