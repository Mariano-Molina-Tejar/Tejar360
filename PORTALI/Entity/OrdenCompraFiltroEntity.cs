using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class OrdenCompraFiltroEntity
    {
        public int DocNum { get; set; }           // Número del documento
        public string CardCode { get; set; }      // Código del socio
        public string CardName { get; set; }      // Nombre del socio
        public DateTime DocDate { get; set; }     // Fecha del documento
        public double DocTotal { get; set; }     // Total del documento
        public string Estado { get; set; }        // Estado calculado (Cerrado, Abierto, etc.)
    }
}
