using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class PlanificacionDetalleModel
    {
        public int Code { get; set; }               // ID o código único
        public DateTime Name { get; set; }            // Nombre o descripción
        public DateTime U_FechaHora { get; set; }   // Fecha y hora del registro
        public int U_Planificacion { get; set; } // Fecha de planificación
        public string U_Longitud { get; set; }         // Longitud (entero si es código de planificación)
        public string U_Latitud { get; set; }       // Latitud geográfica
        public string U_Lugar { get; set; }         // Longitud geográfica
        public string U_LugarFinal { get; set; }
        public string U_Estado { get; set; }        // Estado (Y/N)
        public DateTime U_FechaInicio { get; set; } // Fecha de inicio
        public DateTime? U_FechaFinal { get; set; } // Fecha de fin (nullable)
        public int Visitas { get; set; }
    }

}
