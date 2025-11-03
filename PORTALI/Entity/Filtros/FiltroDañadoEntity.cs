using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Filtros
{
    public class FiltroDañadoEntity : FechaFiltroEntity
    {
        public string AlmacenSeleccionado { get; set; }
        public string TranportistaSeleccionado { get; set; }

        public DateTime FechaInicioGraficas { get; set; }
        public DateTime FechaFinGraficas { get; set; }

        public new void Validar()
        {
            base.Validar(); //Mantiene los filtros originales

            //Agregamos el rango extendido para graficas
            var hoy = DateTime.Today;

            FechaInicioGraficas = new DateTime(hoy.Year, 1, 1);//Desde enero
            FechaFinGraficas = new DateTime(hoy.Year, hoy.Month, 1).AddMonths(1).AddDays(-1); //Hata fin de mes actual
        }
    }
}
