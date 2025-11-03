using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class EditarCotizacionEntity
    {
        public EditEncabezado Encabezado { get; set; }
        public List<EditDetalle> Detalle { get; set; }
    }
    public class EditEncabezado
    {
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public string Nit { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string Direccion { get; set; }
        public string Contacto { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Nota { get; set; }

    }
    public class EditDetalle 
    {
        public string ItemCode { get; set; }
        public int LineNum { get; set; }
        public double Cantidad { get; set; }
        public double Precio { get; set; }
        public double Descuento { get; set; }
        public string WhsCode { get; set; }
        public double PrecioCDescto { get; set; }
        public double LineTotal { get; set; }
        public int ListPrice { get; set; }
        public string Promocion { get; set; }
    }
}
