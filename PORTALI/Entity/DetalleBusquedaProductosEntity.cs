using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class DetalleBusquedaProductosEntity
    {
        public BuscarProductoEntity Encabezado { get; set; }
        public List<BusquedaDetalleTiendasEntity> Tiendas { get; set; }
        public BusquedaDetallePromocionesEntity Promociones { get; set; }
        public List<BuscarProductoEntity> Alternativos { get; set; }
        public List<BuscarProductoEntity> VentaCruzada { get; set; }
    }

    public class BusquedaDetalleTiendasEntity
    {
        public string WhsCode { get; set; }
        public string WhsName { get; set; }
        public double Stock { get; set; }
        public double Price { get; set; }
    }
    public class BusquedaDetallePromocionesEntity 
    {
        public string IdPromocion { get; set; }
        public string Tienda { get; set; }
        public double Precio { get; set; }
    }    
}
