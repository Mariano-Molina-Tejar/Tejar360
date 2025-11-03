using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class BolsonPerdidoEntity
    {
        public string TipoTabla { get; set; }
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public int SlpCode { get; set; }
        public string SlpName { get; set; }
        public string Nit { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public DateTime DocDate { get; set; }
        public double DocTotal { get; set; }
        public string Estado { get; set; }
        public int IdMotivo { get; set; }
        public string MotivoPerdida { get; set; }
        public DateTime? FechaPerdida { get; set; }   // Puede ser null
        public string NotaPerdida { get; set; }
        public string WhsCode { get; set; }
        public string WhsName { get; set; }
        public int IdRegion { get; set; }
        public string Region { get; set; }
        public int Tipo { get; set; }
        public string ReferidoSac { get; set; }
    }
}
