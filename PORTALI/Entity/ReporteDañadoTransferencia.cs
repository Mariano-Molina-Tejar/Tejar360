using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class ReporteDañadoTransferencia
    {// Documento y producto
        public DateTime Fecha { get; set; }
        public string Mes { get; set; }
        public int MesNumero { get; set; }

        public string DocNum { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        
        public string Categoria { get; set; }
        public string SubCategoria { get; set; }
        public string Linea { get; set; }

        // Cantidades y costos
        public int Cantidad { get; set; }
        public double PrecioUnitario { get; set; }
        public decimal Total { get; set; }

        // Origen/Destino
        public string Origen { get; set; }
        public string CodigoAlmacen { get; set; }
        public string AlmacenOrigen { get; set; }
        public string CodigoAlmacenD { get; set; }
        public string AlmacenDestino { get; set; }

        // Adicional
        public string Status { get; set; }
        public string Proveedor { get; set; }
        public string Comentario { get; set; }

        // Control de errores
        public string ErrorMessage { get; set; }
    }
}
