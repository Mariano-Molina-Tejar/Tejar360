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
    public class DALBolsonActivo
    {
        public static List<ListadoBolsonEntity> BolsonActivoCotizaciones(int? IdRegion = null, string WhsCode = null, int? SlpCode = null, DateTime? FechaI = null, DateTime? FechaF = null, int? ReferidoSac = null)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<ListadoBolsonEntity> listado = new List<ListadoBolsonEntity>();

            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                using (OleDbCommand iCommand = new OleDbCommand("sp_boslon_activo_v4", iConnection))
                {
                    iCommand.CommandType = CommandType.StoredProcedure;
                    iCommand.Parameters.AddWithValue("@IdRegion", (object)IdRegion ?? DBNull.Value);
                    iCommand.Parameters.AddWithValue("@WhsCode", (object)WhsCode ?? DBNull.Value);
                    iCommand.Parameters.AddWithValue("@SlpCode", (object)SlpCode ?? DBNull.Value);
                    iCommand.Parameters.AddWithValue("@FechaI", (object)FechaI ?? DBNull.Value);
                    iCommand.Parameters.AddWithValue("@FechaF", (object)FechaF ?? DBNull.Value);
                    iCommand.Parameters.AddWithValue("@ReferidoSac", (object)ReferidoSac ?? DBNull.Value);

                    try
                    {
                        DataTable dt = new DataTable();
                        using (OleDbDataAdapter iDAResult = new OleDbDataAdapter(iCommand))
                        {
                            iDAResult.Fill(dt);
                        }

                        listado = (from row in dt.AsEnumerable()
                                   select new ListadoBolsonEntity
                                   {
                                       DocEntry = Convert.ToInt32(row["DocEntry"]),
                                       DocNum = Convert.ToInt32(row["DocNum"]),
                                       SlpCode = Convert.ToInt32(row["SlpCode"]),
                                       SlpName = row["SlpName"].ToString(),
                                       Nit = row["Nit"].ToString(),
                                       Nombre = row["Nombre"].ToString(),
                                       Telefono = row["Telefono"] == DBNull.Value ? null : row["Telefono"].ToString(),
                                       DocDate = row["DocDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["DocDate"]),
                                       DocTotal = row["DocTotal"] == DBNull.Value ? 0 : Convert.ToDecimal(row["DocTotal"]),
                                       PosibleFechaCom = row["PosibleFechaCom"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["PosibleFechaCom"]),
                                       Estado = row["Estado"].ToString(),
                                       FechaSeguimiento = row["FechaSeguimiento"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["FechaSeguimiento"]),
                                       WhsCode = row["WhsCode"].ToString(),
                                       WhsName = row["WhsName"].ToString(),
                                       IdRegion = Convert.ToInt32(row["IdRegion"]),
                                       Region = row["Region"].ToString(),
                                       Activo = double.Parse(row["Activo"].ToString()),
                                       Nuevos = double.Parse(row["Nuevos"].ToString()),
                                       Facturado = double.Parse(row["Facturado"].ToString()),
                                       Perdida = double.Parse(row["Perdida"].ToString()),
                                       Tipo = int.Parse(row["Tipo"].ToString())
                                   }).ToList();

                        return listado;
                    }
                    catch (Exception ex)
                    {
                        // Puedes loggear ex.Message si deseas
                        return new List<ListadoBolsonEntity>();
                    }
                }
            }
        }

        public static List<CrmSeguimientoCotiEntity> HistorialSeg(int DocEntry)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<CrmSeguimientoCotiEntity> listado = new List<CrmSeguimientoCotiEntity>();

            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                using (OleDbCommand iCommand = new OleDbCommand("sp_historial_seguimientos", iConnection))
                {
                    iCommand.CommandType = CommandType.StoredProcedure;
                    iCommand.Parameters.AddWithValue("@DocEntry", DocEntry);                    

                    try
                    {
                        DataTable dt = new DataTable();
                        using (OleDbDataAdapter iDAResult = new OleDbDataAdapter(iCommand))
                        {
                            iDAResult.Fill(dt);
                        }

                        listado = (from row in dt.AsEnumerable()
                                   select new CrmSeguimientoCotiEntity
                                   {
                                       FechaCreado = Convert.ToDateTime(row["Creado"].ToString()),
                                       DocEntry = Convert.ToInt32(row["DocEntry"]),
                                       DocNum = Convert.ToInt32(row["DocNum"]),
                                       NombreMedioContacto = row["MedioContacto"].ToString(),
                                       Notas = row["Notas"].ToString(),
                                       FechaSeguimiento = Convert.ToDateTime(row["FechaProximoSeg"].ToString()),
                                       PosFechaCompra = Convert.ToDateTime(row["PosibleFechaCompra"].ToString()),
                                       Usuario = row["Usuario"].ToString()
                                   }).ToList();

                        return listado;
                    }
                    catch (Exception ex)
                    {
                        // Puedes loggear ex.Message si deseas
                        return new List<CrmSeguimientoCotiEntity>();
                    }
                }
            }
        }

        public static List<CrmSeguimientoCotiEntity> HistorialSeguimientos(int DocEntry)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<CrmSeguimientoCotiEntity> listado = new List<CrmSeguimientoCotiEntity>();

            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                using (OleDbCommand iCommand = new OleDbCommand("SELECT a.*,b.Name As MedioContacto FROM [@CRM_SEGUIMIENTOS] a LEFT OUTER JOIN [@CRM_TIPOCONT] b ON a.U_MedioContacto = b.Code WHERE a.U_DocEntry = " + DocEntry + " ORDER BY a.Code DESC", iConnection))                {
                    iCommand.CommandType = CommandType.Text;
                    try
                    {
                        DataTable dt = new DataTable();
                        using (OleDbDataAdapter iDAResult = new OleDbDataAdapter(iCommand))
                        {
                            iDAResult.Fill(dt);
                        }

                        listado = (from row in dt.AsEnumerable()
                                   select new CrmSeguimientoCotiEntity
                                   {
                                       DocEntry = Convert.ToInt32(row["U_DocEntry"]),
                                       DocNum = Convert.ToInt32(row["U_DocNum"]),
                                       PosFechaCompra = row["U_PosFechaCompra"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["U_PosFechaCompra"]),
                                       MedioContacto = Convert.ToInt32(row["U_MedioContacto"]),
                                       NombreMedioContacto = row["MedioContacto"].ToString(),
                                       MotivoPerdida = Convert.ToInt32(row["U_MotivoPerdida"].ToString()),
                                       Notas = row["U_Notas"].ToString(),
                                       UserId = Convert.ToInt32(row["U_UserId"].ToString()),
                                       FechaSeguimiento = row["U_FechaSeguimiento"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["U_FechaSeguimiento"])
                                   }).ToList();

                        return listado;
                    }
                    catch (Exception ex)
                    {
                        // Puedes loggear ex.Message si deseas
                        return new List<CrmSeguimientoCotiEntity>();
                    }
                }
            }
        }
    }
}
