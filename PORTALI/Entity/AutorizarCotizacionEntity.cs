using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class AutorizarCotizacionEntity
    {
        public int UserId1 { get; set; }
        public string Usuario1 { get; set; }
        public string Email1 { get; set; }
        public int UserId2 { get; set; }
        public string Usuario2 { get; set; }
        public string Email2 { get; set; }
        public int IdEstado { get; set; }
        public string EstadoAuto { get; set; }
        public int Code { get; set; }
        public int LineId { get; set; }
    }
}
