using System;
using System.Collections.Generic;
using System.Linq;
using Entity;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;

namespace DAL
{
    public class DALComercial
    {
        public static ComercialTransaccionesEntity EncabezadoTransacciones(DateTime FechaI, DateTime FechaF, string WhsCode, string Region)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                using (OleDbCommand iCommand = new OleDbCommand("sp_comercial_transacciones_generales_v3", iConnection))
                {
                    iCommand.CommandType = CommandType.StoredProcedure;
                    iCommand.Parameters.AddWithValue("@FechaI", FechaI);
                    iCommand.Parameters.AddWithValue("@FechaF", FechaF);
                    iCommand.Parameters.AddWithValue("@WhsCode", WhsCode);
                    iCommand.Parameters.AddWithValue("@Region", Region);

                    try
                    {
                        ComercialTransaccionesEntity datos = new ComercialTransaccionesEntity();
                        DataTable dt = new DataTable();
                        using (OleDbDataAdapter iDAResult = new OleDbDataAdapter(iCommand))
                        {
                            iDAResult.Fill(dt);
                        }
                        if (dt.Rows.Count > 0)
                        {
                            datos.WhsCode = dt.Rows[0]["WhsCode"].ToString();
                            datos.WhsName = dt.Rows[0]["WhsName"].ToString();
                            datos.Meta = double.Parse(dt.Rows[0]["Meta"].ToString());
                            datos.MetaTrans = int.Parse(dt.Rows[0]["MetaTrans"].ToString());
                            datos.CantidadTrans = int.Parse(dt.Rows[0]["CantidadTrans"].ToString());
                            datos.PromedioDiario = int.Parse(dt.Rows[0]["PromedioDiario"].ToString());
                            datos.Indice = double.Parse(dt.Rows[0]["Indice"].ToString());
                            datos.IndiceProyectado = double.Parse(dt.Rows[0]["IndiceProyectado"].ToString());
                        }

                        return datos;
                    }
                    catch (Exception ex)
                    {
                        // Puedes loggear ex.Message si deseas
                        return new ComercialTransaccionesEntity();
                    }
                }
            }
        }
        public static List<VendedorMeta> TransaccionesPorAsesor(DateTime FechaI, DateTime FechaF, string WhsCode)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                using (OleDbCommand iCommand = new OleDbCommand("sp_comercial_transacciones_asesores", iConnection))
                {
                    iCommand.CommandType = CommandType.StoredProcedure;
                    iCommand.Parameters.AddWithValue("@FechaI", FechaI);
                    iCommand.Parameters.AddWithValue("@FechaF", FechaF);
                    iCommand.Parameters.AddWithValue("@WhsCode", WhsCode);

                    try
                    {
                        List<VendedorMeta> datos = new List<VendedorMeta>();
                        DataTable dt = new DataTable();
                        using (OleDbDataAdapter iDAResult = new OleDbDataAdapter(iCommand))
                        {
                            iDAResult.Fill(dt);
                        }
                        datos = (from Row in dt.AsEnumerable()
                                 select new VendedorMeta
                                 {
                                     Code = int.Parse(Row["Code"].ToString()),
                                     SlpName = Row["SlpName"].ToString(),
                                     MetaTickets = int.Parse(Row["MetaTickets"].ToString()),
                                     CantidadTrans = int.Parse(Row["CantidadTrans"].ToString())
                                 }).ToList();

                        return datos;
                    }
                    catch (Exception ex)
                    {
                        // Puedes loggear ex.Message si deseas
                        return new List<VendedorMeta>();
                    }
                }
            }
        }
        public static List<Top5TiendasEntity> Top5Tiendas(DateTime FechaI, DateTime FechaF, int Tipo, string Region)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<Top5TiendasEntity> listado = new List<Top5TiendasEntity>();

            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                using (OleDbCommand iCommand = new OleDbCommand("sp_comercial_transacciones_top5_tiendas_v2", iConnection))
                {
                    iCommand.CommandType = CommandType.StoredProcedure;
                    iCommand.Parameters.AddWithValue("@FechaI", FechaI);
                    iCommand.Parameters.AddWithValue("@FechaF", FechaF);
                    iCommand.Parameters.AddWithValue("@Tipo", Tipo);
                    iCommand.Parameters.AddWithValue("@Region", Region);
                    try
                    {
                        DataTable dt = new DataTable();
                        using (OleDbDataAdapter iDAResult = new OleDbDataAdapter(iCommand))
                        {
                            iDAResult.Fill(dt);
                        }

                        listado = (from row in dt.AsEnumerable()
                                   select new Top5TiendasEntity
                                   {
                                       WhsCode = row["WhsCode"].ToString(),
                                       WhsName = row["WhsName"].ToString(),
                                       Transacciones = int.Parse(row["Transacciones"].ToString())
                                   }).ToList();

                        return listado;
                    }
                    catch (Exception ex)
                    {
                        // Puedes loggear ex.Message si deseas
                        return new List<Top5TiendasEntity>();
                    }
                }
            }
        }

        public static List<TransaccionesEntity> TransaccionesDiarias(DateTime FechaI, DateTime FechaF, string WhsCode, string Region)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<TransaccionesEntity> listado = new List<TransaccionesEntity>();

            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                using (OleDbCommand iCommand = new OleDbCommand("sp_comercial_transacciones_diarias_v2", iConnection))
                {
                    iCommand.CommandType = CommandType.StoredProcedure;
                    iCommand.Parameters.AddWithValue("@FechaI", FechaI);
                    iCommand.Parameters.AddWithValue("@FechaF", FechaF);
                    iCommand.Parameters.AddWithValue("@WhsCode", WhsCode);
                    iCommand.Parameters.AddWithValue("@Region", Region);

                    try
                    {
                        DataTable dt = new DataTable();
                        using (OleDbDataAdapter iDAResult = new OleDbDataAdapter(iCommand))
                        {
                            iDAResult.Fill(dt);
                        }

                        listado = (from row in dt.AsEnumerable()
                                   select new TransaccionesEntity
                                   {
                                       Dia = int.Parse(row["Dia"].ToString()),
                                       NombreDia = row["NombreDia"].ToString(),
                                       D12AM = int.Parse(row["D12AM"].ToString()),
                                       D3PM = int.Parse(row["D3PM"].ToString()),
                                       Transacciones = int.Parse(row["Transacciones"].ToString())
                                   }).ToList();

                        return listado;
                    }
                    catch (Exception ex)
                    {
                        // Puedes loggear ex.Message si deseas
                        return new List<TransaccionesEntity>();
                    }
                }
            }
        }
    }
}
