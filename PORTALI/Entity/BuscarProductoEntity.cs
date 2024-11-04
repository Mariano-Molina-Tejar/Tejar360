using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class BuscarProductoEntity
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string WhsCode { get; set; }
        public double Stock { get; set; }
        public double PrecioVenta { get; set; }
        public double PrecioBeneficio { get; set; }
        public string Formato { get; set; }
        public string Imagen { get; set; }
    }
}
