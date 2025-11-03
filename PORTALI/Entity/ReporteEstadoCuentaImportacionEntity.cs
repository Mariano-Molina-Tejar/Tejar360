using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class ReporteEstadoCuentaImportacionEntity
    {
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public DateTime FechaOC { get; set; }
        public string OrdenCompra { get; set; }
        public decimal MontoOC { get; set; }
        public DateTime FechaFacProv { get; set; }
        public string FacProv { get; set; }
        public decimal MontoFP { get; set; }
        public string Serie { get; set; }
        public string  Factura { get; set; }
        public DateTime FechaPago { get; set; }
        public string Pago { get; set; }
        public decimal MontoPagado { get; set; }
    }
}
