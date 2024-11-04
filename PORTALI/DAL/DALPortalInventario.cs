using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using Connection;
using System.Data.OleDb;
using System.Data;

namespace DAL
{
    public class DALPortalInventario
    {
        public static List<BusquedaDetalleTiendasEntity> ListadoProductosStockTiendas(string ItemCode, string WhsCode)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<BusquedaDetalleTiendasEntity> listadoProductos = new List<BusquedaDetalleTiendasEntity>();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_buscar_productos_detalle_stock_tiendas", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@ItemCode", ItemCode);
                iCommand.Parameters.AddWithValue("@WhsCode", WhsCode);

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        listadoProductos = (from row in dt.AsEnumerable()
                                            select new BusquedaDetalleTiendasEntity()
                                            {
                                                WhsCode = row["WhsCode"].ToString(),
                                                WhsName = row["WhsName"].ToString(),
                                                Stock = double.Parse(row["Stock"].ToString())
                                            }).ToList();
                        return listadoProductos;
                    }

                    return listadoProductos;
                }
                catch (Exception ex)
                {
                    return new List<BusquedaDetalleTiendasEntity>();
                }
            }
        }
        public static BuscarProductoEntity DetalleSingle(string ItemCode, string WhsCode)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            BuscarProductoEntity encabezado = new BuscarProductoEntity();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_buscar_productos_detalle", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@ItemName", ItemCode);
                iCommand.Parameters.AddWithValue("@WhsCode", WhsCode);

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        encabezado.ItemCode = dt.Rows[0]["ItemCode"].ToString();
                        encabezado.ItemName = dt.Rows[0]["ItemName"].ToString();
                        encabezado.WhsCode = dt.Rows[0]["WhsCode"].ToString();
                        encabezado.Stock = double.Parse(dt.Rows[0]["Stock"].ToString());
                        encabezado.PrecioVenta = double.Parse(dt.Rows[0]["PrecioVenta"].ToString());
                        encabezado.PrecioBeneficio = double.Parse(dt.Rows[0]["PrecioBeneficio"].ToString());
                        encabezado.Formato = dt.Rows[0]["Formato"].ToString();
                    }

                    return encabezado;
                }
                catch (Exception ex)
                {
                    return new BuscarProductoEntity();
                }
            }
        }
        public static List<BuscarProductoEntity> ListadoProductos(string ItemName, string WhsCode)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<BuscarProductoEntity> listadoProductos = new List<BuscarProductoEntity>();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_buscar_productos", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@ItemName", ItemName);
                iCommand.Parameters.AddWithValue("@WhsCode", WhsCode);

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        listadoProductos = (from row in dt.AsEnumerable()
                                            select new BuscarProductoEntity()
                                            {
                                                ItemCode = row["ItemCode"].ToString(),
                                                ItemName = row["ItemName"].ToString(),
                                                WhsCode = row["WhsCode"].ToString(),
                                                PrecioBeneficio = double.Parse(row["PrecioBeneficio"].ToString()),
                                                PrecioVenta = double.Parse(row["PrecioVenta"].ToString()),
                                                Stock = double.Parse(row["Stock"].ToString()),
                                                Imagen = ""
                                            }).ToList();
                        return listadoProductos;
                    }

                    return listadoProductos;
                }
                catch (Exception ex)
                {
                    return new List<BuscarProductoEntity>();
                }
            }
        }
    }
}
