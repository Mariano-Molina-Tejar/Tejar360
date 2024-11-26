using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Entity;
using Newtonsoft.Json;

namespace DAL
{
    public class DALPortalSolicitudCompra
    {
        public static bool ValidarContratosItems(string ItemsCode)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_portal_validar_contratos_items", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@ItemsCode", ItemsCode);
                iCommand.Parameters.AddWithValue("@Aplica", "N");

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        return false;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
        public static GetAllProductsEntity CargaExcelData(string DataExcelJson, int Depto)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_portal_buscar_producto_carga_excel", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@json", DataExcelJson);
                iCommand.Parameters.AddWithValue("@Depto", Depto);

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        GetAllProductsEntity data = new GetAllProductsEntity();

                        data.ListaUno = (from row in dt.AsEnumerable()
                                       select new BusquedaDetalleProductoEntity()
                                       {
                                           Rw = int.Parse(row["Rw"].ToString()),
                                           ItemCode = row["ItemCode"].ToString(),
                                           Dscription = row["Dscription"].ToString(),
                                           Cuenta = row["Cuenta"].ToString(),
                                           LineTotal = double.Parse(row["LineTotal"].ToString()),
                                           NombreCuenta = row["AcctName"].ToString(),
                                           NotasLine = row["LineNote"].ToString(),
                                           Prespuesto = double.Parse(row["Presupuesto"].ToString()),
                                           Price = double.Parse(row["Price"].ToString()),
                                           Quantity = double.Parse(row["Quantity"].ToString()),
                                           UrlImg = "",
                                           WhsCode = "",
                                           Stock = 0
                                       }).ToList();


                        data.ListaDos = (from row in dt.AsEnumerable()
                                         select new
                                         {
                                             Cuenta = row["Cuenta"].ToString(),
                                             Nombre = row["AcctName"].ToString(),
                                             Presupuesto = double.Parse(row["Presupuesto"].ToString()),
                                             Gasto = double.Parse(row["TotalGasto"].ToString()),
                                             Saldo = double.Parse(row["Saldo"].ToString())
                                         }).Distinct()
                                         .Select(x => new ResumenPresupuestoEntity()
                                         {
                                             Cuenta = x.Cuenta,
                                             Nombre = x.Nombre,
                                             Presupuesto = x.Presupuesto,
                                             Gasto = x.Gasto,
                                             Saldo = x.Saldo
                                         }).ToList();


                        return data;
                    }
                    return new GetAllProductsEntity();
                }
                catch (Exception ex)
                {
                    return new GetAllProductsEntity();
                }
            }
        }
        public static List<SolicitudCompraDetalleEntity> EditarSolicitudDetalle(int DocEntry)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_portal_listado_solicitudes_detalle_edit", iConnection);
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
                        var listado = (from row in dt.AsEnumerable()
                                       select new SolicitudCompraDetalleEntity()
                                       {                                           
                                           ItemCode = row["ItemCode"].ToString(),
                                           LineNum = int.Parse(row["LineNum"].ToString()),
                                           Dscription = row["Dscription"].ToString(),
                                           Quantity = double.Parse(row["Quantity"].ToString()),
                                           WhsCode = row["WhsCode"].ToString(),
                                           NotasLine = row["NotasLine"].ToString()

                                       }).ToList();
                        return listado;
                    }
                    return new List<SolicitudCompraDetalleEntity>();
                }
                catch (Exception ex)
                {
                    return new List<SolicitudCompraDetalleEntity>();
                }
            }
        }
        public static SolicitudCompraEncabezadoEntity EditarSolicitud(string UserCode, int DocEntry)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_portal_listado_solicitudes", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@UserCode", UserCode);
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
                        return new SolicitudCompraEncabezadoEntity
                        {
                            DocEntry = int.Parse(dt.Rows[0]["DocEntry"].ToString()),
                            DocNum = int.Parse(dt.Rows[0]["DocNum"].ToString()),
                            IdDepto = int.Parse(dt.Rows[0]["Department"].ToString()),
                            DocDate = DateTime.Parse(dt.Rows[0]["DocDate"].ToString()),
                            DocDueDate = DateTime.Parse(dt.Rows[0]["DocDueDate"].ToString()),
                            IdSucursal = int.Parse(dt.Rows[0]["BPLId"].ToString()),
                            Observaciones = dt.Rows[0]["Comments"].ToString()
                        };
                    }
                    return new SolicitudCompraEncabezadoEntity();
                }
                catch (Exception ex)
                {
                    return new SolicitudCompraEncabezadoEntity();
                }
            }
        }
        public static List<DetalleCompraEntity> DetalleSolicitudCompra(int DocEntry, int Depto)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_portal_solicitud_compra_detalle", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@DocEntry", DocEntry);
                iCommand.Parameters.AddWithValue("@Depto", Depto);

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
                                       select new DetalleCompraEntity()
                                       {
                                           DocNum = int.Parse(row["DocNum"].ToString()),
                                           ItemCode = row["ItemCode"].ToString(),
                                           Dscription = row["Dscription"].ToString(),
                                           LineTotal = double.Parse(row["LineTotal"].ToString()),
                                           Price = double.Parse(row["Price"].ToString()),
                                           Quantity = double.Parse(row["Quantity"].ToString()),
                                           LineText = row["LineText"].ToString()
                                       }).ToList();
                        return listado;
                    }
                    return new List<DetalleCompraEntity>();
                }
                catch (Exception ex)
                {
                    return new List<DetalleCompraEntity>();
                }
            }
        }

        public static List<PortalCotizacionDetalleEntity> PortalCotizacionCompraDetalle(int DocEntry)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_portal_listado_cotizaciones_detalle_productos", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@DocEntry", DocEntry);

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    if(dt.Rows.Count > 0) 
                    {
                        var listado = (from row in dt.AsEnumerable()
                                       select new PortalCotizacionDetalleEntity()
                                       {
                                           DocEntrySolC = int.Parse(row["DocEntrySolC"].ToString()),
                                           DocNumSolC = int.Parse(row["DocNumSolC"].ToString()),
                                           DocEntry = int.Parse(row["DocEntry"].ToString()),
                                           DocNum = int.Parse(row["DocNum"].ToString()),
                                           ItemCode = row["ItemCode"].ToString(),
                                           Dscription = row["Dscription"].ToString(),
                                           LineNum = int.Parse(row["LineNum"].ToString()),
                                           LineText = row["LineText"].ToString(),
                                           LineTotal = double.Parse(row["LineTotal"].ToString()),
                                           Price = double.Parse(row["Price"].ToString()),
                                           Quantity = double.Parse(row["Quantity"].ToString())
                                       }).ToList();
                        return listado;
                    }
                    return new List<PortalCotizacionDetalleEntity>();
                }
                catch (Exception ex)
                {
                    return new List<PortalCotizacionDetalleEntity>();
                }
            }
        }
        public static List<PortalTotalCotizacionesEntity> TotalCotizacionesSolicitudCompra2(int DocEntry, string Depto)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_portal_listado_cotizaciones", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@DocEntry", DocEntry);
                iCommand.Parameters.AddWithValue("@Depto", Depto);

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    if (dt.Rows.Count > 1)
                    {
                        var listado = (from row in dt.AsEnumerable()
                                       select new PortalTotalCotizacionesEntity()
                                       {
                                           DocEntrySolC = int.Parse(row["DocEntrySolC"].ToString()),
                                           DocNumSolC = int.Parse(row["DocNumSolC"].ToString()),
                                           DocEntry = int.Parse(row["DocEntry"].ToString()),
                                           DocNum = int.Parse(row["DocNum"].ToString()),
                                           CardCode = row["CardCode"].ToString(),
                                           CardName = row["CardName"].ToString(),
                                           Solicitante = row["Requester"].ToString(),
                                           Depto = int.Parse(row["Dept"].ToString()),
                                           SubTotal = double.Parse(row["SubTotal"].ToString()),
                                           Iva = double.Parse(row["Iva"].ToString()),
                                           Total = double.Parse(row["Total"].ToString()),
                                           Email = row["Email"].ToString(),
                                           FechaEntrega = DateTime.Parse(row["FechaEntrega"].ToString()),
                                           FormaPago = row["PymntGroup"].ToString()
                                       }).ToList();
                        return listado;
                    }
                    return new List<PortalTotalCotizacionesEntity>();
                }
                catch (Exception ex)
                {
                    return new List<PortalTotalCotizacionesEntity>();
                }
            }
        }
        public static List<DetallePresupuestoEntity> DetallePresupuesto(int DocEntry)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_detalle_presupuesto", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@DocEntryCoti", DocEntry);

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
                                       select new DetallePresupuestoEntity()
                                       {
                                           AcctCode = row["AcctCode"].ToString(),
                                           AcctName = row["AcctName"].ToString(),
                                           Presupuesto = double.Parse(row["Presupuesto"].ToString()),
                                           Gastado = double.Parse(row["Gastado"].ToString()),
                                           Saldo = double.Parse(row["Saldo"].ToString()),
                                           GastoActual = double.Parse(row["GastoAhora"].ToString()),
                                           SaldoActual = double.Parse(row["SaldoCoti"].ToString())
                                       }).ToList();
                        return listado;
                    }
                    return new List<DetallePresupuestoEntity>();
                }
                catch (Exception ex)
                {
                    return new List<DetallePresupuestoEntity>();
                }
            }
        }

        public static List<PortalTotalCotizacionesEntity> TotalCotizacionesSolicitudCompra(int DocEntry, string Depto)
        {   
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_portal_listado_cotizaciones", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@DocEntry", DocEntry);
                iCommand.Parameters.AddWithValue("@Depto", Depto);

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);
                    
                    if(dt.Rows.Count > 0) 
                    {
                        var listado = (from row in dt.AsEnumerable()
                                       select new PortalTotalCotizacionesEntity()
                                       {
                                           DocEntrySolC = int.Parse(row["DocEntrySolC"].ToString()),
                                           DocNumSolC = int.Parse(row["DocNumSolC"].ToString()),
                                           DocEntry = int.Parse(row["DocEntry"].ToString()),
                                           DocNum = int.Parse(row["DocNum"].ToString()),
                                           CardCode = row["CardCode"].ToString(),
                                           CardName = row["CardName"].ToString(),
                                           Solicitante = row["Requester"].ToString(),
                                           Depto = int.Parse(row["Dept"].ToString()),
                                           SubTotal = double.Parse(row["SubTotal"].ToString()),
                                           Iva = double.Parse(row["Iva"].ToString()),
                                           Total = double.Parse(row["Total"].ToString()),
                                           Email = row["Email"].ToString(),
                                           FechaEntrega = DateTime.Parse(row["FechaEntrega"].ToString()),
                                           FormaPago = row["PymntGroup"].ToString(),
                                           DocEntryCoti = int.Parse(row["DocEntryCoti"].ToString()),
                                           IdAutoriza = int.Parse(row["IdAutoriza"].ToString())
                                       }).ToList();
                        return listado;
                    }
                    return new List<PortalTotalCotizacionesEntity>();
                }
                catch (Exception ex)
                {
                    return new List<PortalTotalCotizacionesEntity>();
                }
            }
        }
        public static List<PortalSolicitudDetalleEntity> DetalleProductosSolicitudCompra(int DocEntry)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_portal_listado_solicitudes_detalle_v2", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@DocEntry", DocEntry);

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    if(dt.Rows.Count > 0)
                    {
                        var listado = (from row in dt.AsEnumerable()
                                       select new PortalSolicitudDetalleEntity()
                                       {
                                           ItemCode = row["ItemCode"].ToString(),
                                           Dscription = row["Dscription"].ToString(),
                                           LineTotal = int.Parse(row["LineNum"].ToString()),
                                           Price = double.Parse(row["Price"].ToString()),
                                           Quantity = double.Parse(row["LineTotal"].ToString()),
                                           NotasLinea = row["LineText"].ToString(),
                                           IdEstado = row["IdEstado"].ToString()
                                       }).ToList();
                        return listado;
                    }
                    return new List<PortalSolicitudDetalleEntity>();
                }
                catch (Exception ex)
                {
                    return new List< PortalSolicitudDetalleEntity>();  
                }
            }
        }
        public static List<PortalSolicitudEncabezadoEntity> DetalleSolicitudCompra(string UserCode, int Tipo)
        {
            List<PortalSolicitudEncabezadoEntity> listado = new List<PortalSolicitudEncabezadoEntity>();
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {                
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_portal_listado_solicitudes_detalle", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("UserCode", UserCode);
                iCommand.Parameters.AddWithValue("Tipo", Tipo);

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        listado = JsonConvert.DeserializeObject<List<PortalSolicitudEncabezadoEntity>>(dt.Rows[0]["Items"].ToString());
                    }

                    return listado;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex.InnerException);
                }
            }
        }
        public static List<PortalSolicitudEncabezadoEntity> EncabezadoSolicitudCompra(int Depto, string Estado, string Usuario, int DocNum)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_portal_listado_solicitudes_v2", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@Depto", Depto);
                iCommand.Parameters.AddWithValue("@Estado", Estado);
                iCommand.Parameters.AddWithValue("@Usuario", Usuario);
                iCommand.Parameters.AddWithValue("@DocNum", DocNum);

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    var listado = (from row in dt.AsEnumerable()
                                   select new PortalSolicitudEncabezadoEntity()
                                   {
                                       Rw = int.Parse(row["Rw"].ToString()),
                                       CardCode = row["CardCode"].ToString(),
                                       CardName = row["CardName"].ToString(),
                                       DocDate = DateTime.Parse(row["DocDAteDraft"].ToString()),
                                       DocDueDate = DateTime.Parse(row["DocDueDate"].ToString()),
                                       DocEntry = int.Parse(row["DocEntryDraft"].ToString()),
                                       DocNum = int.Parse(row["DocNumDraft"].ToString()),
                                       DocNumOc = int.Parse(row["DocNumOc"].ToString()),
                                       Estado = row["Estado"].ToString(),
                                       ReqDate = DateTime.Parse(row["ReqDate"].ToString()),
                                       UserCode = row["Requester"].ToString(),
                                       UserName = row["Usuario"].ToString(),
                                       WhsCode = row["WhsCode"].ToString(),
                                       IdSucursal = int.Parse(row["BPLId"].ToString()),
                                       Depto = int.Parse(row["Department"].ToString()),
                                       IdEstado = row["IdEstado"].ToString(),
                                       ColorEstado = row["ColorEstado"].ToString(),
                                       Comentarios = row["Comments"].ToString()
                                   }).ToList();
                    return listado;                    
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        public static string CrearSolicitud(string Url, Object objecto)
        {
            try
            {
                return Connection.Conexion.ConsumirAPI(Url, objecto, null);
            }
            catch (Exception)
            {

                return "";
            }
        }
        public static string NewSolicitud(string Url, Object objecto, string Parameters)
        {
            try
            {
                return Connection.Conexion.ConsumirAPI(Url, objecto, Parameters);
            }
            catch (Exception ex)
            {

                return "";
            }
        }
    }
}
