using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class MargenTiendasEntity
    {
        public string WhsCode { get; set; }
        public string WhsName { get; set; }
        public int IdRegion { get; set; }
        public string Region { get; set; }
        public double VentaActual { get; set; }
        public double VentaDescuento { get; set; }
        public double VentaPromocion { get; set; }
        public double VentaProyectada { get; set; }
        public double Rentabilidad { get; set; }
        public double RentabilidadDescuento { get; set; }
        public double RentabilidadPromocion { get; set; }
        public int Facturas { get; set; }
    }
}
