using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class ReporteEstadoCuentaServiciosAdicionales
    {
        public string Ruta { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string Destino { get; set; }
        public DateTime FechaOC { get; set; }
        public int OrdenCompra { get; set; }
        public decimal MontoOC { get; set; }
        public DateTime FechaFacProv { get; set; }
        public int FacProv { get; set; }
        public decimal MontoFP { get; set; }
        public string Serie { get; set; }
        public string Factura { get; set; }
        public DateTime FechaPago { get; set; }
        public int Pago { get; set; }
        public decimal MontoPagado { get; set; }
    }
}
