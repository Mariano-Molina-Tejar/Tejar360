using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class ReporteEstadoCuentaGastosEntity
    {
        public DateTime Fecha { get; set; }
        public string CodigoArticulo { get; set; }
        public string Articulo { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public decimal GastoTotal { get; set; }
    }
}
