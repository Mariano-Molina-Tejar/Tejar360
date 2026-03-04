using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Solvencias
    {

    }

    public class EmpleadosSolvencia
    {
        public int EmpId { get; set; }
        public string Nombre { get; set; }
        public string Departamento { get; set; }
        public string Puesto { get; set; }
        public string PuestoId { get; set; }
        public bool EmisioDeSolvencia { get; set; }
    }

    public class SolvenciaIT
    {
        public string Code { get; set; }
        public string U_EquipoDeComputo { get; set; }
        public string U_TelefonoYAccesorios { get; set; }
        public DateTime? U_FechaSolvenciaIT { get; set; }
        public string U_HoraSolvenciaIT { get; set; }
        public string U_ObeservacionesIT { get; set; }
    }
    public class SolvenciaContabilidad
    {
        public string Code { get; set; }
        public decimal U_CuentaEspecial { get; set; }
        public decimal U_LiquidacionDeViaticos { get; set; }
        public DateTime? U_FechaSolvenciaFinanzas { get; set; }
        public string U_HoraSolvenciaFinanzas { get; set; }
        public string U_ObeservacionesFinanzas { get; set; }
    }

    public class SolvenciaAditoria
    {
        public string Code { get; set; }
        public decimal U_CompraEmpleados { get; set; }
        public decimal U_FaltantesBodega { get; set; }
        public DateTime? U_FechaSolvenciaAuditoria { get; set; }
        public string U_HoraSolvenciaAuditoria { get; set; }
        public string U_ObservacionesAuditoria { get; set; }
    }
    public class SolvenciaNomina
    {
        public string Code { get; set; }
        public decimal U_AnticiposSalarios { get; set; }
        public decimal U_PrestamoBancario { get; set; }
        public decimal U_Embargos { get; set; }
        public decimal U_DescuentoUniforme { get; set; }
        public decimal U_DevolucionISR { get; set; }
        public string U_Uniformes { get; set; }
        public DateTime? U_FechaSolvenciaNomina { get; set; }
        public string U_HoraSolvenciaNomina { get; set; }
        public string U_NotificacionAreasInteresadas { get; set; }
        public string U_ObservacionesNomina { get; set; }
    }
}
