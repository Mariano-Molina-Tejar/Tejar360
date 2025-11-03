using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class PremiosPromosEntity
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string UrlImagen { get; set; }
        public int Regalito { get; set; }
        public int Sugerido { get; set; }
        public double Precio { get; set; }        
        public int Stock { get; set; }        
    }
}
