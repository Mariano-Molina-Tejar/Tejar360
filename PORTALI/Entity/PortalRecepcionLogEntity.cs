using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class PortalRecepcionLogEntity
    {
        public int Code { get; set; }
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public DateTime Creado { get; set; }
        public string Fecha { get; set; }
        public string Usuario { get; set; }
        public string Notas { get; set; }
        public string IdEstatus { get; set; }
        public string Estatus { get; set; }
        public string Letra { get; set; }
        public string Texto { get; set; }
    }
}
