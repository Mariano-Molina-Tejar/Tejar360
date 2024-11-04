using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class PortalComisionAsesorEntity
    {
        public int SlpCode { get; set; }
        public List<PortalBonificacionLigaEntity> Bonos { get; set; }
    }
}
