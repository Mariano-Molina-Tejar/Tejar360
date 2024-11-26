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
                iCommand = new OleDbCommand("sp_portal_listado_productos_margen", iConnection);
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
                                       Imagen = row["Imagen"].ToString()

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

        public static List<ReporteMargenEntity> ListadoMargenAsesores(string UserCode, string WhsCode)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<ReporteMargenEntity> listado = new List<ReporteMargenEntity>();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_margen_utilidad", iConnection);
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
                                                Facturas = int.Parse(row["Facturas"].ToString())
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
    }
}
