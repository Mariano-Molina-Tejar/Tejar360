using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class DashboardAsesoresEntity 
    {
        public int SlpCode { get; set; }
        public double MetaHoy { get; set; }
        public double VentasHoy { get; set; }
        public double RestanteHoy { get; set; }
        public double IndiceVentaHoy { get; set; }
        public double MetaUtilidad { get; set; }
        public double UtilidadHoy { get; set; }
        public double RestanteUtilidadHoy { get; set; }
        public double IndiceUtilidadHoy { get; set; }
        public double MetaVentaMes { get; set; }
        public double VentaMes { get; set; }
        public double IndiceVentaMes { get; set; }
        public double ProyeccionVentaMes { get; set; }
        public double MetaUtilidadMes { get; set; }
        public double UtilidadMes { get; set; }
        public double IndiceUtilidadMes { get; set; }
        public double ProyeccionUtilidadMes { get; set; }
        public int Ranking { get; set; }
        public int TotalFacturas { get; set; }
        public int ClientesAtendidos { get; set; }
        public int DiasTrans { get; set; }
        public int DiasMes { get; set; }
        public int DiasRestantes { get; set; }
    }    
}
