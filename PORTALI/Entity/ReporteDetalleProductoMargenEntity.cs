using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class ReporteDetalleProductoMargenEntity
    {
        public string Tipo { get; set; }
        public DateTime DocDate { get; set; }
        public string ItemCode { get; set; }
        public string Dscription { get; set; }
        public double Quantity { get; set; }
        public double Ganancia { get; set; }
        public double Base { get; set; }
        public double Margen { get; set; }
        public string Imagen { get; set; }

    }
}
