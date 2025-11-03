using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class ReporteEstadoCuentaLocalEntity
    {
        public string Ruta { get; set; }
        public string Origen { get; set; }
        public string  Destino { get; set; }
        public string CodDestino { get; set; }
        public string  Codigo { get; set; }
        public string  Nombre { get; set; }
        public DateTime FechaOC { get; set; }
        public string NumOC { get; set; }
        public decimal  MontoOC { get; set; }
        public DateTime FechaFacProv { get; set; }
        public decimal MontoFP { get; set; }
        public string  NumFP { get; set; }
        public string DetalleFP { get; set; }
        public DateTime FechaPago { get; set; }
        public string  NumPago { get; set; }
        public decimal MontoPago { get; set; }
        public string DetallePago { get; set; }
        public string Serie { get; set; }
        public string  Factura { get; set; }
    }
}
