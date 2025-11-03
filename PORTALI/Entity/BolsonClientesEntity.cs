using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    class BolsonClientesEntity
    {
    }

    public class ReporteVentas
    {
        public string SlpName { get; set; }       // Vendedor
        public string Nit { get; set; }           // Número de identificación
        public string Nombre { get; set; }        // Nombre del cliente
        public string Telefono { get; set; }      // Teléfono

        // Cotizaciones
        public int? Cotizaciones { get; set; }
        public decimal? TotalCotizacion { get; set; }

        // Mes actual
        public int? TicketsMes { get; set; }
        public decimal? TotalMes { get; set; }
        public decimal? MargenMes { get; set; }

        // Trimestre actual
        public int? TicketsTrimestre { get; set; }
        public decimal? TotalTrimestre { get; set; }
        public decimal? MargenTrimestre { get; set; }

        // Año actual
        public int? TicketsAnio { get; set; }
        public decimal? TotalAnio { get; set; }
        public decimal? MargenAnio { get; set; }

        // Erro
        public string ErrorMessage { get; set; }
    }

    public class BolsonClientesInfo
    {
        public int BolsonClientes { get; set; }
        public int ClientesNuevos { get; set; }
        public int ClientesActivos { get; set; }
        public int ClientesInactivos { get; set; }

        // Usamos double o decimal para porcentaje
        public double ClientesSinContactoPorcentaje { get; set; }

        public string ErrorMessage { get; set; }

    }


}
