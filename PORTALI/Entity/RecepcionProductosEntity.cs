using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class RecepcionProductosEntity
    {
        public int DocEntryOc { get; set; }
        public int DocNumOc { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string Comments { get; set; }
        public DateTime DocDateCoti { get; set; }
        public DateTime DocDateOc { get; set; }
        public DateTime DocDateReq { get; set; }
        public double DocTotal { get; set; }
    }
}
