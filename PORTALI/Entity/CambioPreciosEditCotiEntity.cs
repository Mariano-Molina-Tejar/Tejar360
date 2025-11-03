using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class CambioPreciosEditCotiEntity
    {
        public string ItemCode { get; set; }
        public double Quantity { get; set; }
        public double Descuento { get; set; }
        public double Price { get; set; }
        public double LineTotal { get; set; }
    }
}
