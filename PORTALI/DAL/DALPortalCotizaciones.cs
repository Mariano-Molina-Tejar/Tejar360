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
        public static AutorizarCotizacionEntity ValidarTablaAutorizaciones(int Depto, int DocEntry, int IdUser, int DocEntrySol)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            AutorizarCotizacionEntity autorizarCotizacionEntity = new AutorizarCotizacionEntity();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_autorizar_cotizacion", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@Depto", Depto);
                iCommand.Parameters.AddWithValue("@DocEntry", DocEntry);
                iCommand.Parameters.AddWithValue("@IdUser", IdUser);
                iCommand.Parameters.AddWithValue("@DocEntrySol", DocEntrySol);

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        autorizarCotizacionEntity.UserId1 = int.Parse(dt.Rows[0]["UserId1"].ToString());
                        autorizarCotizacionEntity.Usuario1 = dt.Rows[0]["Usuario1"].ToString();
                        autorizarCotizacionEntity.Email1 = dt.Rows[0]["Email1"].ToString();
                        autorizarCotizacionEntity.UserId2 = int.Parse(dt.Rows[0]["UserId2"].ToString());
                        autorizarCotizacionEntity.Usuario2 = dt.Rows[0]["Usuario2"].ToString();
                        autorizarCotizacionEntity.Email2 = dt.Rows[0]["Email2"].ToString();
                        autorizarCotizacionEntity.IdEstado = int.Parse(dt.Rows[0]["IdEstado"].ToString());
                        autorizarCotizacionEntity.EstadoAuto = dt.Rows[0]["EstadoAuto"].ToString();
                        autorizarCotizacionEntity.Code = int.Parse(dt.Rows[0]["Code"].ToString());
                        autorizarCotizacionEntity.LineId = int.Parse(dt.Rows[0]["LineId"].ToString());
                        return autorizarCotizacionEntity;
                    }

                    return autorizarCotizacionEntity;
                }
                catch (Exception ex)
                {
                    return autorizarCotizacionEntity;
                }
            }
        }
    }
}
