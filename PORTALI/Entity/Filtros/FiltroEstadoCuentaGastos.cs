using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Filtros
{
    public class FiltroEstadoCuentaGastos : FechaFiltroEntity
    {
        public string Proveedor { get; set; }
        public string  CodArticulo { get; set; }
    }
}
