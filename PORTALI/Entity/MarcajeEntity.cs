using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class MarcajeEntity
    {
        public int IdArea { get; set; }
        public string Area { get; set; }
        public int CodDepartamento { get; set; }
        public string DepartamentoTienda { get; set; }
        public int Incumplimientos { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class MarcajeFiltrosEntiry
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string CodigoDepartamento { get; set; }
        public int CodigoEmpleado { get; set; }
        public int area { get; set; }
        public string areaDescripcion { get; set; }
        public int estado { get; set; }
    }

    public class DetalleMarcaje
    {
        public int Rw { get; set; }
        public DateTime Fecha { get; set; }            // Fecha
        public string Dia { get; set; }                 // Dia
        public string U_CodEmpleado { get; set; }       // U_CodEmpleado
        public string Empleado { get; set; }            // Empleado
        public int IdArea { get; set; }                 // IdArea
        public string Area { get; set; }                // Area
        public string Posicion { get; set; }            // Posicion
        public string CodigoDepartamento { get; set; }  // CodigoDepartamento
        public string DepartamentoTienda { get; set; }  // Departamento/Tienda
        public string HoraEntrada { get; set; }      // HoraEntrada
        public string HoraSalida { get; set; }       // HoraSalida
        public int U_IdRol { get; set; }             // U_IdRol
        public string HoraEntradaROL { get; set; }   // HoraEntradaROL
        public string HoraSalidaROL { get; set; }    // HoraSalidaROL
        public string Incumplimientos { get; set; }    // HoraSalidaROL
        public string Incumplimiento { get; set; }    // HoraSalidaROL

        public string Inc { get; set; }                 // Inc
        public string Name { get; set; }
        public string ErrorMessage { get; set; }
        public int TotalHorasLaboradas { get; set; }
        public int TotalHorasLaborales { get; set; }
    }

    public class DetalleVacaciones
    {
        public int RW { get; set; }
        public DateTime Fecha { get; set; }
        public int U_CodEmpleado { get; set; }
        public string Empleado { get; set; }
        public int IdArea { get; set; }
        public string Area { get; set; }
        public string Posicion { get; set; }
        public string CodigoDepartamento { get; set; }
        public string Departamento_Tienda { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int DiasAccion { get; set; }
        public string AutorizadoPor { get; set; }
        public string Observaciones { get; set; }
        public string Autorizado { get; set; }
        public string ErrorMessage { get; set; }        
    }

    public class FiltroVacaciones
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int? Area { get; set; }
        public string Departamento { get; set; }
        public string CodEmpleado { get; set; }
        public int CodDepartamento { get; set; }
        public int TipoPermiso { get; set; }
    }

    public class AreaEntity
    {
        public int Codigo { get; set; }
        public string Descripcion { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class DepartamentosEntity
    {
        public int CodigoArea { get; set; }
        public string Area { get; set; }
        public int CodigoDepartamento { get; set; }
        public string Departamento { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class EmpleadoEntity
    {
        public int CodigoEmpleado { get; set; }
        public string Nombre { get; set; }
        public string Departamento { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class PermisosEntity
    {
        public int CodigoEmpleado { get; set; }
        public int IdFirmante { get; set; }
        public int Existe { get; set; }
        public List<detalle> Detalle { get; set; }
    }

    public class detalle
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string HoraIncio { get; set; }
        public string HoraFin { get; set; }
        public int Tipo { get; set; }
        public string Observaciones { get; set; }
        public string Solicita { get; set; }
        public int Id { get; set; }
        public int LineId { get; set; }
        public int rrhh { get; set; }
    }
    public class detallePermiso
    {
        public int CodigoEmpleado { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string HoraIncio { get; set; }
        public string HoraFin { get; set; }
        public int Tipo { get; set; }
        public string Observaciones { get; set; }
        public string Solicita { get; set; }
        public int LineId { get; set; }
        public int rrhh { get; set; }
    }

    public class GetPermisoEntity
    {
        public int Rw { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string TipoPermiso { get; set; }
        public string U_Observacion { get; set; }
        public int CodigoEmpleado { get; set; }
        public string Nombre { get; set; }
        public string Puesto { get; set; }
        public string Departamento { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFin { get; set; }
        public DateTime FechaPresentarse { get; set; }
        public int DiasGozados { get; set; }
        public int U_Estado { get; set; }
        public int LineId { get; set; }
        public string ErrorMessage { get; set; }
        public int Autorizar { get; set; }
        public int rrhh { get; set; }
        public int EstadoRH { get; set; }

    }

    public class TiposPermisosEntity
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class FirmaDigitalEntity
    {
        public DateTime FechaFirma { get; set; }
        public string Firmante { get; set; }
        public string RolFirmante { get; set; }
        public string FirmaDigital { get; set; } // Firma codificada en base64
        public string Comentario { get; set; }
        public string ErrorMessage { get; set; }

    }


}
