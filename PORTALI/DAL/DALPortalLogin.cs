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
    public class DALPortalLogin
    {        
        public static bool ValidarPasswordOld(string User, string Password)
        {
            SessionLoginEntity sessionLoginEntity = new SessionLoginEntity();
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_portal_login", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@Usuario", User);
                iCommand.Parameters.AddWithValue("@Password", Password);

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        return true;
                    }

                    return false;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public static SessionLoginEntity SessionLogin(LoginEntity loginEntity, string path)
        {
            SessionLoginEntity sessionLoginEntity = new SessionLoginEntity();
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_portal_login", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@Usuario", loginEntity.Usuario);
                iCommand.Parameters.AddWithValue("@Password", loginEntity.Password);

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        sessionLoginEntity.UserId = int.Parse(dt.Rows[0]["UserId"].ToString());
                        sessionLoginEntity.Email = dt.Rows[0]["Email"].ToString();
                        sessionLoginEntity.UserName = dt.Rows[0]["UserName"].ToString();
                        sessionLoginEntity.SlpCode = int.Parse(dt.Rows[0]["SlpCode"].ToString());
                        sessionLoginEntity.SlpName = dt.Rows[0]["SlpName"].ToString();
                        sessionLoginEntity.UserCode = dt.Rows[0]["UserCode"].ToString();
                        sessionLoginEntity.WhsCode = dt.Rows[0]["WhsCode"].ToString();
                        sessionLoginEntity.Nivel = int.Parse(dt.Rows[0]["Nivel"].ToString());
                        sessionLoginEntity.Depto = int.Parse(dt.Rows[0]["Depto"].ToString());
                        sessionLoginEntity.DeptoName = dt.Rows[0]["DeptoName"].ToString();
                        sessionLoginEntity.CodeEmpleado = int.Parse(dt.Rows[0]["CodeEmpleado"].ToString());
                        sessionLoginEntity.UrlPic = DALPortalGenerales.ValidarExistenciaArchivo((path + dt.Rows[0]["Photo"].ToString()).ToString(), dt.Rows[0]["Photo"].ToString()).ToString();
                        sessionLoginEntity.Descto = double.Parse(dt.Rows[0]["Descto"].ToString());
                        sessionLoginEntity.ClienteCf = dt.Rows[0]["ClienteCf"].ToString();
                        sessionLoginEntity.Serie = int.Parse(dt.Rows[0]["Serie"].ToString());

                        return sessionLoginEntity;
                    }

                    return null;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        public static PortalPerfilEntity Profile(string UserCode, string path)
        {
            PortalPerfilEntity perfil = new PortalPerfilEntity();
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_portal_get_usuario_v2", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@UserCode", UserCode);

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        perfil.UserId = int.Parse(dt.Rows[0]["UserId"].ToString());
                        perfil.Email = dt.Rows[0]["Email"].ToString();
                        perfil.UserName = dt.Rows[0]["UserName"].ToString();
                        perfil.SlpCode = int.Parse(dt.Rows[0]["SlpCode"].ToString());
                        perfil.SlpName = dt.Rows[0]["SlpName"].ToString();
                        perfil.UserCode = dt.Rows[0]["UserCode"].ToString();
                        perfil.NombrePic = DALPortalGenerales.ValidarExistenciaArchivo((path + dt.Rows[0]["Photo"].ToString()).ToString(), dt.Rows[0]["Photo"].ToString()).ToString();
                    }

                    return perfil;
                }
                catch (Exception ex)
                {
                    perfil.NombrePic = "imagen-no-disponible.png";
                    return perfil;
                }
            }
        }

        public static bool GuardarDatosPerfil(PortalPerfilEntity portalPerfilEntity)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_portal_update_profile", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@Password", portalPerfilEntity.Password);
                iCommand.Parameters.AddWithValue("@UserCode", portalPerfilEntity.UserCode);
                iCommand.Parameters.AddWithValue("@Photo", portalPerfilEntity.NombrePic);

                try
                {
                    iConnection.Open();
                    Object iRetunValue = iCommand.ExecuteScalar();
                    iConnection.Close();
                    return true;
                }
                catch (Exception ex)
                {                    
                    return false;
                }
            }
        }
    }
}
