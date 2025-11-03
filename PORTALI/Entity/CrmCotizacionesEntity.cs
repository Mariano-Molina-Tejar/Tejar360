using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class CrmCotizacionesEntity
    {
        public string WhsCode { get; set; }
        public string WhsName { get; set; }
        public string Region { get; set; }
        public int TotalCoti { get; set; }
        public double LineTotalCoti { get; set; }
        public double LineTotalCobiAb { get; set; }
        public int TotalFac { get; set; }
        public double LineTotalFac { get; set; }
        public double TasaCierre { get; set; }
        public int MetaCoti { get; set; }
        public double TasaGeneracion { get; set; }
        public int ClientesNuevos { get; set; }
        public int TotalClie { get; set; }
        public int totalClieRetenido { get; set; }
        public double TasaRetencion { get; set; }
        public int Cerradas { get; set; }
        public int Abiertos { get; set; }
        public int Perdida { get; set; }
        public int Seguimiento { get; set; }
        public int SinEstatus { get; set; }
    }
}
