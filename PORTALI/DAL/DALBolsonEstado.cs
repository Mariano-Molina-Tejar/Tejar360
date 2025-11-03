using Entity;
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
    public class DALBolsonEstado
    {
        public static CrmEstadoBolsonDetalleEntity BolsonActivoCotizacionesAsesor(int? IdRegion = null, string WhsCode = null, int? SlpCode = null)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            CrmEstadoBolsonDetalleEntity listado = new CrmEstadoBolsonDetalleEntity();

            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                using (OleDbCommand iCommand = new OleDbCommand("sp_crm_estado_bolson_por_asesor", iConnection))
                {
                    iCommand.CommandType = CommandType.StoredProcedure;
                    iCommand.Parameters.AddWithValue("@IdRegion", (object)IdRegion ?? DBNull.Value);
                    iCommand.Parameters.AddWithValue("@WhsCode", (object)WhsCode ?? DBNull.Value);
                    iCommand.Parameters.AddWithValue("@SlpCode", (object)SlpCode ?? DBNull.Value);

                    try
                    {
                        iConnection.Open();
                        using (OleDbDataReader reader = iCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                listado.SlpName = reader["SlpName"].ToString();
                                listado.BolsonActivo = Convert.ToDouble(reader["BolsonActivo"]);
                                listado.LineasCot = Convert.ToInt32(reader["LineasCot"]);
                                listado.DocTotalCot = Convert.ToDouble(reader["DocTotalCot"]);
                                listado.LineasFac = Convert.ToInt32(reader["LineasFac"]);
                                listado.DocTotalFac = Convert.ToDouble(reader["DocTotalFac"]);
                                listado.TasaCierre = Convert.ToDouble(reader["TasaCierre"]);
                                listado.LineasPerd = Convert.ToInt32(reader["LineasPerd"]);
                                listado.DocTotalPerd = Convert.ToDouble(reader["DocTotalPerd"]);
                                listado.TasaPerd = Convert.ToDouble(reader["TasaPerd"]);
                                listado.TasaGeneracion = Convert.ToDouble(reader["TasaGeneracion"]);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log o manejo de errores si lo deseas
                        // return null o nuevo objeto vacío según tu lógica
                        listado = new CrmEstadoBolsonDetalleEntity(); // retorno seguro
                    }
                }
            }
            return listado;
        }
        public static List<CrmEstadoBolsonDetalleEntity> BolsonActivoCotizacionesDetalle(int? IdRegion = null, string WhsCode = null, int? SlpCode = null)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<CrmEstadoBolsonDetalleEntity> listado = new List<CrmEstadoBolsonDetalleEntity>();

            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                using (OleDbCommand iCommand = new OleDbCommand("sp_crm_estado_bolson_por_tienda", iConnection))
                {
                    iCommand.CommandType = CommandType.StoredProcedure;
                    iCommand.Parameters.AddWithValue("@IdRegion", (object)IdRegion ?? DBNull.Value);
                    iCommand.Parameters.AddWithValue("@WhsCode", (object)WhsCode ?? DBNull.Value);
                    iCommand.Parameters.AddWithValue("@SlpCode", (object)SlpCode ?? DBNull.Value);

                    try
                    {
                        DataTable dt = new DataTable();
                        using (OleDbDataAdapter iDAResult = new OleDbDataAdapter(iCommand))
                        {
                            iDAResult.Fill(dt);
                        }

                        listado = (from row in dt.AsEnumerable()
                                   select new CrmEstadoBolsonDetalleEntity
                                   {
                                       SlpName = row["SlpName"].ToString(),
                                       BolsonActivo = Convert.ToDouble(row["BolsonActivo"]),
                                       LineasCot = Convert.ToInt32(row["LineasCot"]),
                                       DocTotalCot = Convert.ToDouble(row["DocTotalCot"]),
                                       LineasFac = Convert.ToInt32(row["LineasFac"]),
                                       DocTotalFac = Convert.ToDouble(row["DocTotalFac"]),
                                       TasaCierre = Convert.ToDouble(row["TasaCierre"]),
                                       LineasPerd = Convert.ToInt32(row["LineasPerd"]),
                                       DocTotalPerd = Convert.ToDouble(row["DocTotalPerd"]),
                                       TasaPerd = Convert.ToDouble(row["TasaPerd"]),
                                       TasaGeneracion = Convert.ToDouble(row["TasaGeneracion"])
                                   }).ToList();

                        return listado;
                    }
                    catch (Exception ex)
                    {
                        // Puedes loggear ex.Message si deseas
                        return new List<CrmEstadoBolsonDetalleEntity>();
                    }
                }
            }
        }
        public static List<CrmEstadoBolsonEntity> BolsonActivoCotizaciones(int? IdRegion = null, string WhsCode = null, int? SlpCode = null, int? UserId = null)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<CrmEstadoBolsonEntity> listado = new List<CrmEstadoBolsonEntity>();

            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                using (OleDbCommand iCommand = new OleDbCommand("sp_crm_estado_bolson", iConnection))
                {
                    iCommand.CommandType = CommandType.StoredProcedure;
                    iCommand.Parameters.AddWithValue("@IdRegion", (object)IdRegion ?? DBNull.Value);
                    iCommand.Parameters.AddWithValue("@WhsCode", (object)WhsCode ?? DBNull.Value);
                    iCommand.Parameters.AddWithValue("@SlpCode", (object)SlpCode ?? DBNull.Value);
                    iCommand.Parameters.AddWithValue("@UserId", (object)UserId ?? DBNull.Value);

                    try
                    {
                        DataTable dt = new DataTable();
                        using (OleDbDataAdapter iDAResult = new OleDbDataAdapter(iCommand))
                        {
                            iDAResult.Fill(dt);
                        }

                        listado = (from row in dt.AsEnumerable()
                                       select new CrmEstadoBolsonEntity
                                       {
                                           WhsCode = row["WhsCode"].ToString(),
                                           WhsName = row["WhsName"].ToString(),
                                           Location = row["Location"].ToString(),
                                           BolsonActivo = Convert.ToDouble(row["BolsonActivo"]),
                                           LineasCot = Convert.ToInt32(row["LineasCot"]),
                                           DocTotalCot = Convert.ToDouble(row["DocTotalCot"]),
                                           LineasFac = Convert.ToInt32(row["LineasFac"]),
                                           DocTotalFac = Convert.ToDouble(row["DocTotalFac"]),
                                           TasaCierre = Convert.ToDouble(row["TasaCierre"]),
                                           LineasPerd = Convert.ToInt32(row["LineasPerd"]),
                                           DocTotalPerd = Convert.ToDouble(row["DocTotalPerd"]),
                                           TasaPerd = Convert.ToDouble(row["TasaPerd"]),
                                           TasaGeneracion = Convert.ToDouble(row["TasaGeneracion"])
                                       }).ToList();

                        return listado;
                    }
                    catch (Exception ex)
                    {
                        // Puedes loggear ex.Message si deseas
                        return new List<CrmEstadoBolsonEntity>();
                    }
                }
            }
        }
    }
}
