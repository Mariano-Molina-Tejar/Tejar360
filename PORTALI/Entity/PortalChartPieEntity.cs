using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class PortalChartPieEntity
    {
        public List<string> labels { get; set; }    
        public List<DtSets> datasets { get; set; }        
    }

    public class DtSets 
    {
        public List<string> backgroundColor { get; set; }
        public List<double> data { get; set; }
    }
}
