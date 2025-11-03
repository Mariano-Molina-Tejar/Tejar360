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
        public List<HistorialAutorizaciones> HistorialAuto { get; set; }        
        public string EsAsesor { get; set; }
    }

    public class HistoralNotas 
    {
        public int Code { get; set; }
        public string Notas { get; set; }
    }
    public class PortalListadoCotizacionesEntity
    {
        public string EstadoCoti { get; set; }
        public string DscrTipoCoti { get; set; }
        public int Llave { get; set; }
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public int DocNumFac { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string NitDocto { get; set; }
        public string NitCardCode { get; set; }
        public string Nit { get; set; }
        public string FacNombre { get; set; }
        public string Address { get; set; }
        public string DireccionCardCode { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime DocDueDate { get; set; }
        public string CorreoCliente { get; set; }
        public string TelefonoCliente { get; set; }
        public string DomicilioCliente { get; set; }
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
        public string Hora { get; set; }
        public int IdTipoCrm { get; set; }
        public string DscrpTipoCrm { get; set; }
        public string CorreoDocto { get; set; }
        public string TelefonoDocto { get; set; }
        public int CountCotizaciones { get; set; }
        public double DocTotalCoti { get; set; }
        public int CountFacturas { get; set; }
        public double DocTotalFact { get; set; }
        public string Contacto { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public int PriceList { get; set; }
        public string Promocion { get; set; }
        public int Tipo { get; set; }
        public string Estado { get; set; }
        public string WhsCode { get; set; }
        public string WhsName { get; set; }
        public int ListNum { get; set; }
        public string ListName { get; set; }
        public string Texto { get; set; }
        public List<CrmGraficoDonaCotiEntity> DatosGraficosDona { get; set; }
        public List<HistoralNotas> HistorialNotas { get; set; }
    }

    public class PortalCotizacionesDetalleEntity
    {
        public string ImagenUrl { get; set; }
        public int LineNum { get; set; }
        public string ItemCode { get; set; }
        public string Dscription { get; set; }
        public double Quantity { get; set; }
        public double Descuento { get; set; }
        public int PriceList { get; set; }
        public string Promocion { get; set; }
        public double PrecioReal { get; set; }
        public double DsctoPor { get; set; }
        public double DsctoNuevoPr { get; set; }
        public double DsctoListaPr { get; set; }
        public double DsctoQuetzal { get; set; }
        public int DescuentoLpr { get; set; }
        public double DescuentoPor { get; set; }
        public double DescuentoQtz { get; set; }
        public double Price { get; set; }
        public double LineTotal { get; set; }
        public double Iva { get; set; }
        public int CodigoPromo { get; set; }
        public int Rw { get; set; }
    }

    public class HistorialAutorizaciones 
    {
        public int Id { get; set; }
        public int DocEntryOld { get; set; }
        public int DocEntryNew { get; set; }
        public int DocNumOld { get; set; }
        public int DocNumNew { get; set; }
        public string Notas { get; set; }
        public int IdEstado { get; set; }
        public string Estado { get; set; }
        public string Usuario { get; set; }
        public DateTime FechaHora { get; set; }
    }
}
