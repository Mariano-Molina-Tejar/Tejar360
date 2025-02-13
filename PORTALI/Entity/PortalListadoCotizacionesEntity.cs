using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class CarritoComprasPDFEntity
    {
        public PortalListadoCotizacionesEntity Encabezado { get; set; }
        public List<PortalCotizacionesDetalleEntity> Detalle { get; set; }
    }
    public class PortalListadoCotizacionesEntity
    {
        public string EstadoCoti { get; set; }
        public string DscrTipoCoti { get; set; }
        public int Llave { get; set; }
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string Nit { get; set; }
        public string FacNombre { get; set; }
        public string Address { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime DocDueDate { get; set; }
        public int SlpCode { get; set; }
        public string SlpName { get; set; }
        public double SubTotal { get; set; }
        public double Impuesto { get; set; }
        public double Descuento { get; set; }
        public double DocTotal { get; set; }
        public string DireccionTejar { get; set; }
        public string Notas { get; set; }
        public string IsCookie { get; set; }
        public string Comments { get; set; }
    }

    public class PortalCotizacionesDetalleEntity 
    {
        public string ItemCode { get; set; }
        public string Dscription { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public double Descuento { get; set; }
        public double LineTotal { get; set; }
    }
}
