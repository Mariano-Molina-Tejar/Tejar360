using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class OcEncabezadoEntity
    {
        public string CardCode { get; set; }
        public int IdSucursal { get; set; }
        public DateTime FechaDocumento { get; set; }
        public DateTime FechaComprometido { get; set; }
        public string Notas { get; set; }
        public string Usuario { get; set; }
        public string Moneda { get; set; }
        public List<OcDetalleEntity> Detalle { get; set; }
    }
    public class OcDetalleEntity
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public double Quantity { get; set; }
        public double PriceU { get; set; }
        public string CentroCosto { get; set; }
        public string Proyecto { get; set; }
        public string Etapa { get; set; }
        public string TipoArticulo { get; set; }
        public string TipoImpuesto { get; set; }
    }
}
