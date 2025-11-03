using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class AsistenciaModel
    {
        public string Sucursal { get; set; }          // Ej: "Sucursal Quiché"
        public string Fecha { get; set; }           // Ej: 2025-10-06
        public int Semana { get; set; }           // Ej: 2025-10-06
        public string Dia { get; set; }               // Ej: "lunes"
        public string Empleado { get; set; }          // Ej: "Sheila Maite Beten Galindo"
        public string HoraEntrada { get; set; }       // Ej: "09:00:58 AM"
        public string HoraSalida { get; set; }        // Ej: "06:59:02 PM"
        public string ErrorMessage { get; set; }
    }

    public class AsistenciaSemanalViewModel
    {
        public string Empleado { get; set; }

        // Lunes
        public string LunesSucursal { get; set; }
        public string LunesEntrada { get; set; }
        public string LunesSalida { get; set; }

        // Martes
        public string MartesSucursal { get; set; }
        public string MartesEntrada { get; set; }
        public string MartesSalida { get; set; }

        // Miércoles
        public string MiercolesSucursal { get; set; }
        public string MiercolesEntrada { get; set; }
        public string MiercolesSalida { get; set; }

        // Jueves
        public string JuevesSucursal { get; set; }
        public string JuevesEntrada { get; set; }
        public string JuevesSalida { get; set; }

        // Viernes
        public string ViernesSucursal { get; set; }
        public string ViernesEntrada { get; set; }
        public string ViernesSalida { get; set; }
            
        // Sábado
        public string SabadoSucursal { get; set; }
        public string SabadoEntrada { get; set; }
        public string SabadoSalida { get; set; }
        
        //Mensaje de error
        public string ErrorMessage { get; set; }

    }

}
