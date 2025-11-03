using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class ReporteCheckListEntity
    {
        public string Code { get; set; }
        public DateTime U_FechaCreado { get; set; }
        public DateTime U_FechaFinalizacion { get; set; }
        public string U_DG_CodTienda { get; set; }
        public string Tienda { get; set; }
        public int CodRegion { get; set; }
        public string Region { get; set; }
        public int U_DG_CodArea { get; set; }
        public string U_IdFormulario { get; set; }
        public decimal U_PuntajeAlmacenaje { get; set; }
        public decimal U_PuntajeRecepcion { get; set; }
        public decimal U_PuntajeDespacho { get; set; }
        public decimal U_PuntajeDaniado { get; set; }
        public decimal U_Puntaje5S { get; set; }
        //public decimal U_CalificacionTotal { get; set; }
        public decimal CalificacionTotal { get; set; }

        // 🔹 Campo calculado de promedio general (ignora ceros)
        public decimal CumplimientoGeneral
        {
            get
            {
                var valores = new List<decimal>
                {
                    U_PuntajeAlmacenaje,
                    U_PuntajeRecepcion,
                    U_PuntajeDespacho,
                    U_PuntajeDaniado,
                    U_Puntaje5S
                };

                var valoresValidos = valores.Where(v => v > 0).ToList();

                if (!valoresValidos.Any())
                    return 0;

                return valoresValidos.Average();
            }
        }

        // 🔹 Línea de referencia fija para gráfica
        public const int Objetivo = 100;
    }
}
