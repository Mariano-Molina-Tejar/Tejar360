using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class OcPrintEntity
    {
        public string NombreSucursal { get; set; }
        public string NitSucursal { get; set; }
        public int DocNum { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime DocDueDate { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string E_Mail { get; set; }
        public string Address2 { get; set; }
        public string PymntGroup { get; set; }
        public string DocCur { get; set; }
        public string U_Name { get; set; }        
        public double Subtotal { get; set; }
        public double Iva { get; set; }
        public double DocTotal { get; set; }
        public string Comments { get; set; }
        public List<DetalleOc> Detalle { get; set; }
    }

    public class DetalleOc
    {
        public string ItemCode { get; set; }
        public string Dscription { get; set; }
        public string Almacen { get; set; }
        public string UnidadMed { get; set; }
        public double Quantity { get; set; }
        public double PrecioU { get; set; }
        public double LineTotal { get; set; }
    }
}
