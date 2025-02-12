using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class ItemsCotizacionEntity
    {
        public int LineId { get; set; }
        public string ItemCode { get; set; }
        public string Tipo { get; set; }
        public double DescuentoQ { get; set; }
        public double PreciUnitAlto { get; set; }
        public double Price { get; set; }        
    }
}