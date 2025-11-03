using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{    
    public class ComercialTransaccionesEntity
    {
        public string WhsCode { get; set; }
        public string WhsName { get; set; }
        public double Meta { get; set; }
        public int MetaTrans { get; set; }
        public int CantidadTrans { get; set; }
        public int PromedioDiario { get; set; }
        public double Indice { get; set; }
        public double IndiceProyectado { get; set; }
    }
    public class VendedorMeta
    {
        public int Code { get; set; }           // Código del vendedor
        public string SlpName { get; set; }        // Nombre del vendedor
        public int MetaTickets { get; set; }       // Meta de tickets asignada
        public int CantidadTrans { get; set; }     // Cantidad de transacciones realizadas
    }


    public class TransaccionesEntity
    {
        public int Dia { get; set; }
        public string NombreDia { get; set; }
        public int D12AM { get; set; }
        public int D3PM { get; set; }
        public int Transacciones { get; set; }

    }

    public class Top5TiendasEntity
    {
        public string WhsCode { get; set; }
        public string WhsName { get; set; }
        public int Transacciones { get; set; }
    }
}
