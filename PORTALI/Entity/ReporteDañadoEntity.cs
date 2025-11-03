using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class ReporteDañadoEntity
    {
        // Fechas y documentos
        public DateTime Fecha { get; set; }
        public string Mes { get; set; }
        public int MesNumero { get; set; }

        public string Anio { get; set; }
        public string FechaNC { get; set; }

        // Información de venta
        //public string NoRutaOV { get; set; }
        public string RutaOV { get; set; }
        public string Tienda { get; set; }
        public string CodigoArticulo { get; set; }
        public string Articulo { get; set; }
        public int CantidadItemOV { get; set; }
        public double PrecioUnitario { get; set; }
        public decimal MontoOV { get; set; }

        // Nota de crédito
        public string NotaCredito { get; set; }
        public decimal MontoNC { get; set; }

        // Clasificación
        public string Categoria { get; set; }
        public string Subcategoria { get; set; }
        public string Motivo { get; set; }

        // Logística
        public string WhsName { get; set; }
        public string Ruta { get; set; }
        public string CodigoTransportista { get; set; }
        public string Transportista { get; set; }
        public string OrdenVenta { get; set; }
        public string OrdenServicio { get; set; }

        // Control de errores
        public string ErrorMessage { get; set; }


    }
}
