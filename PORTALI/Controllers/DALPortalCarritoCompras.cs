using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using Newtonsoft.Json;

namespace DAL
{
    public class DALPortalCarritoCompras
    {
        public static bool UpdateAsesorCotizacion(int slpCode, int docEntry)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            using (OleDbConnection iConnection = new OleDbConnection(
                "Provider=SQLOLEDB;Server=" + pConnection.ServerName +
                ";Database=" + pConnection.DataBase +
                ";Uid=" + pConnection.User +
                ";Pwd=" + pConnection.Password + ";"))
            {
                try
                {
                    using (OleDbCommand iCommand = new OleDbCommand("sp_actualizar_asesor_cotizacion", iConnection))
                    {
                        iCommand.CommandType = CommandType.StoredProcedure;
                        iCommand.Parameters.AddWithValue("@DocEntry", docEntry);
                        iCommand.Parameters.AddWithValue("@SlpCode", slpCode);                        

                        iConnection.Open();
                        int rowsAffected = iCommand.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    // Aquí podrías loguear el error
                    return false;
                }
            }
        }

        public static List<PremiosPromosEntity> Complementarios(string dataJson, int PriceList, string WhsCode)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<PremiosPromosEntity> detalle = new List<PremiosPromosEntity>();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_crm_articulos_complementarios_v3", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@DatoJson", dataJson);
                iCommand.Parameters.AddWithValue("@PriceList", PriceList);
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
                        detalle = (from row in dt.AsEnumerable()
                                   select new PremiosPromosEntity()
                                   {
                                       ItemCode = row["ItemCode"].ToString(),
                                       ItemName = row["ItemName"].ToString(),
                                       UrlImagen = row["ImagenUrl"].ToString(),
                                       //Sugerido = int.Parse(row["Sugerido"].ToString()),
                                       Precio = double.Parse(row["Precio"].ToString()),
                                       Sugerido = int.Parse(row["Sugerido"].ToString()),
                                       Stock = int.Parse(row["Stock"].ToString())
                                       //Regalito = int.Parse(row["Sugerido"].ToString())
                                   }).ToList();
                        return detalle;
                    }
                    return detalle;
                }
                catch (Exception ex)
                {
                    return new List<PremiosPromosEntity>();
                }
            }
        }
        public static List<PremiosPromosEntity> PremiosPromocion(string dataJson)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<PremiosPromosEntity> detalle = new List<PremiosPromosEntity>();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_premios_promociones_v2", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@DatoJson", dataJson);                

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
                                   select new PremiosPromosEntity()
                                   {
                                       ItemCode = row["ItemCode"].ToString(),
                                       ItemName = row["ItemName"].ToString(),
                                       UrlImagen = row["ImagenUrl"].ToString(),
                                       Regalito = int.Parse(row["Regalito"].ToString())
                                   }).ToList();
                        return detalle;
                    }
                    return detalle;
                }
                catch (Exception ex)
                {
                    return new List<PremiosPromosEntity>();
                }
            }
        }
        public static List<ListaAsesoresEntity> getAllUsuariosPorTienda(string WhsCode)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_listado_asesores_por_tienda_2", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
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
                        var listado = (from row in dt.AsEnumerable()
                                       select new ListaAsesoresEntity()
                                       {
                                           SlpCode = int.Parse(row["SlpCode"].ToString()),
                                           SlpName = row["SlpName"].ToString()
                                       }).ToList();
                        return listado;
                    }
                    return new List<ListaAsesoresEntity>();
                }
                catch (Exception ex)
                {
                    return new List<ListaAsesoresEntity>();
                }
            }
        }

        public static CarritoComprasPDFEntity getAllCotizacionesPendientesDetalleAutorizacion(int DocEntry)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                CarritoComprasPDFEntity carritoComprasPDFEntity = new CarritoComprasPDFEntity();
                carritoComprasPDFEntity.Encabezado = new PortalListadoCotizacionesEntity();
                carritoComprasPDFEntity.Detalle = new List<PortalCotizacionesDetalleEntity>();

                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_autorizacion_cotizaciones_de_carrito_detalle_v3", iConnection);
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
                        PortalListadoCotizacionesEntity encabezado = new PortalListadoCotizacionesEntity();
                        encabezado.NitDocto = dt.Rows[0]["NitDocto"].ToString();
                        encabezado.NitCardCode = dt.Rows[0]["NitCardCode"].ToString();
                        encabezado.CardCode = dt.Rows[0]["CardCode"].ToString();
                        encabezado.DocNum = int.Parse(dt.Rows[0]["DocNum"].ToString());
                        encabezado.DocDate = DateTime.Parse(dt.Rows[0]["DocDate"].ToString());
                        encabezado.DocDueDate = DateTime.Parse(dt.Rows[0]["DocDueDate"].ToString());
                        encabezado.CardName = dt.Rows[0]["CardName"].ToString();
                        encabezado.DocEntry = int.Parse(dt.Rows[0]["DocEntry"].ToString());
                        encabezado.Comments = dt.Rows[0]["Comments"].ToString();
                        encabezado.DocNumFac = int.Parse(dt.Rows[0]["DocNumFac"].ToString());
                        encabezado.Address = dt.Rows[0]["AddressDocto"].ToString();
                        encabezado.DireccionCardCode = dt.Rows[0]["DireccionCardCode"].ToString();
                        encabezado.CorreoCliente = dt.Rows[0]["CorreoCardCode"].ToString();
                        encabezado.TelefonoCliente = dt.Rows[0]["TelefonoCardCode"].ToString();
                        encabezado.CorreoDocto = dt.Rows[0]["CorreoDocto"].ToString();
                        encabezado.TelefonoDocto = dt.Rows[0]["TelefonoDocto"].ToString();
                        encabezado.SlpName = dt.Rows[0]["SlpName"].ToString();
                        encabezado.Contacto = dt.Rows[0]["Contacto"].ToString();
                        encabezado.Estado = dt.Rows[0]["Estado"].ToString();
                        string historialNotasJson = dt.Rows[0]["HistorialNotas"].ToString();
                        encabezado.ListNum = int.Parse(dt.Rows[0]["ListNum"].ToString());
                        encabezado.ListName = dt.Rows[0]["ListName"].ToString();
                        encabezado.Texto = dt.AsEnumerable().Where(r => Convert.ToInt32(r["Rw"]) == 1).Select(r => r["Texto"]?.ToString()).FirstOrDefault();

                        var historialNotas = JsonConvert.DeserializeObject<List<HistoralNotas>>(historialNotasJson);

                        // asignar al encabezado
                        encabezado.HistorialNotas = historialNotas;

                        var listado = (from row in dt.AsEnumerable()
                                       select new PortalCotizacionesDetalleEntity()
                                       {
                                           LineNum = int.Parse(row["LineNum"].ToString()),
                                           ItemCode = row["ItemCode"].ToString(),
                                           Dscription = row["Dscription"].ToString(),
                                           Quantity = double.Parse(row["Quantity"].ToString()),
                                           Descuento = double.Parse(row["DiscPrcnt"].ToString()),
                                           PrecioReal = double.Parse(row["PrecioReal"].ToString()),
                                           DescuentoPor = double.Parse(row["DescuentoPor"].ToString()),
                                           DescuentoQtz = double.Parse(row["DescuentoQtz"].ToString()),
                                           DescuentoLpr = int.Parse(row["DescuentoLpr"].ToString()),
                                           Price = double.Parse(row["Price"].ToString()),
                                           LineTotal = double.Parse(row["LineTotal"].ToString()),
                                           Iva = double.Parse(row["Iva"].ToString()),
                                           CodigoPromo = int.Parse(row["CodigoPromo"].ToString()),
                                           Rw = int.Parse(row["Rw"].ToString())
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

        public static CarritoComprasPDFEntity getAllCotizacionesPendientesDetalle(int DocEntry, int SlpCode, int Tipo)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                CarritoComprasPDFEntity carritoComprasPDFEntity = new CarritoComprasPDFEntity();
                carritoComprasPDFEntity.Encabezado = new PortalListadoCotizacionesEntity();
                carritoComprasPDFEntity.Detalle = new List<PortalCotizacionesDetalleEntity>();

                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_autorizacion_cotizaciones_de_carrito_detalle_v4", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@DocEntry", DocEntry);
                iCommand.Parameters.AddWithValue("@SlpCode", SlpCode);
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
                        PortalListadoCotizacionesEntity encabezado = new PortalListadoCotizacionesEntity();
                        encabezado.NitDocto = dt.Rows[0]["NitDocto"].ToString();
                        encabezado.NitCardCode = dt.Rows[0]["NitCardCode"].ToString();
                        encabezado.CardCode = dt.Rows[0]["CardCode"].ToString();
                        encabezado.DocNum = int.Parse(dt.Rows[0]["DocNum"].ToString());
                        encabezado.DocDate = DateTime.Parse(dt.Rows[0]["DocDate"].ToString());
                        encabezado.DocDueDate = DateTime.Parse(dt.Rows[0]["DocDueDate"].ToString());
                        encabezado.CardName = dt.Rows[0]["FacNombre"].ToString();
                        encabezado.DocEntry = int.Parse(dt.Rows[0]["DocEntry"].ToString());
                        encabezado.Comments = dt.Rows[0]["Comments"].ToString();
                        encabezado.DocNumFac = int.Parse(dt.Rows[0]["DocNumFac"].ToString());
                        encabezado.Address = dt.Rows[0]["AddressDocto"].ToString();
                        encabezado.DireccionCardCode = dt.Rows[0]["DireccionCardCode"].ToString();
                        encabezado.CorreoCliente = dt.Rows[0]["CorreoCardCode"].ToString();
                        encabezado.TelefonoCliente = dt.Rows[0]["TelefonoCardCode"].ToString();
                        encabezado.CorreoDocto = dt.Rows[0]["CorreoDocto"].ToString();
                        encabezado.TelefonoDocto = dt.Rows[0]["TelefonoDocto"].ToString();
                        encabezado.Contacto = dt.Rows[0]["U_Contacto"].ToString();
                        encabezado.Correo = dt.Rows[0]["U_Correo"].ToString();
                        encabezado.Telefono = dt.Rows[0]["U_Telefono"].ToString();
                        encabezado.PriceList = int.Parse(dt.Rows[0]["PriceList"].ToString());
                        encabezado.Tipo = int.Parse(dt.Rows[0]["Tipo"].ToString());
                        encabezado.Estado = dt.Rows[0]["Estado"].ToString();
                        encabezado.WhsCode = dt.Rows[0]["WhsCode"].ToString();
                        encabezado.SlpCode = int.Parse(dt.Rows[0]["SlpCode"].ToString());

                        var listado = (from row in dt.AsEnumerable()
                                       select new PortalCotizacionesDetalleEntity()
                                       {
                                           LineNum = int.Parse(row["LineNum"].ToString()),
                                           ItemCode = row["ItemCode"].ToString(),
                                           Dscription = row["Dscription"].ToString(),
                                           Quantity = double.Parse(row["Quantity"].ToString()),
                                           Price = double.Parse(row["Price"].ToString()),
                                           Descuento = double.Parse(row["DiscPrcnt"].ToString()),
                                           LineTotal = double.Parse(row["LineTotal"].ToString()),
                                           PriceList = int.Parse(row["PriceList"].ToString()),
                                           DsctoPor = double.Parse(row["DsctoPor"].ToString()),
                                           DsctoNuevoPr = double.Parse(row["DsctoNuevoPr"].ToString()),
                                           DsctoListaPr = double.Parse(row["DsctoListaPr"].ToString()),
                                           DsctoQuetzal = double.Parse(row["DsctoQuetzal"].ToString()),
                                           Promocion = row["EsPromo"].ToString()
                                       }).ToList();
                        carritoComprasPDFEntity.Encabezado = encabezado;
                        carritoComprasPDFEntity.Detalle = listado;
                        carritoComprasPDFEntity.HistorialAuto = DALPortalCarritoCompras.getAllHistorialAutoriza(encabezado.DocEntry, encabezado.DocNum, Tipo);
                    }
                    return carritoComprasPDFEntity;
                }
                catch (Exception ex)
                {
                    return carritoComprasPDFEntity;
                }
            }
        }

        public static List<HistorialAutorizaciones> getAllHistorialAutoriza(int DocEntry, int DocNum, int Tipo)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_crm_historial_autorizaciones", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@DocEntry", DocEntry);
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
                        var listado = (from row in dt.AsEnumerable()
                                       select new HistorialAutorizaciones()
                                       {
                                           DocEntryNew = int.Parse(row["DocEntryNew"].ToString()),
                                           DocEntryOld = int.Parse(row["DocEntryOld"].ToString()),
                                           DocNumNew = int.Parse(row["DocNumNew"].ToString()),
                                           DocNumOld = int.Parse(row["DocNumOld"].ToString()),
                                           IdEstado = int.Parse(row["IdEstado"].ToString()),
                                           Estado = row["Estado"].ToString(),
                                           Usuario = row["Usuario"].ToString(),
                                           FechaHora = DateTime.Parse(row["FechaHora"].ToString()),
                                           Notas = row["Notas"].ToString()
                                       }).ToList();
                        return listado;
                    }
                    return new List<HistorialAutorizaciones>();
                }
                catch (Exception ex)
                {
                    return new List<HistorialAutorizaciones>();
                }
            }
        }

        public static List<PortalListadoCotizacionesEntity> getAllCotizacionesPendientes(int UserCode, int SlpCode)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_autorizacion_cotizaciones_de_carrito", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@UserCode", UserCode);
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
                                           WhsName = row["WhsName"].ToString(),
                                           DocDate = DateTime.Parse(row["DocDate"].ToString()),
                                           DocTotal = double.Parse(row["DocTotal"].ToString()),
                                           IsCookie = "N",
                                           EstadoCoti = row["TipoCoti"].ToString(),
                                           DscrTipoCoti = row["DscrTipoCoti"].ToString(),
                                           Estado = row["Estado"].ToString()
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
                        Enc.Address = dt.Rows[0]["Direccion"].ToString();
                        Enc.SlpName = dt.Rows[0]["SlpName"].ToString();
                        Enc.DocDate = DateTime.Parse(dt.Rows[0]["DocDate"].ToString());
                        Enc.DocDueDate = DateTime.Parse(dt.Rows[0]["DocDueDate"].ToString());
                        Enc.SubTotal = double.Parse(dt.Rows[0]["SubTotal"].ToString());
                        Enc.Impuesto = double.Parse(dt.Rows[0]["Iva"].ToString());
                        Enc.Descuento = double.Parse(dt.Rows[0]["Descuento"].ToString());
                        Enc.DocTotal = double.Parse(dt.Rows[0]["DocTotal"].ToString());
                        Enc.DireccionTejar = dt.Rows[0]["DireccionTejar"].ToString();
                        Enc.Notas = dt.Rows[0]["Notas"].ToString();
                        Enc.CorreoCliente = dt.Rows[0]["Correo"].ToString();
                        Enc.TelefonoCliente = dt.Rows[0]["Phone1"].ToString();
                        Enc.Hora = dt.Rows[0]["Hora"].ToString();


                        var listado = (from row in dt.AsEnumerable()
                                       select new PortalCotizacionesDetalleEntity()
                                       {
                                           ItemCode = row["ItemCode"].ToString(),
                                           Dscription = row["Dscription"].ToString(),
                                           Quantity = double.Parse(row["Quantity"].ToString()),
                                           Price = double.Parse(row["Price"].ToString()),
                                           LineTotal = double.Parse(row["LineTotal"].ToString()),                                           
                                           ImagenUrl = row["Imagen"].ToString()
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
        public static List<PortalListadoCotizacionesEntity> getAllCotizacionesVenta(int SlpCode, DateTime FechaI, DateTime FechaF, int TipoCrm, string WhsCode)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_cotizaciones_de_carrito", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@SlpCode", SlpCode);
                iCommand.Parameters.AddWithValue("@FechaI", FechaI);
                iCommand.Parameters.AddWithValue("@FechaF", FechaF);
                iCommand.Parameters.AddWithValue("@TipoCrm", TipoCrm);
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
                        string json = dt.Rows[0]["Dona"].ToString();
                        List<CrmGraficoDonaCotiEntity> listaDona = JsonConvert.DeserializeObject<List<CrmGraficoDonaCotiEntity>>(json);

                        var listado = (from row in dt.AsEnumerable()
                                       select new PortalListadoCotizacionesEntity()
                                       {
                                           DocEntry = int.Parse(row["DocEntry"].ToString()),
                                           DocNum = int.Parse(row["DocNum"].ToString()),
                                           DocNumFac = int.Parse(row["DocNumFac"].ToString()),
                                           Nit = row["Nit"].ToString(),
                                           FacNombre = row["FacNombre"].ToString(),
                                           CardCode = row["CardCode"].ToString(),
                                           CardName = row["CardName"].ToString(),
                                           SlpCode = int.Parse(row["SlpCode"].ToString()),
                                           DocDate = DateTime.Parse(row["DocDate"].ToString()),
                                           DocTotal = double.Parse(row["DocTotal"].ToString()),
                                           IsCookie = "N",
                                           EstadoCoti = row["TipoCoti"].ToString(),
                                           DscrTipoCoti = row["DscrTipoCoti"].ToString(),
                                           IdTipoCrm = int.Parse(row["IdEstadoCrm"].ToString()),
                                           DscrpTipoCrm = row["DescEstadoCrm"].ToString(),
                                           CountCotizaciones = int.Parse(row["CountCotizaciones"].ToString()),
                                           DocTotalCoti = double.Parse(row["DocTotalCoti"].ToString()),
                                           CountFacturas = int.Parse(row["CountFacturas"].ToString()),
                                           DocTotalFact = double.Parse(row["DocTotalFact"].ToString()),
                                           DatosGraficosDona = (dt.Rows.IndexOf(row) == 0) ? listaDona : null
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
