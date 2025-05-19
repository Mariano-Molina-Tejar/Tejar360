using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class ListadoSeguimientoEntity
    {
        public string Creado { get; set; }
        public int Code { get; set; }
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public int IdEstado { get; set; }
        public string DescrEstado { get; set; }
        public DateTime FechaSeg { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public int IdTipoContacto { get; set; }
        public string TipoDeContacto { get; set; }
        public string Accion { get; set; }
        public string Notas { get; set; }
        public string Usuario { get; set; }
    }
}
