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
        public static List<ListaPreciosEntity> ListaPreciosVentas(string ItemCode)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<ListaPreciosEntity> listadoProductos = new List<ListaPreciosEntity>();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_listado_precios_productos_detalle", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@ItemCode", ItemCode);

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
                                            select new ListaPreciosEntity()
                                            {
                                                IdLista = int.Parse(row["ListNum"].ToString()),
                                                ListName = row["ListName"].ToString(),
                                                Price = double.Parse(row["Price"].ToString()),
                                                Minimo = double.Parse(row["Minimo"].ToString()),
                                                Maximo = double.Parse(row["Maximo"].ToString())
                                            }).ToList();
                        return listadoProductos;
                    }

                    return listadoProductos;
                }
                catch (Exception ex)
                {
                    return new List<ListaPreciosEntity>();
                }
            }
        }
        public static List<BuscarProductoEntity> ListaProductosVentaCruzada(string ItemName, string WhsCode)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<BuscarProductoEntity> listadoProductos = new List<BuscarProductoEntity>();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_buscar_productos_venta_cruzada", iConnection);
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
                                                Imagen = row["UrlImagen"].ToString()
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
        public static List<BuscarProductoEntity> ListaProductosAltenativos(string ItemName, string WhsCode)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<BuscarProductoEntity> listadoProductos = new List<BuscarProductoEntity>();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_buscar_productos_alternativo", iConnection);
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
                                                Imagen = row["UrlImagen"].ToString()
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
                                                Stock = double.Parse(row["Stock"].ToString()),
                                                Direccion = row["Direccion"].ToString()
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
                        encabezado.Imagen = dt.Rows[0]["UrlImagen"].ToString();
                        encabezado.Ambiente = dt.Rows[0]["Ambiente"].ToString();
                        encabezado.Metraje = dt.Rows[0]["Metraje"].ToString();
                        encabezado.IdGrupo = int.Parse(dt.Rows[0]["IdGrupo"].ToString());
                        encabezado.MetrosDisp = double.Parse(dt.Rows[0]["MetrosDisp"].ToString());
                        encabezado.EsPromo = int.Parse(dt.Rows[0]["EsPromo"].ToString());
                        encabezado.PrecioPromocion = double.Parse(dt.Rows[0]["PrecioPromo"].ToString());
                        encabezado.TipoPromo = dt.Rows[0]["TipoPromo"].ToString();
                        encabezado.PLFinal = int.Parse(dt.Rows[0]["LpFinal"].ToString());
                        encabezado.PrecioFinal = double.Parse(dt.Rows[0]["PrecioFinal"].ToString());
                    }

                    return encabezado;
                }
                catch (Exception ex)
                {
                    return new BuscarProductoEntity();
                }
            }
        }
               
        public static List<BuscarProductoEntity> ListadoProductos(string ItemName, string WhsCode, bool Promos)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<BuscarProductoEntity> listadoProductos = new List<BuscarProductoEntity>();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_buscar_productos_v2", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@ItemName", ItemName);
                iCommand.Parameters.AddWithValue("@WhsCode", WhsCode);
                iCommand.Parameters.AddWithValue("@Promos", Promos);

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
                                                PrecioPromocion = double.Parse(row["PrecioPromo"].ToString()),
                                                DetallePromo = row["Detalle"].ToString(),
                                                Imagen = row["UrlImagen"].ToString(),
                                                EsPromo = int.Parse(row["EsPromo"].ToString())
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
