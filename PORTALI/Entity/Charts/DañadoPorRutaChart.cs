using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Charts
{
    public class DañadoPorRutaChart
    {
        public string NoRutaOV { get; set; }
        public decimal MontoNC { get; set; }
        public decimal MontoOV { get; set; }
        public decimal PorcentajeDañadoRuta { get; set; } // Calculado = MontoNC / MontoOV
    }
}
