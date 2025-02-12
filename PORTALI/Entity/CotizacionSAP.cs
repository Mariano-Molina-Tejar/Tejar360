using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class CotizacionSAP
    {
        public int SlpCode { get; set; }
        public int Sucursal { get; set; }
        public EncabezadoSAP Encabezado { get; set; }
        public DetalleSAP Detalle { get; set; }
    }

    public class EncabezadoSAP
    {
        public string Nit { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
    }

    public class DetalleSAP
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public double Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double Descuento { get; set; }
    }
}
