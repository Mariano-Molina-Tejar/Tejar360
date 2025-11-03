using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class CrmVentasEntity
    {
        public int DocEntry { get; set; }
        public int DocNumFac { get; set; }
        public DateTime DocDate { get; set; }
        public string DocNumCot { get; set; }
        public string Folio { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string WhsCode { get; set; }
        public string WhsName { get; set; }
        public int SlpCode { get; set; }
        public string SlpName { get; set; }
        public int IdRegion { get; set; }
        public string Region { get; set; }
        public double LineTotal { get; set; }
    }
}
