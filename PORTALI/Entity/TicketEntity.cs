using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class TicketEntity
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Asignado { get; set; }
        public string Creador { get; set; }
        public string Prioridad { get; set; }
        public string Categoria { get; set; }
        public string Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaCierre { get; set; }
        public string PrimeraRespuestaMinutos { get; set; }
        public string TiempoCierreMinutos { get; set; }
    }

    public class TicketAnualEntity
    {
        public int Anio { get; set; }
        public int Mes { get; set; }
        public int TotalTickets { get; set; }
        public double PromedioMinutos { get; set; }
        public string PromedioFormateado { get; set; }
    }

    public class TicketExport
    {
        public List<User> Users { get; set; }
        public List<CustomAttribute> Custom_Attributes { get; set; }
        public List<Ticket> Tickets { get; set; }
        public List<TicketUpdate> Ticket_Updates { get; set; }
    }

    public class User
    {
        public string Email { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Role { get; set; }
        public decimal Hourly_Rate { get; set; }
        public long Import_Id { get; set; }
    }

    public class CustomAttribute
    {
        public int Organization_Id { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public string Attr_Type { get; set; }
        public long Creator_Id { get; set; }
        public string Created_At { get; set; }
        public string Updated_At { get; set; }
        public long Import_Id { get; set; }
        public string Type { get; set; }
        public bool Included_In_Portal { get; set; }
        public bool Required_In_Portal { get; set; }
        public string Col_Type { get; set; }
        public bool Custom { get; set; }
        public string Label { get; set; }
        public string Model { get; set; }
        public List<string> Options { get; set; }
        public string Sql_Type { get; set; }
    }

    public class Ticket
    {
        public int Ticket_Number { get; set; }
        public int Organization_Id { get; set; }
        public string Receipt_Type { get; set; }
        public long Creator_Id { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }      // Puede venir "1", "2", etc.
        public string Status { get; set; }
        public string Due_At { get; set; }
        public long Import_Id { get; set; }
        public string Created_At { get; set; }
        public string Updated_At { get; set; }
        public string end_user_id { get; set; }
        public long first_response_secs { get; set; }
        public long close_time_secs { get; set; }

        // Algunas entradas de "tickets" también traen "category" y campos extras.
        public string Category { get; set; }

        public string assigned_to { get; set; }
        public string created_by { get; set; }
        public string Organization { get; set; }
        public string Created { get; set; }
    }

    public class TicketUpdate
    {
        public string Body { get; set; }
        public bool Is_Public { get; set; }
        public int Organization_Id { get; set; }
        public long Creator_Id { get; set; }
        public string Action { get; set; }
        public long Ticket_Id { get; set; }
        public string Created_At { get; set; }
        public string Updated_At { get; set; }
        public TicketUpdateDetails Details { get; set; }
        public long Import_Id { get; set; }
        public bool Ticket_Muted { get; set; }
        public long Created_By { get; set; }
    }

    public class TicketUpdateDetails
    {
        public List<long> Ticket_Category_Id { get; set; }  // A veces viene [null, valor]
        public Dictionary<string, List<object>> Attrs { get; set; }
        public List<object> Status { get; set; }
    }
}