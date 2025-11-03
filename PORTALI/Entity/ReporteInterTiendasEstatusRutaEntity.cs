using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class ReporteInterTiendasEstatusRutaEntity
    {
        public int NoRuta { get; set; }
        public int idEstatus { get; set; }
        public string EstadoRuta { get; set; }
        public DateTime FechaInicio { get; set; }
        public string HoraInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string HoraFin { get; set; }
    }
}
