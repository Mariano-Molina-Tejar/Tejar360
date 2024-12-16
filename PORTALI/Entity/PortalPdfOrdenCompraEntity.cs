using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class PortalPdfOrdenCompraEntity
    {
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string NitSucursal { get; set; }
        public DateTime FechaEmision { get; set; }
        public DateTime FechaEntrega { get; set; }
        public int DocNum { get; set; }
        public string Nit { get; set; }
        public string Email { get; set; }
        public string NombreGrupo { get; set; }
        public string Sucursal { get; set; }
        public string DirEntrega { get; set; }
        public string Comentario { get; set; }
        public double GranTotal { get; set; }
        public string ElaboradoPor { get; set; }
        public string Telefono { get; set; }
        public List<PortalPdfOrdenCompraDetalleEntity> Detalle { get; set; }
    }

    public class PortalPdfOrdenCompraDetalleEntity 
    {
        public string ItemCode { get; set; }
        public string Dscription { get; set; }
        public string Almacen { get; set; }
        public string Umedida { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public double LineTotal { get; set; }
    }
}
