using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class CrmEstadoBolsonEntity
    {
        public string WhsCode { get; set; }           // Código de almacén
        public string WhsName { get; set; }           // Nombre del almacén
        public string Location { get; set; }          // Ubicación
        public double BolsonActivo { get; set; }      // Monto del bolsón activo
        public int LineasCot { get; set; }            // Cantidad de líneas cotizadas
        public double DocTotalCot { get; set; }       // Total de documentos cotizados
        public int LineasFac { get; set; }            // Cantidad de líneas facturadas
        public double DocTotalFac { get; set; }       // Total de documentos facturados
        public double TasaCierre { get; set; }        // Tasa de cierre
        public int LineasPerd { get; set; }           // Cantidad de líneas perdidas
        public double DocTotalPerd { get; set; }      // Total de documentos perdidos
        public double TasaPerd { get; set; }          // Tasa de pérdida
        public double TasaGeneracion { get; set; }
    }

    public class CrmEstadoBolsonDetalleEntity
    {
        public int SlpCode { get; set; }           // Código de almacén
        public string SlpName { get; set; }           // Nombre del almacén        
        public double BolsonActivo { get; set; }      // Monto del bolsón activo
        public int LineasCot { get; set; }            // Cantidad de líneas cotizadas
        public double DocTotalCot { get; set; }       // Total de documentos cotizados
        public int LineasFac { get; set; }            // Cantidad de líneas facturadas
        public double DocTotalFac { get; set; }       // Total de documentos facturados
        public double TasaCierre { get; set; }        // Tasa de cierre
        public int LineasPerd { get; set; }           // Cantidad de líneas perdidas
        public double DocTotalPerd { get; set; }      // Total de documentos perdidos
        public double TasaPerd { get; set; }          // Tasa de pérdida
        public double TasaGeneracion { get; set; }
    }
}
