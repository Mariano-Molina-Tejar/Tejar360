using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Utilitario
{
    public class Resultado <T>
    {
        public List<T> Datos { get; set; } = new List<T>();
        public string  Error { get; set; }
        //Devuelve true si no hay error, false si hay 
        public bool Exitoso => string.IsNullOrEmpty(Error);
        //Devuelve un resultado con error
        public static Resultado<T> ConError(string mensaje)
        {
            return new Resultado<T> { Error = mensaje };
        }
        //Devuelve un resultado con datos (cuando todo salió bien)
        public static Resultado<T> ConDatos(List<T> datos)
        {
            return new Resultado<T> { Datos = datos };
        }
    }
}
