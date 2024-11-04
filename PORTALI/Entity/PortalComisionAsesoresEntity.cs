using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class PortalComisionAsesoresEntity
    {
        public int SlpCode { get; set; }
        public string WhsCode { get; set; }
        public double MetaAsesor { get; set; }
        public double VentaAsesor { get; set; }
        public double VentaAsesorPro { get; set; }
        public double IndiceAsesor { get; set; }
        public double MetaTienda { get; set; }
        public double VentaTienda { get; set; }
        public double VentaTiendaPro { get; set; }
        public double IndiceTienda { get; set; }
        public double MetaBanio { get; set; }
        public double VentaBanio { get; set; }
        public double VentaBanioPro { get; set; }
        public double IndiceBanio { get; set; }
        public double MetaCcp { get; set; }
        public double VentaCcp { get; set; }
        public double VentaCcpPro { get; set; }
        public double IndiceCcp { get; set; }
        public double IndiceAsesorPro { get; set; }
        public double IndiceBanioPro { get; set; }
        public double IndiceCcpPro { get; set; }
        public double IndiceTiendaPro { get; set; }
        public double ComisionBase { get; set; }
        public double ComisionPlus { get; set; }
        public double BonoBase { get; set; }
        public double BonoPersonal { get; set; }
        public double BonoBanio { get; set; }
        public double BonoCcp { get; set; }
        public double BonoTienda { get; set; }
        public double TotalRecibir { get; set; }
        public List<ListaBonos> ListaBonos { get; set; }
        public List<PortalListadoAsesoresEntity> ListadoAsesores { get; set; }        
        public List<MesesEntity> ListaMeses{ get; set; }
        public int AnioActual { get; set; }
        public int MesActual { get; set; }
    }

    public class ListaBonos
    {
        public string Escala { get; set; }
        public double Meta { get; set; }
        public double Bono { get; set; }
        public string Color { get; set; }
        public string Icono { get; set; }
        public string Flag { get; set; }
        public string Oculto { get; set; }
        public double BonoAplica { get; set; }
    }
}
