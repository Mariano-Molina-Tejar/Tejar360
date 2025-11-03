using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class EstadoBolsonV3Entity
    {
        public string WhsCode { get; set; }
        public string WhsName { get; set; }
        public int SlpCode { get; set; }
        public string SlpName { get; set; }
        public int IdRegion { get; set; }
        public string Region { get; set; }
        public int Semana { get; set; }

        public int CotGen_TotalU { get; set; }
        public double CotGen_TotalQ { get; set; }
        public double TicketPromedio { get; set; }

        public int CotiFac_TotalU { get; set; }
        public double CotiFac_TotalQ { get; set; }
        public double CotiFac_TicketPromedio { get; set; }
        public double CotiFac_Tasa { get; set; }

        public int CotiPer_TotalU { get; set; }
        public double CotiPer_TotalQ { get; set; }
        public double CotiPer_TicketPromedio { get; set; }
        public double CotiPer_Tasa { get; set; }

        public int TotalAb_TotalU { get; set; }
        public double TotalAb_TotalQ { get; set; }
        public double TotalAb_TicketPromedio { get; set; }
        public double TotalAb_Tasa { get; set; }
    }
}
