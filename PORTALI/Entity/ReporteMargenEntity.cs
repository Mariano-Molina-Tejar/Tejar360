using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{    
    public class ReporteMargenEntity
    {
        public string WhsCode { get; set; }
        public string WhsName { get; set; }
        public int SlpCode { get; set; }
        public string SlpName { get; set; }
        public double VentaActual { get; set; }
        public double VentaNormal { get; set; }
        public double VentaDescuento { get; set; }
        public double VentPromocion { get; set; }
        public double VentaProyectada { get; set; }
        public double Rentabilidad { get; set; }
        public double RentabilidadDecuento { get; set; }
        public double RentabilidadPromocion { get; set; }
        public int Facturas { get; set; }
    }
    public class Reporte_Margen_Facturas
    {
        public string Tipo { get; set; }
        public int DocNum { get; set; }
        public DateTime DocDate { get; set; }
        public string CardName { get; set; }
        public double Ganancia { get; set; }
        public double Base { get; set; }
        public double LineTotal { get; set; }
        public double Margen { get; set; }
    }
    public class Detalle_Margen_Factura
    {
        public string Tipo { get; set; }
        public int LineNum { get; set; }
        public int DocNum { get; set; }
        public DateTime DocDate { get; set; }

        public string CardCode { get; set; }
        public string CardName { get; set; }
        public double Quantity { get; set; }
        public string ItemCode { get; set; }
        public string Dscripcion { get; set; }
        public double LineTotal { get; set; }
        public double Margen { get; set; }
        public double MargenTotal { get; set; }
        public string TipoD { get; set; }
        public string PorcentajeAutorizado { get; set; }
    }
    public class MargenesUtilidadEntity 
    {
        public List<PortalTiendasEntity> ListaTiendas { get; set; }
        public List<ReporteMargenEntity> ListaMargenes { get; set; }
        public ReporteMargenEntity MargenTienda { get; set; }
    }
}
