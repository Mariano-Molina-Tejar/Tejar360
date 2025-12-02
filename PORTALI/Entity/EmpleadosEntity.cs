using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Entity
{
    public class EmpleadosEntity
    {
        public int EmpId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Puesto { get; set; } = string.Empty;
        public int PuestoId { get; set; }
        public string Departamento { get; set; } = string.Empty;
        public string Activo { get; set; }
        public string Sexo { get; set; } = string.Empty;
        public string CreadoSAP { get; set; } = string.Empty;
        public string JefeInmediato { get; set; } = string.Empty;
        public int empIdJefe { get; set; }
        public bool Perfil { get; set; }
    }

    public class Posicion
    {
        public int NoPosicion { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Perfil { get; set; }
    }

    public class EmpleadosViewModel
    {
        public IEnumerable<EmpleadosEntity> Empleados { get; set; }
        public IEnumerable<Posicion> Posicion { get; set; }
        public IEnumerable<Departamentos> Departamentos { get; set; }
        public IEnumerable<Tiendas> Tiendas { get; set; }
        public IEnumerable<dynamic> Areas { get; set; }
    }

    public class EmpleadoSL
    {
        [DefaultValue("")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string firstName { get; set; }

        [DefaultValue("")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string middleName { get; set; }

        [DefaultValue("")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string lastName { get; set; }

        [DefaultValue("")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string jobTitle { get; set; }

        [DefaultValue("")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string position { get; set; }

        [DefaultValue("")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string dept { get; set; }

        [DefaultValue("")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? empID { get; set; }

        [DefaultValue("")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Active { get; set; }

        [DefaultValue("")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string mobile { get; set; }

        [DefaultValue("")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string hometel { get; set; }

        [DefaultValue("")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string email { get; set; }

        [DefaultValue("")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string U_JefeInmediato { get; set; }

        [DefaultValue("")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string U_Tienda { get; set; }
    }

    public class EmpleadoActualizar
    {
        [DefaultValue("")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; }

        [DefaultValue("")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MiddleName { get; set; }

        [DefaultValue("")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }

        [DefaultValue("")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string JobTitle { get; set; }

        [DefaultValue("")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Position { get; set; }

        [DefaultValue("")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Department { get; set; }

        [DefaultValue("")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? empID { get; set; }

        [DefaultValue("")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Active { get; set; }

        [DefaultValue("")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MobilePhone { get; set; }

        [DefaultValue("")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string HomePhone { get; set; }

        [DefaultValue("")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string eMail { get; set; }

        [DefaultValue("")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string U_JefeInmediato { get; set; }

        [DefaultValue("")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string U_Tienda { get; set; }
    }

    public class Departamentos
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public string Remarks { get; set; }
        public int empID { get; set; }
    }

    public class Tiendas
    {
        public string WhsCode { get; set; }
        public string WhsName { get; set; }
    }
}