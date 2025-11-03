using Entity.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Charts;

namespace Entity.ViewModels
{
    public class ReporteDañadoViewModel
    {
        public FiltroDañadoEntity Filtro { get; set; }

        public List<ReporteDañadoEntity> Dañados { get; set; }

        public List<ReporteDañadoTransferencia> Transferencias { get; set; }

        public ReporteDañadoViewModel()
        {
            Filtro = new FiltroDañadoEntity();
            Dañados = new List<ReporteDañadoEntity>();
            Transferencias = new List<ReporteDañadoTransferencia>();

            Transportistas = new List<string>();
            Almacenes = new List<string>();
            ChartDañadoPorTienda = new List<DañadoPorTiendaChart>();
            ChartDañadoPorCategoria = new List<DañadoPorCategoriaChart>();
            ChartDañadoPorRuta = new List<DañadoPorRutaChart>();
            ChartDañadoPorTransportista = new List<DañadoPorTransportistaChart>();
            ChartMontoNCPorMes = new List<MontoNotaCreditoPorMesChart>();
            ChartPorcentajeDañadoAlmacenMes = new List<DañadoAlmacenPorMesChart>();
            ChartPorcentajeDañadoRutaMes = new List<DañadoRutaPorMesChart>();






        }

        public List<string> Transportistas { get; set; }
        public List<string> Almacenes { get; set; }

        //Totales para tarjetas tipo power Bi
        public decimal TotalMontoOV { get; set; }
        public decimal TotalMontoNC { get; set; }
        public decimal TotalTransferenciaDañado { get; set; }

        public decimal TotalMontoDañado => TotalMontoNC + TotalTransferenciaDañado;

        public decimal PorcentajeDañadoRutas => 
            TotalMontoOV != 0 ? TotalMontoNC / TotalMontoOV : 0;

        public decimal PorcentajeDañadoAlmacen =>
            TotalMontoOV != 0 ? TotalTransferenciaDañado / TotalMontoOV : 0;


        public decimal PorcentajeDañadoTotal =>
            TotalMontoOV != 0 ? (TotalMontoNC + TotalTransferenciaDañado) / TotalMontoOV : 0;

        //Grafica Dañado en Rutas por PDV
        public List<DañadoPorTiendaChart> ChartDañadoPorTienda { get; set; }
        
        //Grafica Dañado por Categoria
        public List<DañadoPorCategoriaChart> ChartDañadoPorCategoria { get; set; }

        //Grafica Dañado por Ruta
        public List<DañadoPorRutaChart> ChartDañadoPorRuta { get; set; }
        //Para Grafica dañado por Transportista
        public List<DañadoPorTransportistaChart> ChartDañadoPorTransportista { get; set; }
        //Para graficas MontoNC por mes
        public List<MontoNotaCreditoPorMesChart> ChartMontoNCPorMes { get; set; }
        //para grafica % Dañado en Almacen
        public List<DañadoAlmacenPorMesChart> ChartPorcentajeDañadoAlmacenMes { get; set; }

        //public List<DañadoAlmacenPorMesChart> ChartPorcentajeDañadoAlmacenMes { get; set; }
        //public decimal PromedioDañadoAlmacen { get; set; }
        //Para grafica Dañado en Rutas
        public List<DañadoRutaPorMesChart> ChartPorcentajeDañadoRutaMes { get; set; }

        public List<RelacionDañadosPorProcesosChart> ChartRelacionDañadoProcesos { get; set; }





    }

}
