using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class CotizacionHeadEntity
    {
        public List<PortalTotalCotizacionesEntity> totalCotizaciones { get; set; }
        public CotizacionCompraEntity HeadDetail { get; set; }
        public int Depto { get; set; }
        public int DocEntrySol { get; set; }
        public DetallePresupuestoEntity DetallePresupuesto { get; set; }
    }
}
