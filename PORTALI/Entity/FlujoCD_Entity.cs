using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class FlujoCD_Entity
    {
        public string Almacen { get; set; }
        public string Origen { get; set; }
        public double CantidadO { get; set; }
        public string Destino { get; set; }
        public double CantidadD { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class AlmacenesFLujoCD
    {
        public string WhsCode { get; set; }
        public string WhsName { get; set; }
        public string MessageError { get; set; }
    }

    public class ViewModelFlujoCD
    {
        public List<FlujoCD_Entity> Flujo { get; set; }
        public List<AlmacenesFLujoCD> Almacenes { get; set; }
        public string ErrorMessage { get; set; }
    }
}
