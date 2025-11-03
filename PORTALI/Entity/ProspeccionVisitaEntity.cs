using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class ProspeccionDataEntity
    {
        public ProspeccionVisitaEntity Encabezado { get; set; }
        public List<VisitasContactosEntity> Contactos { get; set; }
        public List<VisitasFotografiasEntity> Fotografias { get; set; }
    }
    public class ProspeccionVisitaEntity
    {
        public int Code { get; set; } 
        public string Name { get; set; }

        public string U_IdProgra { get; set; }
        public string U_IdPuntoPartida { get; set; }
        public DateTime? U_Fecha { get; set; }

        public string U_IdEtapa { get; set; }
        public string U_IdNegociacion { get; set; }
        public string U_NombreCliente { get; set; }
        public string U_Direccion { get; set; }
        public string U_IdTipoConstruccion { get; set; }
        public string U_NombreObra { get; set; }
        public string U_Comentarios { get; set; }

        public DateTime? U_ProximaVisita { get; set; }
        public string U_Ubicacion { get; set; }
        public string U_Estado { get; set; }
        public int U_Usuario { get; set; }
    }

    public class VisitasContactosEntity
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public string U_IdVisita { get; set; }
        public string U_Nombres { get; set; }
        public string U_Telefono { get; set; }
        public string U_IdTipo { get; set; }
        public string U_CompradorPrincipal { get; set; }
        public string U_Estado { get; set; }
    }

    public class VisitasFotografiasEntity 
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public string U_IdVisita { get; set; }
        public string U_Url { get; set; }
        public string U_Notas { get; set; }
        public string U_Estado { get; set; }
    }
}
