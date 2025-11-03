using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    class PresupuestoEntity
    {

    }

    public class DetallePresupuestoN1
    {
        public int Mes { get; set; }

        public string Cuenta3 { get; set; }

        public decimal TotalEjecutado { get; set; } 

        public decimal TotalPresupuesto { get; set; }

        public decimal PorcentajeEjecutado { get; set; }

        public string Cuenta { get; set; }
        public string AcctCode { get; set; }

        public decimal TotalEjecutadoCt { get; set; }

        public decimal TotalPresupuestoCt { get; set; }
        public decimal DifenciaCt { get; set; }
        public decimal EjecutadoCt { get; set; }
        public int Code { get; set; }
        public string Nota { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class DetallePresupuestoN3
    {
        public int DocNum { get; set; }              // int
        public int DocNumOC { get; set; }            // int
        public int? DocEntry { get; set; }           // int (nullable porque 0 en "Nullable")
        public int DocEntryOC { get; set; }          // int
        public DateTime DocDate { get; set; }        // datetime
        public DateTime DocDateOC { get; set; }      // datetime
        public decimal MontoFactura { get; set; }    // numeric(38,9)
        public decimal MontoOC { get; set; }         // numeric(38,11)
        public string CardName { get; set; }         // nvarchar(100)
        public string AcctCode { get; set; }         // nvarchar(15)
        public string PrcName { get; set; }          // nvarchar(30)
        public string TipoDocumento { get; set; }    // varchar(19)
        public int IdDocumento { get; set; }    // varchar(19)
        public string ErrorMessage { get; set; }
    }

    public class NotasPto
    {
        public int U_Mes { get; set; }
        public int U_Anio { get; set; }
        public string U_Cuenta { get; set; }
        public string U_Nota { get; set; }
        public string Code { get; set; }

        public string ErrorMessage { get; set; }

    }
    public class DetalleDocumento
    {
        public int DocNum { get; set; }
        public DateTime FechaContabilizacion { get; set; }
        public DateTime FechaDocumento { get; set; }
        public string Proveedor { get; set; }
        public string Nombre { get; set; }
        public string Sucursal { get; set; }
        public string ItemCode { get; set; }
        public string Dscription { get; set; }
        public decimal Quantity { get; set; }           // numeric(19,6)
        public decimal LineTotal { get; set; }          // decimal(18,2)
        public decimal Total { get; set; }              // decimal(18,2)
        public decimal TotalCuenta { get; set; }              // decimal(18,2)
        public decimal TotalDocumento { get; set; }     // decimal(38,2)
        public string AcctCode { get; set; }
        public string Color { get; set; }
        public string Comentarios { get; set; }
        public string ErrorMessage { get; set; }
        // varchar(7) y puede ser NULL
    }

    public class AreasPresupuesto
    {
        public int? AcctCode { get; set; }
        public string AcctName { get; set; }
        public string ErrorMessage { get; set; }

    }
}
