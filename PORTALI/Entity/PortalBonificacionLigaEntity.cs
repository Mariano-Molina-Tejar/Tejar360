using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entity
{
    public class PortalBonificacionLigaEntity
    {
        public int? SlpCode { get; set; }
        public double? VentaTotal { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El rengo de debe ser menor a cero")]
        public double? MetaMensual { get; set; }
        public double? ComisionVenta { get; set; }
        public int? IdEscala { get; set; }
        public string Escala { get; set; }
        public double? MetaLiga { get; set; }
        public double? BonoLiga { get; set; }
        public string Color { get; set; }
        public string Imagen { get; set; }
        public double? IndiceVenta { get; set; }
        public string Flag { get; set; } 
        public double AplicarBono { get; set; }        
        public List<PortalBonificacionDetalleEntity> DetallePisoLozaCCP { get; set; }
        public PortalMetaVentaTiendaEntity VentasTienda { get; set; }
    }

    public class PortalBonificacionDetalleEntity 
    {
        public int IdGrupo { get; set; }
        public double Meta { get; set; }
        public double Venta { get; set; }
        public double Indice { get; set; }
        public double Bono { get; set; }
        public double Comision { get; set; }
        public double TotalRecibir { get; set; }
    }
}
