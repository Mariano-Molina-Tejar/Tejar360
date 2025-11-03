using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class GetSingleOcEntity
    {
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime DocDueDate { get; set; }
        public double DocTotal { get; set; }
        public int BPLId { get; set; }
        public string Comments { get; set; }
        public List<getSingleOcDetalleEntity> Detalle { get; set; }
    }

    public class getSingleOcDetalleEntity 
    {
        public string ItemCode { get; set; }
        public string Dscription { get; set; }
        public string TaxCode { get; set; }
        public double Quantity { get; set; }
        public double PriceAfVat { get; set; }
        public double GTotal { get; set; }
        public string OcrCode { get; set; }
    }
}
