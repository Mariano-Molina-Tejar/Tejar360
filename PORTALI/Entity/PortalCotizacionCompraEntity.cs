using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class PortalCotizacionInicialCompraEntity
    {
        public List<PortalTotalCotizacionesEntity> totalCotizaciones { get; set; }
        public PortalTotalCotizacionesEntity Encabezado { get; set; }
        public List<PortalCotizacionDetalleEntity> DetalleProductos { get; set; }
    }

    public class PortalCotizacionDetalleEntity 
    {
        public int DocEntrySolC { get; set; }
        public int DocNumSolC { get; set; }
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public string ItemCode { get; set; }
        public string Dscription { get; set; }
        public int LineNum { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public double LineTotal { get; set; }
        public string LineText { get; set; }
    }

    public class PortalTotalCotizacionesEntity
    {
        public int  DocEntrySolC { get; set; }
        public int DocNumSolC { get; set; }
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public int DocEntryCoti { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string Solicitante { get; set; }
        public string FormaPago { get; set; }
        public string  Email { get; set; }
        public DateTime FechaEntrega { get; set; }
        public int Depto { get; set; }
        public double SubTotal { get; set; }
        public double Iva { get; set; }
        public double Total { get; set; }
        public int IdAutoriza { get; set; }
    }
}
