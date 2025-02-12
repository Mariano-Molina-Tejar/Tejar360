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
    public class DALPortalCarritoCompras
    {
        public static CarritoComprasPDFEntity getAllCotizacionesPendientesDetalle(int DocEntry, int SlpCode)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                CarritoComprasPDFEntity carritoComprasPDFEntity = new CarritoComprasPDFEntity();
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_autorizacion_cotizaciones_de_carrito_detalle", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@DocEntry", DocEntry);
                iCommand.Parameters.AddWithValue("@SlpCode", SlpCode);

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        PortalListadoCotizacionesEntity encabezado = new PortalListadoCotizacionesEntity();
                        encabezado.Nit = dt.Rows[0]["Nit"].ToString();
                        encabezado.DocDate = DateTime.Parse(dt.Rows[0]["DocDate"].ToString());
                        encabezado.CardName = dt.Rows[0]["FacNombre"].ToString();
                        encabezado.DocEntry = int.Parse(dt.Rows[0]["DocEntry"].ToString());


                        var listado = (from row in dt.AsEnumerable()
                                       select new PortalCotizacionesDetalleEntity()
                                       {
                                           ItemCode = row["ItemCode"].ToString(),
                                           Dscription = row["Dscription"].ToString(),
                                           Quantity = double.Parse(row["Quantity"].ToString()),
                                           Price = double.Parse(row["Price"].ToString()),
                                           Descuento = double.Parse(row["DiscPrcnt"].ToString()),
                                           LineTotal = double.Parse(row["LineTotal"].ToString())
                                       }).ToList();
                        carritoComprasPDFEntity.Encabezado = encabezado;
                        carritoComprasPDFEntity.Detalle = listado;
                    }
                    return carritoComprasPDFEntity;
                }
                catch (Exception ex)
                {
                    return carritoComprasPDFEntity;
                }
            }
        }

        public static List<PortalListadoCotizacionesEntity> getAllCotizacionesPendientes(int SlpCode)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_autorizacion_cotizaciones_de_carrito", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@SlpCode", SlpCode);

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        var listado = (from row in dt.AsEnumerable()
                                       select new PortalListadoCotizacionesEntity()
                                       {
                                           DocEntry = int.Parse(row["DocEntry"].ToString()),
                                           DocNum = int.Parse(row["DocNum"].ToString()),
                                           Nit = row["Nit"].ToString(),
                                           FacNombre = row["FacNombre"].ToString(),
                                           CardCode = row["CardCode"].ToString(),
                                           CardName = row["CardName"].ToString(),
                                           SlpCode = int.Parse(row["SlpCode"].ToString()),
                                           SlpName = row["SlpName"].ToString(),
                                           DocDate = DateTime.Parse(row["DocDate"].ToString()),
                                           DocTotal = double.Parse(row["DocTotal"].ToString()),
                                           IsCookie = "N",
                                           EstadoCoti = row["TipoCoti"].ToString(),
                                           DscrTipoCoti = row["DscrTipoCoti"].ToString()
                                       }).ToList();
                        return listado;
                    }
                    return new List<PortalListadoCotizacionesEntity>();
                }
                catch (Exception ex)
                {
                    return new List<PortalListadoCotizacionesEntity>();
                }
            }
        }

        public static CarritoComprasPDFEntity getDataCotizacionToPDF(int DocEntry)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            CarritoComprasPDFEntity carritoComprasPDFEntity = new CarritoComprasPDFEntity();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_cotizaciones_de_carrito_pdf", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@DocEntry", DocEntry);

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {                        
                        PortalListadoCotizacionesEntity Enc = new PortalListadoCotizacionesEntity();
                        Enc.CardCode = dt.Rows[0]["CardCode"].ToString();
                        Enc.CardName = dt.Rows[0]["CardName"].ToString();
                        Enc.DocNum = int.Parse(dt.Rows[0]["DocNum"].ToString());
                        Enc.DocEntry = int.Parse(dt.Rows[0]["DocEntry"].ToString());
                        Enc.FacNombre = dt.Rows[0]["FacNombre"].ToString();
                        Enc.Nit = dt.Rows[0]["Nit"].ToString();
                        Enc.Address = dt.Rows[0]["Address"].ToString();
                        Enc.SlpName = dt.Rows[0]["SlpName"].ToString();
                        Enc.DocDate = DateTime.Parse(dt.Rows[0]["DocDate"].ToString());
                        Enc.DocDueDate = DateTime.Parse(dt.Rows[0]["DocDueDate"].ToString());
                        Enc.SubTotal = double.Parse(dt.Rows[0]["SubTotal"].ToString());
                        Enc.Impuesto = double.Parse(dt.Rows[0]["Iva"].ToString());
                        Enc.Descuento = double.Parse(dt.Rows[0]["Descuento"].ToString());
                        Enc.DocTotal = double.Parse(dt.Rows[0]["DocTotal"].ToString());
                        Enc.DireccionTejar = dt.Rows[0]["DireccionTejar"].ToString();
                        Enc.Notas = dt.Rows[0]["Notas"].ToString();

                        var listado = (from row in dt.AsEnumerable()
                                       select new PortalCotizacionesDetalleEntity()
                                       {
                                           ItemCode = row["ItemCode"].ToString(),
                                           Dscription = row["Dscription"].ToString(),
                                           Quantity = double.Parse(row["Quantity"].ToString()),
                                           Price = double.Parse(row["Price"].ToString()),
                                           LineTotal = double.Parse(row["LineTotal"].ToString())
                                       }).ToList();

                        carritoComprasPDFEntity.Encabezado = Enc;
                        carritoComprasPDFEntity.Detalle = listado;
                        return carritoComprasPDFEntity;
                    }
                    return new CarritoComprasPDFEntity();
                }
                catch (Exception ex)
                {
                    return new CarritoComprasPDFEntity();
                }
            }
        }
        public static List<PortalListadoCotizacionesEntity> getAllCotizacionesVenta(int SlpCode)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_cotizaciones_de_carrito", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@SlpCode", SlpCode);

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        var listado = (from row in dt.AsEnumerable()
                                       select new PortalListadoCotizacionesEntity()
                                       {
                                           DocEntry = int.Parse(row["DocEntry"].ToString()),
                                           DocNum = int.Parse(row["DocNum"].ToString()),
                                           Nit = row["Nit"].ToString(),
                                           FacNombre = row["FacNombre"].ToString(),
                                           CardCode = row["CardCode"].ToString(),
                                           CardName = row["CardName"].ToString(),
                                           SlpCode = int.Parse(row["SlpCode"].ToString()),
                                           DocDate = DateTime.Parse(row["DocDate"].ToString()),
                                           DocTotal = double.Parse(row["DocTotal"].ToString()),
                                           IsCookie = "N",
                                           EstadoCoti = row["TipoCoti"].ToString(),
                                           DscrTipoCoti = row["DscrTipoCoti"].ToString()
                                       }).ToList();
                        return listado;
                    }
                    return new List<PortalListadoCotizacionesEntity>();
                }
                catch (Exception ex)
                {
                    return new List<PortalListadoCotizacionesEntity>();
                }
            }
        }
        public static List<ItemsCotizacionEntity> CambioPrecioCotizacion(int PriceList, string ItemsCode, string Tienda)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<ItemsCotizacionEntity> detalle = new List<ItemsCotizacionEntity>();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_cambio_precios_cotizacion", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@PriceList", PriceList);
                iCommand.Parameters.AddWithValue("@ItemsCode", ItemsCode);
                iCommand.Parameters.AddWithValue("@WhsCode", Tienda);

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        detalle = (from row in dt.AsEnumerable()
                                   select new ItemsCotizacionEntity()
                                   {
                                       ItemCode = row["ItemCode"].ToString(),
                                       Tipo = row["Tipo"].ToString(),
                                       PreciUnitAlto = double.Parse(row["PreciUnitAlto"].ToString()),
                                       Price = double.Parse(row["Price"].ToString()),
                                       DescuentoQ = double.Parse(row["Descuento"].ToString())
                                   }).ToList();
                        return detalle;
                    }
                    return detalle;
                }
                catch (Exception ex)
                {
                    return new List<ItemsCotizacionEntity>();
                }
            }
        }

        public static CarritoComprasSocioEntity getSingleSocio(string Nit)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            CarritoComprasSocioEntity carritoComprasSocioEntity = new CarritoComprasSocioEntity();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("SELECT TOP 1 CardCode,UPPER(CardName) As CardName,UPPER(Address) As Address,UPPER(LicTradNum) As LicTradNum,Phone1,E_Mail FROM OCRD WHERE LicTradNum = '" + Nit + "' ORDER BY CardCode ASC", iConnection);
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
                        carritoComprasSocioEntity.CardCode = dt.Rows[0]["CardCode"].ToString();
                        carritoComprasSocioEntity.CardName = dt.Rows[0]["CardName"].ToString();
                        carritoComprasSocioEntity.Address = dt.Rows[0]["Address"].ToString();
                        carritoComprasSocioEntity.LicTradNum = dt.Rows[0]["LicTradNum"].ToString();
                        carritoComprasSocioEntity.Phone = dt.Rows[0]["Phone1"].ToString();
                        carritoComprasSocioEntity.Email = dt.Rows[0]["E_Mail"].ToString();

                        return carritoComprasSocioEntity;
                    }
                    return new CarritoComprasSocioEntity();
                }
                catch (Exception ex)
                {
                    return new CarritoComprasSocioEntity();
                }
            }
        }
    }
}
