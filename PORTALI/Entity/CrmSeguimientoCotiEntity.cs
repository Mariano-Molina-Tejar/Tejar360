using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class CrmSeguimientoCotiEntity
    {
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public DateTime FechaSeg { get; set; }
        public DateTime FechaVenc { get; set; }
        public int IdEstado { get; set; }
        public int IdTipoCont { get; set; }
        public string AccionRealizada { get; set; }
        public int UserId { get; set; }
        public string Notas { get; set; }
        public string Usuario { get; set; }
    }
}
