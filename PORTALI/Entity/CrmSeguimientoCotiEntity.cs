using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class CrmSeguimientoCotiEntity
    {
        public DateTime FechaCreado { get; set; }
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public int MedioContacto { get; set; }
        public string NombreMedioContacto { get; set; }
        public string Notas { get; set; }
        public DateTime? PosFechaCompra { get; set; }
        public DateTime? FechaSeguimiento { get; set; }
        public string EstadoPerdida { get; set; }
        public int MotivoPerdida { get; set; }
        public int UserId { get; set; }
        public string Usuario { get; set; }
        public string Acciones { get; set; }
    }
}