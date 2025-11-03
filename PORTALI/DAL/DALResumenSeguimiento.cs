using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DALResumenSeguimiento
    {
        public static List<ResumenDeSeguimientoEntity> ResumenSeguimientoDetalle(string WhsCode = null)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<ResumenDeSeguimientoEntity> listado = new List<ResumenDeSeguimientoEntity>();

            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                using (OleDbCommand iCommand = new OleDbCommand("sp_crm_resumen_seguimientos_detalle_v2", iConnection))
                {
                    iCommand.CommandType = CommandType.StoredProcedure;
                    //iCommand.Parameters.AddWithValue("@IdRegion", (object)IdRegion ?? DBNull.Value);
                    iCommand.Parameters.AddWithValue("@WhsCode", (object)WhsCode ?? DBNull.Value);
                    //iCommand.Parameters.AddWithValue("@UserId", (object)UserId ?? DBNull.Value);

                    try
                    {
                        DataTable dt = new DataTable();
                        using (OleDbDataAdapter iDAResult = new OleDbDataAdapter(iCommand))
                        {
                            iDAResult.Fill(dt);
                        }

                        listado = (from row in dt.AsEnumerable()
                                   select new ResumenDeSeguimientoEntity
                                   {
                                       IdRegion = int.Parse(row["IdRegion"].ToString()),
                                       Region = row["Region"]?.ToString(),
                                       WhsCode = row["WhsCode"]?.ToString(),
                                       WhsName = row["WhsName"]?.ToString(),
                                       SlpCode = int.Parse(row["SlpCode"].ToString()),
                                       SlpName = row["SlpName"].ToString(),
                                       TotalActivo = double.Parse(row["TotalActivo"].ToString()),
                                       CantidadActivo = int.Parse(row["CantidadActivo"].ToString()),
                                       TotalSeguimiento = double.Parse(row["TotalSeguimiento"].ToString()),
                                       TasaSeg = double.Parse(row["TasaSeg"].ToString()),
                                       CantidadSeg = int.Parse(row["CantidadSeg"].ToString()),
                                       TotalFueraDeSeguimiento = double.Parse(row["TotalFueraDeSeguimiento"].ToString()),
                                       TasaFueraSeg = double.Parse(row["TasaFueraSeg"].ToString()),
                                       CantidadFueraSeg = int.Parse(row["CantidadFueraSeg"].ToString())
                                   }).ToList();

                        return listado;
                    }
                    catch (Exception)
                    {
                        return new List<ResumenDeSeguimientoEntity>();
                    }
                }
            }
        }
        public static List<ResumenDeSeguimientoEntity> ResumenSeguimiento(int? IdRegion = null, string WhsCode = null, int? UserId = null)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<ResumenDeSeguimientoEntity> listado = new List<ResumenDeSeguimientoEntity>();

            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                using (OleDbCommand iCommand = new OleDbCommand("sp_crm_resumen_seguimientos_v2", iConnection))
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
                                   select new ResumenDeSeguimientoEntity
                                   {
                                       IdRegion = int.Parse(row["IdRegion"].ToString()),
                                       Region = row["Region"]?.ToString(),
                                       WhsCode = row["WhsCode"]?.ToString(),
                                       WhsName = row["WhsName"]?.ToString(),
                                       TotalActivo = double.Parse(row["TotalActivo"].ToString()),
                                       CantidadActivo = int.Parse(row["CantidadActivo"].ToString()),
                                       TotalSeguimiento = double.Parse(row["TotalSeguimiento"].ToString()),
                                       TasaSeg = double.Parse(row["TasaSeg"].ToString()),
                                       CantidadSeg = int.Parse(row["CantidadSeg"].ToString()),
                                       TotalFueraDeSeguimiento = double.Parse(row["TotalFueraDeSeguimiento"].ToString()),
                                       TasaFueraSeg = double.Parse(row["TasaFueraSeg"].ToString()),
                                       CantidadFueraSeg = int.Parse(row["CantidadFueraSeg"].ToString())
                                   }).ToList();

                        return listado;
                    }
                    catch (Exception ex)
                    {
                        return new List<ResumenDeSeguimientoEntity>();
                    }
                }
            }
        }
    }
}
