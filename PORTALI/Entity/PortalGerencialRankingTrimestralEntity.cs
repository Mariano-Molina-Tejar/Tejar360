using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class PortalGerencialRankingTrimestralEntity
    {
        public int No { get; set; }
        public int SlpCode { get; set; }
        public string Tienda { get; set; }
        public string SlpName { get; set; }
        public double MetaMensual { get; set; }
        public double VentaMensual { get; set; }
        public double VentaProyectada { get; set; }
        public double Indice { get; set; }
        public List<PortalTrankingTrimestralAsesorEntity> ListaTrimestral { get; set; }
    }
}
