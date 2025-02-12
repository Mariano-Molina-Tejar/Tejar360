using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class SessionLoginEntity
    {
        public int UserId { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int SlpCode { get; set; }
        public string SlpName { get; set; }
        public string UrlPic { get; set; }
        public int Nivel { get; set; }
        public string WhsCode { get; set; }
        public int Region { get; set; }
        public int Depto { get; set; }
        public string DeptoName { get; set; }
        public int CodeEmpleado { get; set; }
        public double Descto { get; set; }
    }
}
