using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Filtros
{
    public class FiltroReporteCheckList : FechaFiltroEntity
    {
        public DateTime? FechaInicio { get; set; }
        public  DateTime? FechaFin { get; set; }

        public string Area { get; set; }

        public string  Region { get; set; }

        public string Tienda { get; set; }

        public string CodigoTienda { get; set; }
        public int? CodigoArea { get; set; }
        public int? CodigoRegion { get; set; }

    }
}
