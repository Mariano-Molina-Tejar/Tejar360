using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class AutorizaCotizacionEntity
    {
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public int TipoAutoriza { get; set; }
        public string Notas { get; set; }
        public string Usuario { get; set; }
    }
}
