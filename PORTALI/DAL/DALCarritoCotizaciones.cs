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
    public class DALCarritoCotizaciones
    {
        public static EncabezadoVentaEditarEntity CotizacionCompraEncabezado(int DocEntry)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            EncabezadoVentaEditarEntity encabezadoVentaEditarEntity = new EncabezadoVentaEditarEntity();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_crm_editar_cotizacion_encabezado", iConnection);
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
                        encabezadoVentaEditarEntity.DocN = int.Parse(dt.Rows[0]["DocEntry"].ToString());
                        encabezadoVentaEditarEntity.Llave = int.Parse(dt.Rows[0]["DocEntry"].ToString());
                        encabezadoVentaEditarEntity.NoCotizacion = int.Parse(dt.Rows[0]["DocNum"].ToString());
                        encabezadoVentaEditarEntity.Cliente = dt.Rows[0]["Cliente"].ToString();
                        encabezadoVentaEditarEntity.LicTradNum = dt.Rows[0]["LicTradNum"].ToString();
                        encabezadoVentaEditarEntity.CardCode = dt.Rows[0]["CardCode"].ToString();
                        encabezadoVentaEditarEntity.CardName = dt.Rows[0]["CardName"].ToString();
                        encabezadoVentaEditarEntity.Direccion = dt.Rows[0]["Address"].ToString();
                        encabezadoVentaEditarEntity.Email = dt.Rows[0]["E_Mail"].ToString();
                        encabezadoVentaEditarEntity.Telefono = dt.Rows[0]["Phone1"].ToString();
                        encabezadoVentaEditarEntity.Fecha = Convert.ToDateTime(dt.Rows[0]["DocDate"].ToString()).ToString("yyyy-MM-dd");
                        encabezadoVentaEditarEntity.PriceId = int.Parse(dt.Rows[0]["PriceId"].ToString());
                        encabezadoVentaEditarEntity.FacturarNit = dt.Rows[0]["U_FacNit"].ToString();
                        encabezadoVentaEditarEntity.FacturarNombre = dt.Rows[0]["U_FacNom"].ToString();
                        encabezadoVentaEditarEntity.FacturarDireccion = dt.Rows[0]["Address"].ToString();
                        encabezadoVentaEditarEntity.EsCF = dt.Rows[0]["EsCF"].ToString();
                        encabezadoVentaEditarEntity.Borrador = dt.Rows[0]["Borrador"].ToString();

                        encabezadoVentaEditarEntity.productos = CotizacionCompraDetalle(DocEntry);

                        return encabezadoVentaEditarEntity;
                    }
                    return new EncabezadoVentaEditarEntity();
                }
                catch (Exception ex)
                {
                    return new EncabezadoVentaEditarEntity();
                }
            }
        }

        public static List<ProductoEditarEntity> CotizacionCompraDetalle(int DocEntry)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<ProductoEditarEntity> listado = new List<ProductoEditarEntity>();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_crm_editar_cotizacion_detalle", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@DocEntry", DocEntry);

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    listado = (from row in dt.AsEnumerable()
                               select new ProductoEditarEntity()
                               {
                                   Identity = "",
                                   DocE = int.Parse(row["DocEntry"].ToString()),
                                   ItemCode = row["ItemCode"].ToString(),
                                   Dscription = row["Dscription"].ToString(),
                                   Quantity = double.Parse(row["Quantity"].ToString()),
                                   LinkImg = row["U_ImagenUrl"].ToString(),
                                   LineTotal = double.Parse(row["LineTotal"].ToString()),
                                   LineN = int.Parse(row["LineNum"].ToString()),
                                   EsPromo = ""
                               }).ToList();
                    return listado;
                }
                catch (Exception ex)
                {
                    return new List<ProductoEditarEntity>();
                }
            }
        }
    }
}
