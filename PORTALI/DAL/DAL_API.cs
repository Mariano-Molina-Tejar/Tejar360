using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DAL_API
    {
        public static string CrearCotizacionVenta(string Url, Object objecto)
        {
            try
            {
                return Connection.Conexion.ConsumirAPI(Url, objecto, null);
            }
            catch (Exception)
            {

                return "";
            }
        }

        public static string CrearPermiso(string Url, Object objecto)
        {
            try
            {
                return Connection.Conexion.ConsumirAPI(Url, objecto, null);
            }
            catch (Exception)
            {

                return "";
            }
        }
        public static string AutorizarRechazarPermiso(string Url, Object objecto)
        {
            try
            {
                return Connection.Conexion.ConsumirAPI(Url, objecto, null);
            }
            catch (Exception)
            {

                return "";
            }
        }

        public static string NotasPpto(string Url, Object objecto)
        {

            try
            {
                return Connection.Conexion.ConsumirAPI(Url, objecto, null);
            }
            catch (Exception)
            {
                return "";
            }
        }
        public static string EnvioCorreoReporteVentasEmpresa(string Url, Object objecto)
        {

            try
            {
                return Connection.Conexion.ConsumirAPI(Url, objecto, null);
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string CrearFormulario(string Url, Object objecto)
        {

            try
            {
                return Connection.Conexion.ConsumirAPI(Url, objecto, null);
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
