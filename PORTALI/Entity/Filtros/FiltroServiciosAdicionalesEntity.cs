using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Filtros
{
    public class FiltroServiciosAdicionalesEntity:FechaFiltroEntity
    {
        public string Proveedor { get; set; }
        public string OrdenCompra { get; set; }
        public string Factura { get; set; }
    }
}
