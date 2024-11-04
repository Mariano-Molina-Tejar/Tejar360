using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class RecepcionProductosDetalleEntity
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string LineText { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public double Total { get; set; }
    }
}