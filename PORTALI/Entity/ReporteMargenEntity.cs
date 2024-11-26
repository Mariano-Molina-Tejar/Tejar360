using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{    
    public class ReporteMargenEntity
    {
        public string WhsCode { get; set; }
        public string WhsName { get; set; }
        public int SlpCode { get; set; }
        public string SlpName { get; set; }
        public double VentaActual { get; set; }
        public double VentaProyectada { get; set; }
        public double Rentabilidad { get; set; }
        public int Facturas { get; set; }
    }

    public class MargenesUtilidadEntity 
    {
        public List<PortalTiendasEntity> ListaTiendas { get; set; }
        public List<ReporteMargenEntity> ListaMargenes { get; set; }
    }
}
