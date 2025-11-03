using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class ListadoAgendaEntity
    {
        public int Id { get; set; }
        public int IdRegion { get; set; }
        public string Region { get; set; }
        public int SlpCode { get; set; }
        public string SlpName { get; set; }
        public string WhsCode { get; set; }
        public string WhsName { get; set; }
        public int IdDepto { get; set; }
        public int IdMunicipio { get; set; }
        public int? IdColonia { get; set; }   // NULL en tus datos
        public int IdZona { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFinal { get; set; }
        public string AreaAsignada { get; set; }
        public string Notas { get; set; }
        public string Estado { get; set; }
        public bool EstadoPlay { get; set; }
        public bool Enableds { get; set; }
        public int IdPunto { get; set; }
    }
}
