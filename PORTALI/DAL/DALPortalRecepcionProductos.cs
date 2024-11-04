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
    public class DALPortalRecepcionProductos
    {
        public static List<PortalRecepcionLogEntity> getLogRecepcion(string DocNum)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<PortalRecepcionLogEntity> Detalle = new List<PortalRecepcionLogEntity>();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_portal_recepcion_log", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@DocNum", DocNum);

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        Detalle = (from row in dt.AsEnumerable()
                                   select new PortalRecepcionLogEntity()
                                   {
                                       Code = int.Parse(row["Code"].ToString()),
                                       DocEntry = int.Parse(row["DocEntry"].ToString()),
                                       DocNum = int.Parse(row["DocNum"].ToString()),
                                       Notas = row["Notas"].ToString(),
                                       Usuario = row["Usuario"].ToString(),
                                       IdEstatus = row["IdEstatus"].ToString(),
                                       Estatus = row["Estatus"].ToString(),
                                       Creado = DateTime.Parse(row["Creado"].ToString()),
                                       Fecha = row["Fecha"].ToString(),
                                       Letra = row["Letra"].ToString(),
                                       Texto = row["Texto"].ToString()
                                   }).ToList();
                        return Detalle;
                    }

                    return new List<PortalRecepcionLogEntity>();
                }
                catch (Exception ex)
                {
                    return new List<PortalRecepcionLogEntity>();
                }
            }
        }
        public static List<PortalRecepcionEstadosEntity> getAllEstadosRecepcion(string IdEstatus)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<PortalRecepcionEstadosEntity> Detalle = new List<PortalRecepcionEstadosEntity>();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("SELECT FldValue As Id, Descr As Dscription FROM [UFD1] WHERE TableID = '@PR_REME'", iConnection);
                iCommand.CommandType = CommandType.Text;
                
                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        Detalle = (from row in dt.AsEnumerable()
                                   select new PortalRecepcionEstadosEntity()
                                   {
                                       Id = int.Parse(row["Id"].ToString()),
                                       Dscription = row["Dscription"].ToString(),
                                       Selected = (IdEstatus == row["Id"].ToString() ? "selected" : "")
                                   }).ToList();
                        return Detalle;
                    }

                    return new List<PortalRecepcionEstadosEntity>();
                }
                catch (Exception ex)
                {
                    return new List<PortalRecepcionEstadosEntity>();
                }
            }
        }
        public static List<RecepcionProductosDetalleEntity> getDetalleProducts(int DocNum)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<RecepcionProductosDetalleEntity> Detalle = new List<RecepcionProductosDetalleEntity>();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_portal_recepcion_productos_detalle", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;                
                iCommand.Parameters.AddWithValue("@DocNum", DocNum);
                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        Detalle = (from row in dt.AsEnumerable()
                                   select new RecepcionProductosDetalleEntity()
                                   {
                                       ItemCode = row["ItemCode"].ToString(),
                                       ItemName = row["Dscription"].ToString(),
                                       LineText = row["LineText"].ToString(),
                                       Price = double.Parse(row["Price"].ToString()),
                                       Quantity = double.Parse(row["Quantity"].ToString()),
                                       Total = double.Parse(row["LineTotal"].ToString())
                                   }).ToList();
                        return Detalle;
                    }

                    return new List<RecepcionProductosDetalleEntity>();
                }
                catch (Exception ex)
                {
                    return new List<RecepcionProductosDetalleEntity>();
                }
            }
        }
        public static RecepcionProductosModalDetalleEntity getSingle(int Depto, int DocNum)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            RecepcionProductosModalDetalleEntity ListadoTiendas = new RecepcionProductosModalDetalleEntity();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_portal_recepcion_productos", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@Depto", Depto);
                iCommand.Parameters.AddWithValue("@DocNum", DocNum);
                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        return new RecepcionProductosModalDetalleEntity
                        {
                            DocNumOc = int.Parse(dt.Rows[0]["DocNumOC"].ToString()),
                            DocEntryOc = int.Parse(dt.Rows[0]["DocEntryOc"].ToString()),
                            CardCode = dt.Rows[0]["CardCode"].ToString(),
                            CardName = dt.Rows[0]["CardName"].ToString(),
                            Comments = dt.Rows[0]["Comments"].ToString(),
                            Depto = dt.Rows[0]["Depto"].ToString(),
                            IdEstado = dt.Rows[0]["IdEstado"].ToString(),
                            Sucursal = dt.Rows[0]["Sucursal"].ToString(),
                            Tienda = dt.Rows[0]["WhsName"].ToString(),
                            DocDateCoti = DateTime.Parse(dt.Rows[0]["DocDateCoti"].ToString()),
                            DocDateOc = DateTime.Parse(dt.Rows[0]["DocDateOc"].ToString()),
                            DocDateReq = DateTime.Parse(dt.Rows[0]["DocDateReq"].ToString()),
                            SubTotal = double.Parse(dt.Rows[0]["SubTotal"].ToString()),
                            Impuesto = double.Parse(dt.Rows[0]["Impuesto"].ToString()),
                            DocTotal = double.Parse(dt.Rows[0]["DocTotalOc"].ToString())
                        };
                    }

                    return new RecepcionProductosModalDetalleEntity();
                }
                catch (Exception ex)
                {
                    return new RecepcionProductosModalDetalleEntity();
                }
            }
        }
        public static List<RecepcionProductosEntity> getAll(int Depto, int DocNum)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<RecepcionProductosEntity> ListadoTiendas = new List<RecepcionProductosEntity>();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_portal_recepcion_productos", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@Depto", Depto);
                iCommand.Parameters.AddWithValue("@DocNum", DocNum);
                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        ListadoTiendas = (from row in dt.AsEnumerable()
                                          select new RecepcionProductosEntity()
                                          {
                                              DocNumOc = int.Parse(row["DocNumOC"].ToString()),
                                              DocEntryOc = int.Parse(row["DocEntryOc"].ToString()),
                                              CardCode = row["CardCode"].ToString(),
                                              CardName = row["CardName"].ToString(),
                                              Comments = row["Comments"].ToString(),
                                              DocDateCoti = DateTime.Parse(row["DocDateCoti"].ToString()),
                                              DocDateOc = DateTime.Parse(row["DocDateOc"].ToString()),
                                              DocDateReq = DateTime.Parse(row["DocDateReq"].ToString()),
                                              DocTotal = double.Parse(row["DocTotalOc"].ToString())
                                          }).ToList();
                        return ListadoTiendas;
                    }

                    return new List<RecepcionProductosEntity>();
                }
                catch (Exception ex)
                {
                    return new List<RecepcionProductosEntity>();
                }
            }
        }
    }
}
