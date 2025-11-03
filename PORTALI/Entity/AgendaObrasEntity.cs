using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class AgendaObrasEntity
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public string U_UserId { get; set; }
        public string U_WhsCode { get; set; }
        public string U_IdDepto { get; set; }
        public string U_IdMunicipio { get; set; }
        public string U_IdColonia { get; set; }
        public string U_IdZona { get; set; }
        public DateTime? U_FechaInicio { get; set; }
        public DateTime? U_FechaFinal { get; set; }
        public string U_Notas { get; set; }
        public string U_Estado { get; set; }
    }

    public class RecorridoEntity
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public DateTime U_FechaHora { get; set; }
        public int U_Planificacion { get; set; }
        public string U_Longitud { get; set; }
        public string U_Latitud { get; set; }
        public string U_Lugar { get; set; }
        public string U_LugarFinal { get; set; }
        public string U_Estado { get; set; }
        public string U_FechaInicio { get; set; }
        public string U_FechaFinal { get; set; }
        public string U_LongitudFinal { get; set; }
        public string U_LatitudFinal { get; set; }
    }
}
