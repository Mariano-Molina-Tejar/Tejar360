using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Filtros
{
    public class FiltroEstadoCuentaLocal:FechaFiltroEntity
    {
        public new DateTime? FechaInicio { get; set; }
        public new DateTime? FechaFin { get; set; }

        public string  Proveedor  { get; set; }
        public string  Ruta { get; set; }
        public string Destino { get; set; }
        public string  Factura { get; set; }

    }
}
