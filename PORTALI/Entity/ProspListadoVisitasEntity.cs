using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class ProspListadoVisitasEntity
    {
        public int Code { get; set; }
        public DateTime Fecha { get; set; }
        public string SlpName { get; set; }
        public string NombreCliente { get; set; }
        public string Direccion { get; set; }
        public int IdNegociacion { get; set; }
        public string Negociacion { get; set; }
        public int IdEtapa { get; set; }
        public string Etapa { get; set; }
        public int IdTipoConstruccion { get; set; }
        public string TipoConstruccion { get; set; }
        public string NombreObra { get; set; }
        public string Comentarios { get; set; }
        public DateTime ProximaVisita { get; set; }
        public string Ubicacion { get; set; }
        public string NoRegistro { get; set; }
        public string NombreContacto { get; set; }
        public string TelefonoContacto { get; set; }
        public string TipoContacto { get; set; }
    }
}
