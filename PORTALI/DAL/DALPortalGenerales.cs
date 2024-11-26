using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DAL
{
    public class DALPortalGenerales
    {
        public static async Task EnviarCorreo(string subject, string ToCorreo, string Titulo, string _body, string attach, bool isHtml)
        {
            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
            msg.To.Add(ToCorreo);
            msg.From = new MailAddress("eltejarferreteria@gmail.com", Titulo, System.Text.Encoding.UTF8);
            msg.Subject = subject;
            msg.SubjectEncoding = System.Text.Encoding.UTF8;
            msg.Body = _body;
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            msg.IsBodyHtml = isHtml;
            if (attach != "")
            {
                msg.Attachments.Add(new System.Net.Mail.Attachment(attach));
            }

            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("eltejarferreteria@gmail.com", "htymvkzuxsgtrwej");

            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            try
            {
                client.Send(msg);                
            }
            catch (Exception ex)
            {
                
            }
        }

        public static List<PortalTiendasEntity> getAllTiendas(string WhsCode, string UserCode)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<PortalTiendasEntity> ListadoTiendas = new List<PortalTiendasEntity>();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_portal_get_tiendas_v2", iConnection);
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
                        ListadoTiendas = (from row in dt.AsEnumerable()
                                          select new PortalTiendasEntity()
                                          {
                                              WhsCode = row["WhsCode"].ToString(),
                                              WhsName = row["WhsName"].ToString(),
                                              Selected = (row["WhsCode"].ToString() == WhsCode ? "Selected" : "")
                                          }).ToList();
                        return ListadoTiendas;
                    }

                    return ListadoTiendas;
                }
                catch (Exception ex)
                {
                    return ListadoTiendas;
                }
            }
        }

        public static List<PortalTiendasEntity> getAlmacenes(int IdSucursal)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<PortalTiendasEntity> ListadoTiendas = new List<PortalTiendasEntity>();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_portal_get_tiendas", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@IdSucursal", IdSucursal);
                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        ListadoTiendas = (from row in dt.AsEnumerable()
                                       select new PortalTiendasEntity()
                                       {
                                           WhsCode = row["WhsCode"].ToString(),
                                           WhsName = row["WhsName"].ToString()
                                       }).ToList();
                        return ListadoTiendas;
                    }

                    return ListadoTiendas;
                }
                catch (Exception ex)
                {
                    return ListadoTiendas;
                }
            }
        }
        public static PortalPresupuestoEntity Presupuesto(int Depto)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            PortalPresupuestoEntity portalPresupuestoEntity = new PortalPresupuestoEntity();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_portal_presupuesto_general", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
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
                        portalPresupuestoEntity.Presupuesto = double.Parse(dt.Rows[0]["PresupuestoG"].ToString());
                        portalPresupuestoEntity.Gastos = double.Parse(dt.Rows[0]["Gastos"].ToString());
                        portalPresupuestoEntity.Saldo = double.Parse(dt.Rows[0]["Saldo"].ToString());
                        portalPresupuestoEntity.Indice = double.Parse(dt.Rows[0]["Indice"].ToString());
                    }

                    return portalPresupuestoEntity;
                }
                catch (Exception ex)
                {
                    return portalPresupuestoEntity;
                }
            }
        }
        public static double GetPresupuestoGeneral(int Depto)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            SystemDateTimeEntity systemDateTimeEntity = new SystemDateTimeEntity();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_portal_presupuesto_general", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
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
                        return double.Parse(dt.Rows[0]["Saldo"].ToString());
                    }

                    return 0.00;
                }
                catch (Exception ex)
                {
                    return 0.00;
                }
            }
        }
        public static SystemDateTimeEntity DateTimeSystem()
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            SystemDateTimeEntity systemDateTimeEntity = new SystemDateTimeEntity();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_fechas_y_horas_2", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        systemDateTimeEntity.FechaHoy = DateTime.Parse(dt.Rows[0]["FechaHoraSQL"].ToString()); //format: yyyy-MM-dd                        
                    }
                    return systemDateTimeEntity;
                }
                catch (Exception ex)
                {
                    return systemDateTimeEntity;
                }
            }
        }
        public static int DocNum(int BP, int TipoObjecto)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("SELECT TOP 1 a.NextNumber As DocNum FROM NNM1 a WHERE a.BPLId = " + BP + " AND ObjectCode = " + TipoObjecto, iConnection);
                iCommand.CommandType = CommandType.Text;
                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    if(dt.Rows.Count > 0) 
                    {
                        return int.Parse(dt.Rows[0]["DocNum"].ToString());
                    }
                    return 0;
                }
                catch (Exception ex)
                {
                    return -1;
                }
            }
        }
        public static List<PortalListadoDeptosEntity> ListadoDeptos(int Depto)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("SELECT Code,Name FROM OUDP", iConnection);
                iCommand.CommandType = CommandType.Text;
                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    var listado = (from row in dt.AsEnumerable()
                                   select new PortalListadoDeptosEntity()
                                   {
                                       IdDepto = int.Parse(row["Code"].ToString()),
                                       Depto = row["Name"].ToString(),
                                       Selected = (Depto == int.Parse(row["Code"].ToString()) ? "Selected" : "")
                                   }).ToList();
                    return listado;
                }
                catch (Exception ex)
                {
                    return new List<PortalListadoDeptosEntity>();
                }
            }
        }
        public static List<PortaSucursalesEntity> ListadoSucursales()
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("SELECT BPLId,BPLName FROM OBPL WHERE Disabled = 'N'", iConnection);
                iCommand.CommandType = CommandType.Text;
                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    var listado = (from row in dt.AsEnumerable()
                                select new PortaSucursalesEntity()
                                {
                                    IdSucursal = int.Parse(row["BPLId"].ToString()),
                                    Name = row["BPLName"].ToString()
                                }).ToList();
                    return listado;
                }
                catch (Exception ex)
                {
                    return new List<PortaSucursalesEntity>();
                }
            }
        }
        public static List<BusquedaDetalleProductoEntity> BusquedaDetalleProducto(string Dscription, int Depto, string CardCode = null)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_portal_buscar_producto", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@ItemName", Dscription);
                iCommand.Parameters.AddWithValue("@Depto", Depto);
                iCommand.Parameters.AddWithValue("@CardCode", (CardCode == "" ? null : CardCode));

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    var bono = (from row in dt.AsEnumerable()
                                select new BusquedaDetalleProductoEntity()
                                {
                                    ItemCode = row["ItemCode"].ToString(),
                                    Dscription = row["ItemName"].ToString(),
                                    Cuenta = row["Cuenta"].ToString(),
                                    NombreCuenta = row["AcctName"].ToString(),
                                    Prespuesto = double.Parse(row["Prespuesto"].ToString()),
                                    CardCode = row["CardCode"].ToString(),
                                    CardName = row["CardName"].ToString()

                                }).ToList();
                    return bono;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        public static List<MesesEntity> ListadoMeses()
        {
            List<MesesEntity> mesesEntity = new List<MesesEntity>();
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            try
            {
                mesesEntity.Add(new MesesEntity { IdMes = 1, Mes = "Enero" });
                mesesEntity.Add(new MesesEntity { IdMes = 2, Mes = "Febrero" });
                mesesEntity.Add(new MesesEntity { IdMes = 3, Mes = "Marzo" });
                mesesEntity.Add(new MesesEntity { IdMes = 4, Mes = "Abril" });
                mesesEntity.Add(new MesesEntity { IdMes = 5, Mes = "Mayo" });
                mesesEntity.Add(new MesesEntity { IdMes = 6, Mes = "Junio" });
                mesesEntity.Add(new MesesEntity { IdMes = 7, Mes = "Julio" });
                mesesEntity.Add(new MesesEntity { IdMes = 8, Mes = "Agosto" });
                mesesEntity.Add(new MesesEntity { IdMes = 9, Mes = "Septiembre" });
                mesesEntity.Add(new MesesEntity { IdMes = 10, Mes = "Octubre" });
                mesesEntity.Add(new MesesEntity { IdMes = 11, Mes = "Noviembre" });
                mesesEntity.Add(new MesesEntity { IdMes = 12, Mes = "Diciembre" });

                return mesesEntity;
            }
            catch (Exception ex)
            {
                return mesesEntity;
            }
        }
        public static List<PortalListadoAsesoresEntity> getAllAsesores(int SlpCode)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_portal_listado_asesores", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@SlpCode", SlpCode);

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    var bono = (from row in dt.AsEnumerable()
                                select new PortalListadoAsesoresEntity()
                                {
                                    SlpCode = int.Parse(row["SlpCode"].ToString()),
                                    SlpName = row["SlpName"].ToString(),
                                    Selected = (int.Parse(row["SlpCode"].ToString()) == SlpCode ? "Selected" : ""),
                                    Disabled = (int.Parse(row["SlpCode"].ToString()) == SlpCode ? "Disabled" : "")
                                }).ToList();
                    return bono;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        public static string ValidarExistenciaArchivo(string Url, string Name)
        {
            try 
            {                
                if (File.Exists(Url))
                {
                    return Name;
                }
                else 
                {
                    return "imagen-no-disponible.png";
                }
            }
            catch(Exception ex) 
            {
                return "imagen-no-disponible.png";
            }
        }
        public static List<PortalConsultaProductoEntity> ConsultaProducto(string ItemCode, string WhsCode)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_portal_consultar_producto", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@ItemCode", ItemCode);
                iCommand.Parameters.AddWithValue("@WhsCode", WhsCode);

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    var bono = (from row in dt.AsEnumerable()
                                select new PortalConsultaProductoEntity()
                                {
                                    ItemCode = row["ItemCode"].ToString(),
                                    ItemName = row["ItemName"].ToString(),
                                    Stock = double.Parse(row["OnHand"].ToString()),
                                    Metros = double.Parse(row["Stock"].ToString())
                                }).ToList();
                    return bono;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
    }
}
