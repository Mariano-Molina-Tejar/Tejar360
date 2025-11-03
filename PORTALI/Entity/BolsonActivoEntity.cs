using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class BolsonActivoEntity
    {
        public List<ListadoBolsonEntity> Listado { get; set; }
    }

    public class ListadoBolsonEntity
    {
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public int SlpCode { get; set; }
        public string SlpName { get; set; }
        public string Nit { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public DateTime? DocDate { get; set; }
        public decimal DocTotal { get; set; }
        public DateTime? PosibleFechaCom { get; set; }
        public string Estado { get; set; }
        public DateTime? FechaSeguimiento { get; set; }
        public string WhsCode { get; set; }
        public string WhsName { get; set; }
        public int IdRegion { get; set; }
        public string Region { get; set; }
        public double Activo { get; set; }
        public double Nuevos { get; set; }
        public double Facturado { get; set; }
        public double Perdida { get; set; }
        public int Tipo { get; set; }
    }
}
