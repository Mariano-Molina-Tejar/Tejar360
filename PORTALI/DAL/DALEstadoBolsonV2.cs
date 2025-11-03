using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace DAL
{
    public class DALEstadoBolsonV2
    {
        public static List<EstadoBolsonV2Entity> BolsonActivoCotizacionesDetalle(int? IdRegion = null, string WhsCode = null, int? UserId = null)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<EstadoBolsonV2Entity> listado = new List<EstadoBolsonV2Entity>();

            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName +";Database=" + pConnection.DataBase +";Uid=" + pConnection.User +";Pwd=" + pConnection.Password + ";"))
            {
                using (OleDbCommand iCommand = new OleDbCommand("sp_estado_bolson_v2", iConnection))
                {
                    iCommand.CommandType = CommandType.StoredProcedure;
                    iCommand.Parameters.AddWithValue("@IdRegion", (object)IdRegion ?? DBNull.Value);
                    iCommand.Parameters.AddWithValue("@WhsCode", (object)WhsCode ?? DBNull.Value);
                    iCommand.Parameters.AddWithValue("@UserId", (object)UserId ?? DBNull.Value);

                    try
                    {
                        DataTable dt = new DataTable();
                        using (OleDbDataAdapter iDAResult = new OleDbDataAdapter(iCommand))
                        {
                            iDAResult.Fill(dt);
                        }

                        listado = (from row in dt.AsEnumerable()
                                   select new EstadoBolsonV2Entity
                                   {
                                       WhsCode = row["WhsCode"]?.ToString(),
                                       WhsName = row["WhsName"]?.ToString(),
                                       Region = row["Region"]?.ToString(),
                                       InfGen_BolsonAcumuladoUni = (int)(row["InfGen_BolsonAcumuladoUni"] != DBNull.Value ? Convert.ToDouble(row["InfGen_BolsonAcumuladoUni"]) : 0),
                                       InfGen_BolsonNuevoUni = (int)(row["InfGen_BolsonNuevoUni"] != DBNull.Value ? Convert.ToDouble(row["InfGen_BolsonNuevoUni"]) : 0),
                                       InfGen_BolsonAcumuladoQ = row["InfGen_BolsonAcumuladoQ"] != DBNull.Value ? Convert.ToDouble(row["InfGen_BolsonAcumuladoQ"]) : 0,
                                       InfGen_BolsonNuevoQ = row["InfGen_BolsonNuevoQ"] != DBNull.Value ? Convert.ToDouble(row["InfGen_BolsonNuevoQ"]) : 0,
                                       CotNue_TotalCotizaciones = (int)(row["CotNue_TotalCotizaciones"] != DBNull.Value ? Convert.ToDouble(row["CotNue_TotalCotizaciones"]) : 0),
                                       CotNue_MontoCotizadoQ = row["CotNue_MontoCotizadoQ"] != DBNull.Value ? Convert.ToDouble(row["CotNue_MontoCotizadoQ"]) : 0,
                                       TasaDeCotizacion = row["TasaDeCotizacion"] != DBNull.Value ? Convert.ToDouble(row["TasaDeCotizacion"]) : 0,
                                       Fac_TotalFacturas = (int)(row["Fac_TotalFacturas"] != DBNull.Value ? Convert.ToDouble(row["Fac_TotalFacturas"]) : 0),
                                       Fac_MontoFacturasQ = row["Fac_MontoFacturasQ"] != DBNull.Value ? Convert.ToDouble(row["Fac_MontoFacturasQ"]) : 0,
                                       Fac_TasaCierre = row["Fac_TasaCierre"] != DBNull.Value ? Convert.ToDouble(row["Fac_TasaCierre"]) : 0,
                                       Per_TotalCoti = row["Per_TotalCoti"] != DBNull.Value ? Convert.ToDouble(row["Per_TotalCoti"]) : 0,
                                       Per_MontoPerdidoQ = row["Per_MontoPerdidoQ"] != DBNull.Value ? Convert.ToDouble(row["Per_MontoPerdidoQ"]) : 0,
                                       Per_TasaPerdida = row["Per_TasaPerdida"] != DBNull.Value ? Convert.ToDouble(row["Per_TasaPerdida"]) : 0
                                   }).ToList();

                        return listado;
                    }
                    catch (Exception)
                    {
                        return new List<EstadoBolsonV2Entity>();
                    }
                }
            }
        }

    }
}
