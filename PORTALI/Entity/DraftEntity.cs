using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class DraftEntity
    {
        public List<Detalle> Listado { get; set; }
    }

    public class Detalle
    {
        public int DocEntry { get; set; }
        public string ItemCode { get; set; }
        public int LineNum { get; set; }
        public double Quantity { get; set; }
        public int IdAuto { get; set; }
        public double DescuentoP { get; set; }
        public double DescuentoQ { get; set; }
        public int DescuentoLpr { get; set; }
        public double DescuentoNwp { get; set; }
    }
}
