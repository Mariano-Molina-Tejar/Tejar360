using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class GestionEmpleadosEntity
    {

    }

    public class DatosEmpleados
    {
        public int EmpID { get; set; }
        public string Nombre { get; set; }
        public string Puesto { get; set; }
        public int PuestoId { get; set; }
        public string Departamento { get; set; }
        public string RequisicionUniforme { get; set; }
        public string Estado { get; set; }
        public decimal Viaticos { get; set; }
        public string Activo { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Sexo { get; set; }
        public DateTime CreadoSAP { get; set; }
        public bool Perfil { get; set; }
    }

    public class Puestos
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
    
    public class CausasDespido
    {
        public int Code { get; set; }
        public string Name { get; set; }
    }

    public class AutorizacionesGTHViewModel
    {
        public IEnumerable<AutorizacionDeBaja> Autorizaciones { get; set; }
        public IEnumerable<AutorizacionDeBaja> Procesos { get; set; }
        public IEnumerable<CausasDespido> Causas { get; set; }
    }
    public class EnvioCorreoGestionEmpleados
    {
        public string Asunto { get; set; }
        public string Correos { get; set; }
        public string Cuerpo { get; set; }  
        public string Nombre { get; set; }
        public int Estado { get; set; }
        public bool isHTML { get; set; } = false;
    }

    public class SolicitudDeBaja
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public int U_EmpleadoId { get; set; }
        public int U_SolicitanteId { get; set; }
        public string U_Observaciones { get; set; }
        public string U_AutorizadoGTH { get; set; }
        public int U_Estado { get; set; }
        public int U_Motivo { get; set; }
        public DateTime U_FechaSolicitud { get; set; }
    }

    public class Causa
    {
        public int U_IdSolicitud { get; set; }
        public int U_IdCausa { get; set; }
    }

    public class AutorizacionDeBaja
    {
        public int Code { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public int EmpleadoId { get; set; }
        public string Nombre { get; set; }
        public string Puesto { get; set; }
        public int PuestoId { get; set; }
        public string Departamento { get; set; }
        public int DepartamentoId { get; set; }
        public string Solicita { get; set; }
        public int SolicitaId { get; set; }
        public string Motivo { get; set; }
        public string Observaciones { get; set; }
        public int Causas { get; set; }
        public char AutorizadoGTH { get; set; }
        public int Estado { get; set; }
    }

    public class Autorizaciones
    {
        public DateTime FechaDeSolicitud { get; set; }
        public string Posicion { get; set; }
        public char Estado { get; set; }
    }
    public class TrackingDeBaja
    {
        public bool Pendiente { get; set; }
        public bool AutorizadoGTH { get; set; }
        public bool Carta { get; set; }
        public bool Solvencia { get; set; }
        public bool UsuariosOperativos { get; set; }
        public bool ConfirmacionPago { get; set; }
        public bool CargaLiquidacion { get; set; }
        public bool UsuariosContables { get; set; }
    }


}
