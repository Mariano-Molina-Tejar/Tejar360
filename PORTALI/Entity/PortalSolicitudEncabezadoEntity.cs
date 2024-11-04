using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class PortalSolicitudEncabezadoEntity
    {
        public int Rw { get; set; }
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public int DocNumOc { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime DocDueDate { get; set; }
        public DateTime ReqDate { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string Estado { get; set; }
        public string Comentarios { get; set; }
        public string IdEstado { get; set; }
        public string ColorEstado { get; set; }
        public string WhsCode { get; set; }
        public int IdSucursal { get; set; }
        public int Depto { get; set; }
        public List<PortalSolicitudDetalleEntity> Detalle { get; set; }        
    }

    public class PortalSolicitudDetalleEntity 
    {
        public string IdEstado { get; set; }
        public string ItemCode { get; set; }
        public string Dscription { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public double LineTotal { get; set; }
        public string NotasLinea { get; set; }
    }
}
