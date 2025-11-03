using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class ReporteInterTiendasEntity
    {
        public string NoOrdenVenta { get; set; }
        public DateTime FechaOV { get; set; }
        public DateTime FechaEntrega { get; set; }
        public string NoOrdenCompra { get; set; }
        public DateTime FechaOC { get; set; }
        public string NoEntradaMercancia { get; set; }
        public DateTime FechaRecibidoBodega { get; set; }
        public DateTime FechaCarga { get; set; }
        public int DiasTranscurridos { get; set; }
        public string EstadoEntrega { get; set; }
        public int Ruta { get; set; }

        public string BodegaOrigen { get; set; }
        public string RutaCompartida { get; set; }
        public string Destino  { get; set; }
        public DateTime FechaDescarga { get; set; }
        public string Camion  { get; set; }
    }

}
