using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class EstadoBolsonV2Entity
    {
        public string WhsCode { get; set; }
        public string WhsName { get; set; }
        public string Region { get; set; }
        public int InfGen_BolsonAcumuladoUni { get; set; }
        public int InfGen_BolsonNuevoUni { get; set; }
        public double InfGen_BolsonAcumuladoQ { get; set; }
        public double InfGen_BolsonNuevoQ { get; set; }
        public int CotNue_TotalCotizaciones { get; set; }
        public double CotNue_MontoCotizadoQ { get; set; }
        public double TasaDeCotizacion { get; set; }
        public int Fac_TotalFacturas { get; set; }
        public double Fac_MontoFacturasQ { get; set; }
        public double Fac_TasaCierre { get; set; }
        public double Per_TotalCoti { get; set; }
        public double Per_MontoPerdidoQ { get; set; }
        public double Per_TasaPerdida { get; set; }
    }
}
