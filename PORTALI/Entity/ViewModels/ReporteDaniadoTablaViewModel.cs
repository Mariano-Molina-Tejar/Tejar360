using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.ViewModels
{
    public class ReporteDaniadoTablaViewModel
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public List<ReporteDaniadoRegionEntity> Regiones { get; set; }
            = new List<ReporteDaniadoRegionEntity>();

        public ReporteDaniadoTotalesEntity TotalesGlobales { get; set; }
            = new ReporteDaniadoTotalesEntity();
    }
}
