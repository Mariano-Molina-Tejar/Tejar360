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
        public double PrecioPromocion { get; set; }
        public string TipoPromo { get; set; }
        public string Formato { get; set; }
        public string DetallePromo { get; set; }
        public string Imagen { get; set; }
        public int EsPromo { get; set; }
        public string Ambiente { get; set; }
        public string Metraje { get; set; }
        public double Descuento { get; set; }
        public int IdGrupo { get; set; }
        public double MetrosDisp { get; set; }
        public int PLFinal { get; set; }
        public double PrecioFinal { get; set; }
    }
}
