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
    public class DALPortalCotizaciones
    {
        public static List<ListaAsesoresEntity> getAllUsers(string WhsCode)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_listado_asesores_por_tienda", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@WhsCode", WhsCode);

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    var bono = (from row in dt.AsEnumerable()
                                select new ListaAsesoresEntity()
                                {
                                    SlpCode = int.Parse(row["SlpCode"].ToString()),
                                    SlpName = row["SlpName"].ToString()
                                }).ToList();
                    return bono;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        public static List<PortalTiendasEntity> getTiendas()
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_listado_tiendas_sac", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;                

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    var bono = (from row in dt.AsEnumerable()
                                select new PortalTiendasEntity()
                                {
                                    WhsCode = row["WhsCode"].ToString(),
                                    WhsName = row["WhsName"].ToString()
                                }).ToList();
                    return bono;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public static SimpleEntity ValidacionesCotizacionesCompras(int Depto, int DocEntry, int IdUser, int DocEntrySol)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            SimpleEntity simpleEntity = new SimpleEntity();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_portal_validaciones_matriz_autorizacion", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@Depto", Depto);
                iCommand.Parameters.AddWithValue("@Usuario", IdUser);
                iCommand.Parameters.AddWithValue("@DocEntryCoti", DocEntry);
                iCommand.Parameters.AddWithValue("@DocEntrySolC", DocEntrySol);

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        simpleEntity.Id = int.Parse(dt.Rows[0]["IdError"].ToString());
                        simpleEntity.Dscription = dt.Rows[0]["Mensaje"].ToString();
                        simpleEntity.Estado = dt.Rows[0]["Estado"].ToString();
                        simpleEntity.CodeAuto = int.Parse(dt.Rows[0]["CodeAuto"].ToString());
                        simpleEntity.LineAuto = int.Parse(dt.Rows[0]["LineAuto"].ToString());
                        return simpleEntity;
                    }

                    return simpleEntity;
                }
                catch (Exception ex)
                {
                    return simpleEntity;
                }
            }
        }

        //public static AutorizarCotizacionEntity ValidarTablaAutorizaciones(int Depto, int DocEntry, int IdUser, int DocEntrySol)
        //{
        //    ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
        //    AutorizarCotizacionEntity autorizarCotizacionEntity = new AutorizarCotizacionEntity();
        //    using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
        //    {
        //        OleDbCommand iCommand = null;
        //        iCommand = new OleDbCommand("sp_autorizar_cotizacion", iConnection);
        //        iCommand.CommandType = CommandType.StoredProcedure;
        //        iCommand.Parameters.AddWithValue("@Depto", Depto);
        //        iCommand.Parameters.AddWithValue("@DocEntry", DocEntry);
        //        iCommand.Parameters.AddWithValue("@IdUser", IdUser);
        //        iCommand.Parameters.AddWithValue("@DocEntrySol", DocEntrySol);

        //        try
        //        {
        //            OleDbDataAdapter iDAResult = null;
        //            DataTable dt = new DataTable();
        //            iDAResult = new OleDbDataAdapter();
        //            iDAResult.SelectCommand = iCommand;
        //            iDAResult.Fill(dt);

        //            if (dt.Rows.Count > 0)
        //            {
        //                autorizarCotizacionEntity.UserId1 = int.Parse(dt.Rows[0]["UserId1"].ToString());
        //                autorizarCotizacionEntity.Usuario1 = dt.Rows[0]["Usuario1"].ToString();
        //                autorizarCotizacionEntity.Email1 = dt.Rows[0]["Email1"].ToString();
        //                autorizarCotizacionEntity.UserId2 = int.Parse(dt.Rows[0]["UserId2"].ToString());
        //                autorizarCotizacionEntity.Usuario2 = dt.Rows[0]["Usuario2"].ToString();
        //                autorizarCotizacionEntity.Email2 = dt.Rows[0]["Email2"].ToString();
        //                autorizarCotizacionEntity.IdEstado = int.Parse(dt.Rows[0]["IdEstado"].ToString());
        //                autorizarCotizacionEntity.EstadoAuto = dt.Rows[0]["EstadoAuto"].ToString();
        //                autorizarCotizacionEntity.Code = int.Parse(dt.Rows[0]["Code"].ToString());
        //                autorizarCotizacionEntity.LineId = int.Parse(dt.Rows[0]["LineId"].ToString());
        //                return autorizarCotizacionEntity;
        //            }

        //            return autorizarCotizacionEntity;
        //        }
        //        catch (Exception ex)
        //        {
        //            return autorizarCotizacionEntity;
        //        }
        //    }
        //}
    }
}
