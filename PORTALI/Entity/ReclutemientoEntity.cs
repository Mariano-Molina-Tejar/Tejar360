using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class ReclutemientoEntity
    {
        public int SolicitudDeAlta { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public int PuestoId { get; set; }
        public string Puesto { get; set; }
        public string Departamento { get; set; }
        public string DepartamentoId { get; set; }
        public string Solicita { get; set;}
        public int SolicitaId { get; set;}
        public bool Perfil { get; set; }
    }

    public class ReclutamientoViewModel
    {
        public IEnumerable<ReclutemientoEntity> Reclutamiento { get; set; }
        public List<CVDOCREQ> Documentos { get; set; }
    }
    public class DetalleAspirantes
    {
        public int Rw { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string NombreAspirante { get; set; }
        public string Correo { get; set; }
        public string Estado { get; set; }
        public DateTime Created { get; set; }
    }

    public class DatosPersonales
    {
        public int Code { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public char Sexo { get; set; }
        public string NoDPI { get; set; }
        public string NIT { get; set; }
        public string AfilicionIGSS { get; set; }
        public string CorreoPersonal { get; set; }
        public string TelefonoFijo { get; set; }
        public string TelefonoPersonal { get; set; }
        public string profesion { get; set; }
    }

    public class DatosAcademicos
    {
        public string Nivel { get; set; }
        public string UltimoGradoAprobado { get; set; }
        public int Anio { get; set; }
        public string Establecimiento { get; set; }
        public string Carrera { get; set; }
        public string CursosAprobados { get; set; }
    }

    public class DatosLaborales
    {
        public int Code { get; set; }
        public string Empresa { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string TelefonoEmpresa { get; set; }
        public string CorreoEmpresa { get; set; }
        public string NombreJefe { get; set; }
        public string TelefonoJefe { get; set; }
        public string Puesto { get; set; }
        public string Actividades { get; set; }
    }

    public class DatosAspirantesViewModel
    {
        public DatosPersonales DatosPersonalesVM { get; set; }
        public List<DatosAcademicos> DatosAcademicosVM { get; set; }
        public List<DatosLaborales> DatosLaboralesVM { get; set; }
    }
    public class CVDOCREQ
    {
        public int DocEntry { get; set; }              // Identificador único (PK)
        public string Nombre { get; set; }             // Nombre del documento
        public string Descripcion { get; set; }        // Descripción del documento
        public DateTime FechaCreacion { get; set; }    // Fecha de creación
        public char Activo { get; set; }               // Y/N
        public char Obligatorio { get; set; }          // Y/N
        public string Extension { get; set; }          // Extensión esperada (pdf, jpg, etc.)
        public string ErrorMessage { get; set; }

    }
    public class ViewModelDocumento
    {
        public int TipoDocumento { get; set; }
        public string NombreDocumento { get; set; }
        public string Extencion { get; set; }
        public string Descripcion { get; set; }
    }

    public class Comentarios
    {
        public string Comentario { get; set; }
        public DateTime Fecha { get; set; }
        public string ComentarioPor { get; set; }
    }
    public class PerfilPuestoModel
    {
        public int Code { get; set; }
        public int U_IdPuesto { get; set; }
        public int U_ExperienciaMinima { get; set; }
        public int U_RangoEdadMin { get; set; }
        public int U_RangoEdadMax { get; set; }
        public string U_Sexo { get; set; } = string.Empty;
        public string U_NivelEstudio { get; set; } = string.Empty;
        public decimal U_SalarioMin { get; set; }
        public decimal U_SalarioMax { get; set; }
        public string U_Observaciones { get; set; } = string.Empty;
        public DateTime U_FechaCreacion { get; set; }
        public int U_UsuarioCreacion { get; set; }
    }
}
