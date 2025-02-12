using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class RankingViewModel
    {
        public int Position { get; set; } // Posición en el ranking
        public string Name { get; set; } // Nombre del usuario
        public string PhotoUrl { get; set; } // URL de la foto
        public decimal Score { get; set; } // Puntaje
        public bool IsCurrentUser { get; set; } // Indica si es el usuario actual
    }
}
