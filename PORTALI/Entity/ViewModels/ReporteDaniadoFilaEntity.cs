using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.ViewModels
{
    public class ReporteDaniadoFilaEntity
    {
        // Agrupación
        public int Region { get; set; }
        public string Localizacion { get; set; }

        // Identificación
        public string CodTienda { get; set; }
        public string NombreTienda { get; set; }

        // Comunes
        public decimal Venta { get; set; }
        public decimal TotalDaniado { get; set; }
        public decimal PorcentajeDaniado { get; set; }

        // SOLO BODEGA CENTRAL
        public decimal DanadoSalidaCD { get; set; }
        public decimal DanadoNotaCredito { get; set; }
        public decimal DanadoRecuperado { get; set; }

        public decimal PorcentajeNC { get; set; }
        public decimal PorcentajeAlmacen { get; set; }

        // Control
        public bool EsBodegaCentral { get; set; }
    }
}
