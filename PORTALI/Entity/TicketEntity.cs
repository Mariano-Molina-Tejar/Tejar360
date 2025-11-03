using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class TicketEntity
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Asignado { get; set; }
        public string Creador { get; set; }
        public string Prioridad { get; set; }
        public string Categoria { get; set; }
        public string Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaCierre { get; set; }
        public string PrimeraRespuestaMinutos { get; set; }
        public string TiempoCierreMinutos { get; set; }
    }

    public class TicketAnualEntity
    {
        public int Anio { get; set; }
        public int Mes { get; set; }
        public int TotalTickets { get; set; }
        public double PromedioMinutos { get; set; }
        public string PromedioFormateado { get; set; }
    }
}
