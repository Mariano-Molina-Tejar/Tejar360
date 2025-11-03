using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class ListadoVisitasContactosEntity
    {
        public int Code { get; set; }               // Código del registro
        public DateTime Name { get; set; }          // Fecha o nombre del registro
        public int IdVisita { get; set; }           // Id de la visita a la que pertenece
        public string NombreContacto { get; set; }  // Nombre del contacto
        public string Telefono { get; set; }        // Teléfono
        public int IdTipoCli { get; set; }          // Tipo de cliente (id)
        public string TipoCliente { get; set; }     // Tipo de cliente (texto)
        public string CompradorPrincipal { get; set; } // "Y" o "N"
        public string Estado { get; set; }          // Estado del contacto
    }
}
