using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class EncabezadoVentaEditarEntity
    {
        public int DocN { get; set; }
        public int Llave { get; set; }
        public int NoCotizacion { get; set; }
        public string Cliente { get; set; }
        public string LicTradNum { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string Direccion { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Fecha { get; set; }
        public int PriceId { get; set; }
        public string FacturarNit { get; set; }
        public string FacturarNombre { get; set; }
        public string FacturarDireccion { get; set; }
        public string EsCF { get; set; }
        public string Borrador { get; set; }
        public List<ProductoEditarEntity> productos { get; set; }
    }

    public class ProductoEditarEntity
    {
        public int DocE { get; set; }
        public int LineN { get; set; }
        public string Identity { get; set; }
        public int LineId { get; set; }
        public string ItemCode { get; set; }
        public string Dscription { get; set; }
        public double Quantity { get; set; }
        public double ListPrice { get; set; }
        public double Price { get; set; }
        public double DescuentoU { get; set; }
        public double Dscto { get; set; }
        public double Saving { get; set; }
        public double LineTotal { get; set; }
        public double CantidadMetros { get; set; }
        public string LinkImg { get; set; }
        public string EsPromo { get; set; }
        public double DescuentoLpr { get; set; }
        public double DescuentoPor { get; set; }
        public double DescuentoQtz { get; set; }
        public double DescuentoNwp { get; set; }
    }

}
