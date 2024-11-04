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
    public class DALPortalCotizacionCompra
    {
        public static CotizacionCompraEntity CotizacionCompraEncabezado(int DocEntryCot)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            CotizacionCompraEntity cotizacionCompraEntity = new CotizacionCompraEntity();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_portal_cotizaciones_compra_encabezado", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@DocEntryCot", DocEntryCot);                

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    if(dt.Rows.Count > 0)
                    {
                        cotizacionCompraEntity.DocEntrySolC = int.Parse(dt.Rows[0]["DocEntrySolC"].ToString());
                        cotizacionCompraEntity.DocNumSolC = int.Parse(dt.Rows[0]["DocNumSolC"].ToString());
                        cotizacionCompraEntity.DocEntry = int.Parse(dt.Rows[0]["DocEntry"].ToString());
                        cotizacionCompraEntity.DocNum = int.Parse(dt.Rows[0]["DocNum"].ToString());
                        cotizacionCompraEntity.CardCode = dt.Rows[0]["CardCode"].ToString();
                        cotizacionCompraEntity.CardName = dt.Rows[0]["CardName"].ToString();
                        cotizacionCompraEntity.Solicitante = dt.Rows[0]["Requester"].ToString();
                        cotizacionCompraEntity.Depto = int.Parse(dt.Rows[0]["Dept"].ToString());
                        cotizacionCompraEntity.SubTotal = double.Parse(dt.Rows[0]["SubTotal"].ToString());
                        cotizacionCompraEntity.Iva = double.Parse(dt.Rows[0]["Iva"].ToString());
                        cotizacionCompraEntity.Total = double.Parse(dt.Rows[0]["Total"].ToString());
                        cotizacionCompraEntity.Email = dt.Rows[0]["Email"].ToString();
                        cotizacionCompraEntity.FechaEntrega = DateTime.Parse(dt.Rows[0]["FechaEntrega"].ToString());
                        cotizacionCompraEntity.FormaPago = dt.Rows[0]["PymntGroup"].ToString();
                        cotizacionCompraEntity.DocNumCoti = int.Parse(dt.Rows[0]["DocNumCoti"].ToString());
                        cotizacionCompraEntity.ComentariosCoti = dt.Rows[0]["CommentsCoti"].ToString();
                        cotizacionCompraEntity.IdAutoriza = int.Parse(dt.Rows[0]["IdAutoriza"].ToString());

                        return cotizacionCompraEntity;
                    }
                    return new CotizacionCompraEntity();
                }
                catch (Exception ex)
                {
                    return new CotizacionCompraEntity();
                }
            }
        }

        public static List<DetalleCompraEntity> CotizacionCompraDetalle(int DocEntryCot)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<DetalleCompraEntity> detalle = new List<DetalleCompraEntity>();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_portal_cotizaciones_compra_detalle", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@DocEntryCot", DocEntryCot);

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
                                   select new DetalleCompraEntity()
                                   {
                                       DocEntry = int.Parse(row["DocEntry"].ToString()),
                                       DocNum = int.Parse(row["DocNum"].ToString()),
                                       ItemCode = row["ItemCode"].ToString(),
                                       Dscription = row["Dscription"].ToString(),
                                       LineNum = int.Parse(row["LineNum"].ToString()),
                                       Price = double.Parse(row["Price"].ToString()),
                                       LineTotal = double.Parse(row["LineTotal"].ToString()),
                                       Quantity = double.Parse(row["Quantity"].ToString()),
                                       LineText = row["LineText"].ToString(),
                                       NombreCuenta = row["CuentaName"].ToString()
                                   }).ToList();
                        return detalle;
                    }
                    return new List<DetalleCompraEntity>();
                }
                catch (Exception ex)
                {
                    return new List<DetalleCompraEntity>();
                }
            }
        }
    }
}
