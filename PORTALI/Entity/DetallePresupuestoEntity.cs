using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class DetallePresupuestoEntity
    {
        public string AcctCode { get; set; }
        public string AcctName { get; set; }
        public double Presupuesto { get; set; }
        public double Gastado { get; set; }
        public double Saldo { get; set; }
        public double GastoActual { get; set; }
        public double SaldoActual { get; set; }
    }
}
