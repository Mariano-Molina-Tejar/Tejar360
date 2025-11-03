using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.ViewModels
{
    public class DetalleRutaViewModel
    {
        public List<ReporteInterTiendasEstatusRutaEntity> Detalles { get; set; }
        public double PromedioDiasTranscurridos { get; set; }
        public int Ruta { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }

    }
}
