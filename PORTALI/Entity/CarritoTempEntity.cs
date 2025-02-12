using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class CarritoTempEntity
    {
        public int Llave { get; set; }
        public int NoCotizacion { get; set; }
        public string Cliente { get; set; }
        public string LicTradNum { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string Direccion { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public DateTime Fecha { get; set; }
        public int PriceId { get; set; }
        public string FacturarNit { get; set; }
        public string FacturarNombre { get; set; }
        public string FacturarDireccion { get; set; }
        public string EsCF { get; set; }
        public string Borrador { get; set; }
        public List<CarritoTemp2> Productos { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public double DocTotal { get; set; }
    }

    public class CarritoTemp2
    {
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
    }
}
