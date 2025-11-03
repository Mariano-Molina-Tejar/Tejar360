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
        public string Departamento { get; set; }
        public string RequisicionUniforme { get; set; }
        public string Estado { get; set; }
        public decimal Viaticos { get; set; }
        public string Activo { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Sexo { get; set; }
        public DateTime CreadoSAP { get; set; }
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

    public class EnvioCorreoGestionEmpleados
    {
        public string Asunto { get; set; }
        public string Correos { get; set; }
        public string Cuerpo { get; set; }  
        public string Nombre { get; set; }
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
}
