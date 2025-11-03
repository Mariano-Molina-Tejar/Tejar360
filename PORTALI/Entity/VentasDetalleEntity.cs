using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class VentasDetalleEntity
    {
        public string WhsCode { get; set; }
        public string WhsName { get; set; }
        public int SlpCode { get; set; }
        public string SlpName { get; set; }
        public double DiasLaborados { get; set; }
        public double DiasTranscurridos { get; set; }

        // Venta
        public double V_Meta { get; set; }
        public double V_Venta { get; set; }
        public double V_PromDiario { get; set; }
        public double V_Proyeccion { get; set; }
        public double V_MetaDeVentaPorcentaje { get; set; }

        // Utilidad
        public double U_Utilidad { get; set; }
        public double U_PromDiario { get; set; }
        public double U_Proyeccion { get; set; }
        public double U_Margen { get; set; }

        // Diario
        public double D_Venta { get; set; }
        public double D_Utilidad { get; set; }
        public double D_Margen { get; set; }
    }
}
