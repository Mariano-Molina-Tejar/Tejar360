using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class CarritoComprasEntity
    {
        public string LicTradNum { get; set; }
        public string Cliente { get; set; }
        public int NoCotizacion { get; set; }
        public string Llave { get; set; }
        public int DocN { get; set; }
        public int DocNum { get; set; }
        public int DocEntry { get; set; }
        public int NoCotizacionV { get; set; }
        public string Nit { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string Telefono { get; set; }
        public string Contacto { get; set; }
        public string Correo { get; set; }
        public string Direccion { get; set; }
        public string FormaPago { get; set; }
        public int SlpCode { get; set; }
        public int IdSucursal { get; set; }
        public int IdSerie { get; set; }
        public int PriceList { get; set; }
        public string UserCode { get; set; }
        public string Comentarios { get; set; }
        public string FacturarNit { get; set; }
        public string FacturarNombre { get; set; }
        public string FacturarDireccion { get; set; }
        public string EsCf { get; set; }
        public bool AplicaSAC { get; set; }
        public string WhsCodeSAC { get; set; }
        public int SlpCodeSAC { get; set; }
        public int EsUsuSac { get; set; }
        public List<DetalleCarritoEntity> Detalle { get; set; }
        public List<Regalo> Regalo { get; set; }

    }

    public class Regalo 
    {
        public string ItemCode { get; set; }
        public double Cantidad { get; set; }
    }

    public class DetalleCarritoEntity
    {
        public int DocEn { get; set; }
        public int LineN { get; set; }
        public string Identity { get; set; }
        public int LineId { get; set; }
        public string ItemCode { get; set; }
        public string Dscription { get; set; }
        public double Quantity { get; set; }        
        public double DescuentoU { get; set; }
        public double Price { get; set; }
        public double Dscto { get; set; }
        public double Saving { get; set; }
        public double CantMetros { get; set; }
        public double LineTotal { get; set; }
        public string LinkImg { get; set; }
        public int EsPromo { get; set; }
        public int CodPromo { get; set; }
        public int DescuentoLpr { get; set; }
        public double DescuentoNwp { get; set; }
        public double DescuentoPor { get; set; }
        public double DescuentoQtz { get; set; }
        public string CodigoPromo { get; set; }
    }
}
