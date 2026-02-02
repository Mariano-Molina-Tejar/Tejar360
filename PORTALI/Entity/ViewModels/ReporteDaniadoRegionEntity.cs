using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.ViewModels
{
    public class ReporteDaniadoRegionEntity
    {
        public int Region { get; set; }
        public string NombreRegion  { get; set; }

        public List<ReporteDaniadoFilaEntity> Filas { get; set; } = new List<ReporteDaniadoFilaEntity>();

        public ReporteDaniadoTotalesEntity Totales { get; set; } = new ReporteDaniadoTotalesEntity();
    }
}
