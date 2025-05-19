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
    public class DALPortalMargenUtilidad
    {
        public static List<ReporteDetalleProductoMargenEntity> ListadoProductosConMargen(int SlpCode, string WhsCode)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<ReporteDetalleProductoMargenEntity> listado = new List<ReporteDetalleProductoMargenEntity>();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_portal_listado_productos_margen_v2", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@SlpCode", SlpCode);
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
                        listado = (from row in dt.AsEnumerable()
                                   select new ReporteDetalleProductoMargenEntity()
                                   {
                                       Tipo = row["Tipo"].ToString(),
                                       DocDate = DateTime.Parse(row["DocDate"].ToString()),
                                       ItemCode = row["ItemCode"].ToString(),
                                       Dscription = row["Dscription"].ToString(),
                                       Quantity = double.Parse(row["Quantity"].ToString()),
                                       Ganancia = double.Parse(row["Ganancia"].ToString()),
                                       Base = double.Parse(row["Base"].ToString()),
                                       Margen = double.Parse(row["Margen"].ToString()),
                                       Imagen = row["Imagen"].ToString(),
                                       TipoDescuento = row["TipoD"].ToString(),
                                       PorcentajeDescuento = row["%"].ToString()
                                   }).ToList();
                        return listado;
                    }
                    return new List<ReporteDetalleProductoMargenEntity>();
                }
                catch (Exception ex)
                {
                    return new List<ReporteDetalleProductoMargenEntity>();
                }
            }
        }

        public static List<Reporte_Margen_Facturas> ListadoFacturasConMargen(int SlpCode, string WhsCode)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<Reporte_Margen_Facturas> Facturas = new List<Reporte_Margen_Facturas>();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_portal_listado_facturas_margen_v2", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@SlpCode", SlpCode);
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
                        Facturas = (from row in dt.AsEnumerable()
                                    select new Reporte_Margen_Facturas()
                                    {
                                        Tipo = row["Tipo"].ToString(),
                                        DocNum = Convert.ToInt32(row["DocNum"].ToString()),
                                        DocDate = DateTime.Parse(row["DocDate"].ToString()),
                                        CardName = row["CardName"].ToString(),
                                        Ganancia = Convert.ToDouble(row["Ganancia"].ToString()),
                                        Base = Convert.ToDouble(row["Base"].ToString()),
                                        LineTotal = Convert.ToDouble(row["LineTotal"].ToString()),
                                        Margen = Convert.ToDouble(row["Margen"].ToString())

                                    }).ToList();

                        return Facturas;
                    }
                    return new List<Reporte_Margen_Facturas>();
                }
                catch (Exception ex)
                {
                    return new List<Reporte_Margen_Facturas>();
                }
            }
        }



        public static List<ReporteMargenEntity> ListadoMargenAsesores(string UserCode, string WhsCode)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<ReporteMargenEntity> listado = new List<ReporteMargenEntity>();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_margen_utilidad_V2", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@UserCode", UserCode);
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
                        listado = (from row in dt.AsEnumerable()
                                   select new ReporteMargenEntity()
                                   {
                                       WhsCode = row["WhsCode"].ToString(),
                                       WhsName = row["WhsName"].ToString(),
                                       SlpCode = int.Parse(row["SlpCode"].ToString()),
                                       SlpName = row["SlpName"].ToString(),
                                       VentaActual = double.Parse(row["VentaActual"].ToString()),
                                       VentaProyectada = double.Parse(row["VentaProyectada"].ToString()),
                                       Rentabilidad = double.Parse(row["Rentabilidad"].ToString()),
                                       Facturas = int.Parse(row["Facturas"].ToString()),
                                       RentabilidadDecuento = double.Parse(row["Rentabilidad/Descuento"].ToString()),
                                       RentabilidadPromocion = double.Parse(row["Rentabilidad/Promocion"].ToString()),
                                       VentaNormal = double.Parse(row["VentaNormal"].ToString()),
                                       VentaDescuento = double.Parse(row["Venta/Descuento"].ToString()),
                                       VentPromocion = double.Parse(row["Venta/Promocion"].ToString())
                                   }).ToList();
                        return listado;
                    }
                    return new List<ReporteMargenEntity>();
                }
                catch (Exception ex)
                {
                    return new List<ReporteMargenEntity>();
                }
            }
        }

        public static ReporteMargenEntity ReporteUtilidadTienda(string UserCode, string WhsCode)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            ReporteMargenEntity reportedUlidad = new ReporteMargenEntity();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_margen_utilidad_tienda_v1", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@UserCode", UserCode);
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
                            reportedUlidad = new ReporteMargenEntity()
                            {


                                WhsCode = dt.Rows[0]["WhsCode"].ToString(),
                                WhsName = dt.Rows[0]["WhsName"].ToString(),
                                VentaActual = double.Parse(dt.Rows[0]["VentaActual"].ToString()),
                                VentaProyectada = double.Parse(dt.Rows[0]["VentaProyectada"].ToString()),
                                Rentabilidad = double.Parse(dt.Rows[0]["Rentabilidad"].ToString()),
                                Facturas = int.Parse(dt.Rows[0]["Facturas"].ToString()),
                                RentabilidadDecuento = double.Parse(dt.Rows[0]["Rentabilidad/Descuento"].ToString()),
                                RentabilidadPromocion = double.Parse(dt.Rows[0]["Rentabilidad/Promocion"].ToString())
                            };
                            return reportedUlidad;
                        }
                        return reportedUlidad;
                    }
                    catch (Exception ex)
                    {
                        return reportedUlidad;
                    }
                
            }
        }
        public static List<Detalle_Margen_Factura> DetalleFactura(int DocNum, string Tipo)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<Detalle_Margen_Factura> DetalleFacturas = new List<Detalle_Margen_Factura>();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_detalle_margen_factura", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@DocNum", DocNum);
                iCommand.Parameters.AddWithValue("@Tipo", Tipo);

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        DetalleFacturas = (from row in dt.AsEnumerable()
                                   select new Detalle_Margen_Factura()
                                   {
                                       Tipo = row["Tipo"].ToString(),
                                       LineNum = int.Parse(row["LineNum"].ToString()),
                                       DocNum = int.Parse(row["DocNum"].ToString()),
                                       DocDate = DateTime.Parse(row["DocDate"].ToString()),
                                       CardCode = row["CardCode"].ToString(),
                                       CardName = row["CardName"].ToString(),
                                       ItemCode = row["ItemCode"].ToString(),
                                       Dscripcion = row["Dscription"].ToString(),
                                       Quantity = double.Parse(row["Quantity"].ToString()),
                                       LineTotal = double.Parse(row["LineTotal"].ToString()),
                                       Margen = double.Parse(row["Margen"].ToString()),
                                       MargenTotal = double.Parse(row["MargenTotal"].ToString()),
                                       TipoD = row["TipoD"].ToString(),
                                       PorcentajeAutorizado = row["PorcentajeAutorizado"].ToString()
                                       
                                   }).ToList();
                        return DetalleFacturas;
                    }
                    return new List<Detalle_Margen_Factura>();
                }
                catch (Exception ex)
                {
                    return new List<Detalle_Margen_Factura>();
                }
            }
        }

    }
}
