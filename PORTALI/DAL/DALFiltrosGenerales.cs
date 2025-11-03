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
    public class DALFiltrosGenerales
    {
        public static List<ListaFiltroAsesoresEntity> ListadoAsesoresCotizaciones(string WhsCode)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<ListaFiltroAsesoresEntity> listado = new List<ListaFiltroAsesoresEntity>();

            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                using (OleDbCommand iCommand = new OleDbCommand("sp_listado_asesores", iConnection))
                {
                    iCommand.CommandType = CommandType.StoredProcedure;
                    iCommand.Parameters.AddWithValue("@WhsCode", WhsCode);

                    try
                    {
                        DataTable dt = new DataTable();
                        using (OleDbDataAdapter iDAResult = new OleDbDataAdapter(iCommand))
                        {
                            iDAResult.Fill(dt);
                        }

                        listado = (from row in dt.AsEnumerable()
                                   select new ListaFiltroAsesoresEntity
                                   {
                                       SlpCode = int.Parse(row["SlpCode"].ToString()),
                                       SlpName = row["SlpName"].ToString()
                                   }).ToList();

                        return listado;
                    }
                    catch (Exception ex)
                    {
                        // Puedes loggear ex.Message si deseas
                        return new List<ListaFiltroAsesoresEntity>();
                    }
                }
            }
        }
        public static List<ListaFiltroRegionesEntity> ListaDeRegiones(int UserId)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<ListaFiltroRegionesEntity> listado = new List<ListaFiltroRegionesEntity>();

            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                using (OleDbCommand iCommand = new OleDbCommand("sp_lista_regiones_combo", iConnection))
                {
                    iCommand.CommandType = CommandType.StoredProcedure;
                    iCommand.Parameters.AddWithValue("@UserId", UserId);

                    try
                    {
                        DataTable dt = new DataTable();
                        using (OleDbDataAdapter iDAResult = new OleDbDataAdapter(iCommand))
                        {
                            iDAResult.Fill(dt);
                        }

                        listado = (from row in dt.AsEnumerable()
                                   select new ListaFiltroRegionesEntity
                                   {
                                       IdRegion = Convert.ToInt32(row["Code"]),                                       
                                       Region = row["Location"].ToString()
                                   }).ToList();

                        return listado;
                    }
                    catch (Exception ex)
                    {
                        // Puedes loggear ex.Message si deseas
                        return new List<ListaFiltroRegionesEntity>();
                    }
                }
            }
        }

        public static List<ListaFiltroTiendasEntity> ListaDeTiendas(int UserId, int IdRegion)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<ListaFiltroTiendasEntity> listado = new List<ListaFiltroTiendasEntity>();

            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                using (OleDbCommand iCommand = new OleDbCommand("sp_lista_tiendas_combo", iConnection))
                {
                    iCommand.CommandType = CommandType.StoredProcedure;
                    iCommand.Parameters.AddWithValue("@UserId", UserId);
                    iCommand.Parameters.AddWithValue("@IdRegion", IdRegion);

                    try
                    {
                        DataTable dt = new DataTable();
                        using (OleDbDataAdapter iDAResult = new OleDbDataAdapter(iCommand))
                        {
                            iDAResult.Fill(dt);
                        }

                        listado = (from row in dt.AsEnumerable()
                                   select new ListaFiltroTiendasEntity
                                   {
                                       WhsCode = row["WhsCode"].ToString(),
                                       WhsName = row["WhsName"].ToString()
                                   }).ToList();

                        return listado;
                    }
                    catch (Exception ex)
                    {
                        // Puedes loggear ex.Message si deseas
                        return new List<ListaFiltroTiendasEntity>();
                    }
                }
            }
        }

        public static List<ListaFiltroAsesoresEntity> ListaDeAsesores(int UserId, int IdRegion, string WhsCode)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<ListaFiltroAsesoresEntity> listado = new List<ListaFiltroAsesoresEntity>();

            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                using (OleDbCommand iCommand = new OleDbCommand("sp_lista_asesores_combo_v2", iConnection))
                {
                    iCommand.CommandType = CommandType.StoredProcedure;
                    iCommand.Parameters.AddWithValue("@UserId", UserId);
                    iCommand.Parameters.AddWithValue("@IdRegion", IdRegion);
                    iCommand.Parameters.AddWithValue("@WhsCode", WhsCode);

                    try
                    {
                        DataTable dt = new DataTable();
                        using (OleDbDataAdapter iDAResult = new OleDbDataAdapter(iCommand))
                        {
                            iDAResult.Fill(dt);
                        }

                        listado = (from row in dt.AsEnumerable()
                                   select new ListaFiltroAsesoresEntity
                                   {
                                       SlpCode = int.Parse(row["SlpCode"].ToString()),
                                       SlpName = row["SlpName"].ToString()
                                   }).ToList();

                        return listado;
                    }
                    catch (Exception ex)
                    {
                        // Puedes loggear ex.Message si deseas
                        return new List<ListaFiltroAsesoresEntity>();
                    }
                }
            }
        }
    }    
}
