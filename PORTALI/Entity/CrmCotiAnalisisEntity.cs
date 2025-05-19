using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class CrmCotiAnalisisEntity
    {
        public int SlpCode { get; set; }
        public string SlpName { get; set; }
        public double TotalCotAbiertas { get; set; }
        public double TotalCotCerradas { get; set; }
        public double Indice { get; set; }
        public double MetaCoti { get; set; }
        public double IndiceMetaCoti { get; set; }
    }
}
