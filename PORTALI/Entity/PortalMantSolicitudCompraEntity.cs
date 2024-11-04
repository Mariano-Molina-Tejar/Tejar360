using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class PortalMantSolicitudCompraEntity
    {
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public int IdSucursal { get; set; }
        public int Depto { get; set; }
        public DateTime FechaDocumento { get; set; }
        public DateTime FechaNecesaria { get; set; }
        public string  Tienda { get; set; }
        public string  Correo { get; set; }
        public string  Notas { get; set; }
        public string UserCode { get; set; }
        public DateTime FechaHoy { get; set; }
        public double? Presupuesto { get; set; }
        public List<PortaSucursalesEntity> ListaSucursales { get; set; }
        public List<PortalListadoDeptosEntity> ListadoDeptos { get; set; }
        public List<PortalSolicitudDetalleEntity> ListadoProductos { get; set; }


    }
}
