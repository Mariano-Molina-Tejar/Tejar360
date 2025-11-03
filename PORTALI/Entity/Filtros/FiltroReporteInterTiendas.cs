using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Filtros
{
    public class FiltroReporteInterTiendas:FechaFiltroEntity
    {
        public new DateTime? FechaInicio { get; set; }
        public new DateTime? FechaFin { get; set; }

        public string  Transporte { get; set; }
        public int  Ruta { get; set; }
        public string NoOrdenVenta { get; set; }
        public string NoOrdenCompra { get; set; }

    }
}
