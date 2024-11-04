using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class RecepcionProductosModalDetalleEntity
    {
        public int DocEntryOc { get; set; }
        public int DocNumOc { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string Comments { get; set; }
        public string IdEstado { get; set; }
        public DateTime DocDateCoti { get; set; }
        public DateTime DocDateOc { get; set; }
        public DateTime DocDateReq { get; set; }
        public string  Tienda { get; set; }
        public string  Sucursal { get; set; }
        public string  Depto { get; set; }
        public double SubTotal { get; set; }
        public double Impuesto { get; set; }
        public double DocTotal { get; set; }
        public List<RecepcionProductosDetalleEntity> Detalle { get; set; }
        public List<PortalRecepcionEstadosEntity> ListaEstados { get; set; }
        public List<PortalRecepcionLogEntity> logRecepcion { get; set; }
    }
}
