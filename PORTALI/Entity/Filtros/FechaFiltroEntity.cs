using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlTypes;

namespace Entity
{
    public class FechaFiltroEntity
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public void Validar()
        {
            var hoy = DateTime.Today;

            if (FechaInicio == DateTime.MinValue || FechaInicio < (DateTime)SqlDateTime.MinValue)
                FechaInicio = new DateTime(hoy.Year, hoy.Month, 1);

            if (FechaFin == DateTime.MinValue || FechaFin < (DateTime)SqlDateTime.MinValue)
                FechaFin = new DateTime(hoy.Year, hoy.Month, 1).AddMonths(1).AddDays(-1);

        }
    }
}
