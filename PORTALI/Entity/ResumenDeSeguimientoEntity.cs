using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class ResumenDeSeguimientoEntity
    {
        public int IdRegion { get; set; }
        public string Region { get; set; }
        public string WhsCode { get; set; }
        public string WhsName { get; set; }
        public int SlpCode { get; set; }
        public string SlpName { get; set; }
        public double TotalActivo { get; set; }
        public int CantidadActivo { get; set; }
        public double TotalSeguimiento { get; set; }
        public double TasaSeg { get; set; }
        public int CantidadSeg { get; set; }
        public double TotalFueraDeSeguimiento { get; set; }
        public double TasaFueraSeg { get; set; }
        public int CantidadFueraSeg { get; set; }
    }
}
