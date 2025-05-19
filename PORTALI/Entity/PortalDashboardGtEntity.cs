using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class PortalDashboardGtEntity
    {
        public int WhsCode { get; set; }
        public string WhsName { get; set; }
        public double VentaTienda { get; set; }
        public double MetaTienda { get; set; }
        public double Indice { get; set; }        
        public double VentaTiendaProy { get; set; }
        public double IndiceProyectado { get; set; }
        public double UtilidadTienda { get; set; }
        public double MetaUtilidadTienda { get; set; }
        public double UtilidadIndice { get; set; }
        public double UtilidadTiendaProy { get; set; }
        public double IndiceUtilidadProy { get; set; }
        public List<PortalDashboardDetalleAsesorEntity> ListaAsesores { get; set; }
        public PortalChartPieEntity PieChartData { get; set; }
    }

    public class PortalDashboardDetalleAsesorEntity 
    {
        public int SlpCode { get; set; }
        public string SlpName { get; set; }
        public double VentaAsesor { get; set; }
        public double MetaAsesor { get; set; }
        public double Indice { get; set; }
        public double Peso { get; set; }
    }
}
