using Connection;
using Dapper;
using Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DAL
{
    public class DALPortalGenerales
    {
        public static List<TicketAnualEntity> TicketsMensual()
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<TicketAnualEntity> listaTiposCrm = new List<TicketAnualEntity>();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_ticket_mensual", iConnection);
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
                        var listaTickets = (from row in dt.AsEnumerable()
                                            select new TicketAnualEntity
                                            {
                                                Anio = Convert.ToInt32(row["Anio"].ToString()),
                                                Mes = Convert.ToInt32(row["Mes"].ToString()),
                                                PromedioMinutos = Convert.ToDouble(row["PromedioMinutos"].ToString()),
                                                TotalTickets = Convert.ToInt32(row["TotalTickets"].ToString()),
                                                PromedioFormateado = row["PromedioFormateado"].ToString()
                                            }).ToList();

                        return listaTickets;
                    }

                    return listaTiposCrm;
                }
                catch (Exception ex)
                {
                    return listaTiposCrm;
                }
            }
        }
        public static List<TicketEntity> ObtenerTickets(DateTime? FechaI, DateTime? FechaF)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<TicketEntity> listaTiposCrm = new List<TicketEntity>();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_tickets", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@FechaI", FechaI);
                iCommand.Parameters.AddWithValue("@FechaF", FechaF);
                

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        var listaTickets = (from row in dt.AsEnumerable()
                                            select new TicketEntity
                                            {
                                                Id = Convert.ToInt32(row["Id"]),
                                                Titulo = Convert.ToString(row["Titulo"]),
                                                Asignado = Convert.ToString(row["Asignado"]),
                                                Creador = Convert.ToString(row["Creador"]),
                                                Prioridad = Convert.ToString(row["Prioridad"]),
                                                Categoria = Convert.ToString(row["Categoria"]),
                                                Estado = Convert.ToString(row["Estado"]),
                                                FechaCreacion = Convert.ToDateTime(row["FechaCreacion"]),
                                                FechaCierre = Convert.ToDateTime(row["FechaCierre"]),
                                                PrimeraRespuestaMinutos = Convert.ToString(row["PrimeraRespuestaMinutos"]),
                                                TiempoCierreMinutos = Convert.ToString(row["TiempoCierreMinutos"])

                                            }).ToList();

                        return listaTickets;
                    }

                    return listaTiposCrm;
                }
                catch (Exception ex)
                {
                    return listaTiposCrm;
                }
            }
        }

        public static List<ZonasEntity> getAllZonas()
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<ZonasEntity> listaTiposCrm = new List<ZonasEntity>();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("SELECT Code As IdZona, Name As Zona FROM [ELTEJAR_PRUEBAS_R1_5.1].dbo.[@CRM_ZONAS]", iConnection);
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
                        listaTiposCrm = (from row in dt.AsEnumerable()
                                         select new ZonasEntity()
                                         {
                                             Id = int.Parse(row["IdZona"].ToString()),
                                             Nombre = row["Zona"].ToString()
                                         }).ToList();
                        return listaTiposCrm;
                    }

                    return listaTiposCrm;
                }
                catch (Exception ex)
                {
                    return listaTiposCrm;
                }
            }
        }
        public static List<MunicipiosEntity> getAllMunicipios(int IdDepto)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<MunicipiosEntity> listaTiposCrm = new List<MunicipiosEntity>();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_crm_municipios_gt", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@IdDepto", IdDepto);

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        listaTiposCrm = (from row in dt.AsEnumerable()
                                         select new MunicipiosEntity()
                                         {
                                             Id = int.Parse(row["IdMunicipio"].ToString()),
                                             Nombre = row["Municipio"].ToString()
                                         }).ToList();
                        return listaTiposCrm;
                    }

                    return listaTiposCrm;
                }
                catch (Exception ex)
                {
                    return listaTiposCrm;
                }
            }
        }
        public static List<DepartamentosGtEntity> getAllDepartamentosgt(int IdRegion, string WhsCode)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<DepartamentosGtEntity> listaTiposCrm = new List<DepartamentosGtEntity>();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_crm_deptos_gt", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@IdRegion", IdRegion);
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
                        listaTiposCrm = (from row in dt.AsEnumerable()
                                         select new DepartamentosGtEntity()
                                         {
                                             Id = int.Parse(row["IdDepto"].ToString()),
                                             Nombre = row["Departamento"].ToString()
                                         }).ToList();
                        return listaTiposCrm;
                    }

                    return listaTiposCrm;
                }
                catch (Exception ex)
                {
                    return listaTiposCrm;
                }
            }
        }
        public static List<SemanasEntity> getAllSemanas(int Tipo, int Anio)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<SemanasEntity> listaTiposCrm = new List<SemanasEntity>();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_crm_fechas_bolsonv2", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@Tipo", Tipo);
                iCommand.Parameters.AddWithValue("@Anio", Anio);

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        listaTiposCrm = (from row in dt.AsEnumerable()
                                         select new SemanasEntity()
                                         {
                                             Semana = int.Parse(row["Semana"].ToString()),
                                             FechaInicio = DateTime.Parse(row["FechaInicio"].ToString()),
                                             FechaFinal = DateTime.Parse(row["FechaFin"].ToString()),
                                             Texto = row["Texto"].ToString()
                                         }).ToList();
                        return listaTiposCrm;
                    }

                    return listaTiposCrm;
                }
                catch (Exception ex)
                {
                    return listaTiposCrm;
                }
            }
        }
        public static List<AnioEntity> GetAllAnios(int Tipo)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<AnioEntity> listaTiposCrm = new List<AnioEntity>();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_crm_fechas_bolsonv2", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@Tipo", Tipo);
                iCommand.Parameters.AddWithValue("@Anio", 2025);

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        listaTiposCrm = (from row in dt.AsEnumerable()
                                         select new AnioEntity()
                                         {
                                             Anio = int.Parse(row["Anio"].ToString())
                                         }).ToList();
                        return listaTiposCrm;
                    }

                    return listaTiposCrm;
                }
                catch (Exception ex)
                {
                    return listaTiposCrm;
                }
            }
        }
        public static List<AccionesEntity> GetAllAcciones()
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<AccionesEntity> listaTiposCrm = new List<AccionesEntity>();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("SELECT * FROM [@CRM_ACCIONES] WHERE U_Estado = 'Y'", iConnection);
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
                        listaTiposCrm = (from row in dt.AsEnumerable()
                                         select new AccionesEntity()
                                         {
                                             Codigo = int.Parse(row["Code"].ToString()),
                                             Nombre = row["Name"].ToString(),
                                             Icono = row["U_Icono"].ToString()
                                         }).ToList();
                        return listaTiposCrm;
                    }

                    return listaTiposCrm;
                }
                catch (Exception ex)
                {
                    return listaTiposCrm;
                }
            }
        }

        public static List<PortalListadoGeneralEntity> getAllTiposPerdidasCrm()
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<PortalListadoGeneralEntity> listaTiposCrm = new List<PortalListadoGeneralEntity>();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("SELECT * FROM [@CRM_SEGUPERD]", iConnection);
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
                        listaTiposCrm = (from row in dt.AsEnumerable()
                                         select new PortalListadoGeneralEntity()
                                         {
                                             Id = row["Code"].ToString(),
                                             Dscription = row["Name"].ToString().ToUpper()
                                         }).ToList();
                        return listaTiposCrm;
                    }

                    return listaTiposCrm;
                }
                catch (Exception ex)
                {
                    return listaTiposCrm;
                }
            }
        }

        public static List<PortalListadoGeneralEntity> getAllTiposContactoCrm()
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<PortalListadoGeneralEntity> listaTiposCrm = new List<PortalListadoGeneralEntity>();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("SELECT * FROM [@CRM_TIPOCONT]", iConnection);
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
                        listaTiposCrm = (from row in dt.AsEnumerable()
                                         select new PortalListadoGeneralEntity()
                                         {
                                             Id = row["Code"].ToString(),
                                             Dscription = row["Name"].ToString()
                                         }).ToList();
                        return listaTiposCrm;
                    }

                    return listaTiposCrm;
                }
                catch (Exception ex)
                {
                    return listaTiposCrm;
                }
            }
        }
        public static List<PortalListadoGeneralEntity> getAllTiposCrm()
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<PortalListadoGeneralEntity> listaTiposCrm = new List<PortalListadoGeneralEntity>();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("SELECT FldValue As Codigo,Descr As Descripcion FROM UFD1 WHERE TableID = 'OQUT' AND FieldId = '111'", iConnection);
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
                        listaTiposCrm = (from row in dt.AsEnumerable()
                                         select new PortalListadoGeneralEntity()
                                         {
                                             Id = row["Codigo"].ToString(),
                                             Dscription = row["Descripcion"].ToString()
                                         }).ToList();
                        return listaTiposCrm;
                    }

                    return listaTiposCrm;
                }
                catch (Exception ex)
                {
                    return listaTiposCrm;
                }
            }
        }

        public static DatosServerEntity getDatosServer()
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            DatosServerEntity datosServerEntity = new DatosServerEntity();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_datos_servidor", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    if(dt.Rows.Count > 0) 
                    {
                        datosServerEntity.FechaHoy = DateTime.Parse(dt.Rows[0]["FechaHoy"].ToString());
                        datosServerEntity.FechaInicioMes = DateTime.Parse(dt.Rows[0]["FechaInicioMes"].ToString());
                        datosServerEntity.FechaFinalMes = DateTime.Parse(dt.Rows[0]["FechaFinalMes"].ToString());
                        datosServerEntity.Hora = dt.Rows[0]["Hora"].ToString();
                    }
                    return datosServerEntity;
                }
                catch (Exception ex)
                {
                    return new DatosServerEntity();
                }
            }
        }

        public static string BuscarDPI(string cui)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Obtener el token
                    string token = GetTokenSAT();

                    // Verificar si se obtuvo el token
                    if (string.IsNullOrEmpty(token) || token.StartsWith("Error"))
                    {
                        return "Error al obtener el token.";
                    }

                    // URL de la API
                    string url = "https://certificador.feel.com.gt/api/v2/servicios/externos/cui";

                    // Agregar el token en el encabezado de la solicitud
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                    // Datos que se enviarán en el cuerpo de la solicitud
                    var data = new { cui = cui };
                    string jsonData = JsonConvert.SerializeObject(data);
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    // Realizar la solicitud POST de manera síncrona
                    HttpResponseMessage response = client.PostAsync(url, content).Result;

                    // Verificar si la solicitud fue exitosa
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = response.Content.ReadAsStringAsync().Result;
                        dynamic jsonResponse = JsonConvert.DeserializeObject(responseBody);

                        // Retornar solo el nombre
                        return jsonResponse.cui.nombre;
                    }
                    else
                    {
                        return $"Error: {response.StatusCode}";
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
        public static string GetTokenSAT()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // URL de la API
                    string url = "https://certificador.feel.com.gt/api/v2/servicios/externos/login";

                    // Crear el contenido form-data
                    var formData = new MultipartFormDataContent
                    {
                        { new StringContent("TEJAR_TIVOLI"), "prefijo" },
                        { new StringContent("3186EF8FCCAF78B315CD9B7B8DD9AA23"), "llave" }
                    };

                    // Realizar la solicitud POST de manera síncrona
                    HttpResponseMessage response = client.PostAsync(url, formData).Result;

                    // Verificar si la solicitud fue exitosa
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = response.Content.ReadAsStringAsync().Result;
                        dynamic jsonResponse = JsonConvert.DeserializeObject(responseBody);

                        // Retornar solo el token
                        return jsonResponse.token;
                    }
                    else
                    {
                        return $"Error: {response.StatusCode}";
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }


        public static ClienteSATEntity ConsultarNIT(string nit)
        {
            // URL del servicio de la SAT
            string url = ConfigurationManager.AppSettings["SAT_URL"].ToString();

            // Crear el objeto con los datos que se enviarán en la solicitud
            var requestData = new
            {
                emisor_codigo = ConfigurationManager.AppSettings["SAT_CODIGO_EMISOR"].ToString(),
                emisor_clave = ConfigurationManager.AppSettings["SAT_LLAVE"].ToString(),
                nit_consulta = nit
            };

            // Convertir el objeto a JSON
            string jsonData = JsonConvert.SerializeObject(requestData);

            // Crear una solicitud HTTP
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";

            try
            {
                // Escribir el JSON en el cuerpo de la solicitud
                using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
                {
                    writer.Write(jsonData);
                    writer.Flush();
                    writer.Close();
                }

                // Obtener la respuesta del servidor
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    // Leer la respuesta del servicio web
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string jsonResponse = reader.ReadToEnd();

                        // Deserializar la respuesta JSON en un objeto ClienteSATEntity
                        ClienteSATEntity cliente = JsonConvert.DeserializeObject<ClienteSATEntity>(jsonResponse);

                        // Retornar los datos deserializados
                        return cliente;
                    }
                }
            }
            catch (WebException ex)
            {
                // Manejo de errores si la consulta falla
                if (ex.Response != null)
                {
                    using (StreamReader reader = new StreamReader(ex.Response.GetResponseStream()))
                    {
                        string errorResponse = reader.ReadToEnd();
                        Console.WriteLine("Error al realizar la consulta: " + errorResponse);
                    }
                }
                else
                {
                    Console.WriteLine("Error al realizar la consulta: " + ex.Message);
                }
                return null;
            }
            catch (Exception ex)
            {
                // Manejo de excepciones generales
                Console.WriteLine("Error inesperado: " + ex.Message);
                return null;
            }
        }
        public static PortalPdfOrdenCompraEntity DataPdfOrdenCompra(int DocEntry)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            PortalPdfOrdenCompraEntity DataOc = new PortalPdfOrdenCompraEntity();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("RPT_ORDEN_COMPRA", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@TransId", DocEntry);

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        DataOc.CardCode = dt.Rows[0]["Codigo"].ToString();
                        DataOc.NitSucursal = dt.Rows[0]["NITSUCURSAL"].ToString();
                        DataOc.CardName = dt.Rows[0]["NombreProveedor"].ToString();
                        DataOc.FechaEmision = DateTime.Parse(dt.Rows[0]["FechaContabilizacion"].ToString());
                        DataOc.FechaEntrega = DateTime.Parse(dt.Rows[0]["FechaEntrega"].ToString());
                        DataOc.DocNum = int.Parse(dt.Rows[0]["NoOrdenCompra"].ToString());
                        DataOc.Nit = dt.Rows[0]["Nit"].ToString();
                        DataOc.Email = dt.Rows[0]["CorreoElectronico"].ToString();
                        DataOc.NombreGrupo = dt.Rows[0]["NombreGrupo"].ToString();
                        DataOc.Sucursal = dt.Rows[0]["SUCURSAL"].ToString();
                        DataOc.DirEntrega = dt.Rows[0]["DIRENTREGA"].ToString();
                        DataOc.Comentario = dt.Rows[0]["Comentario"].ToString();
                        DataOc.GranTotal = double.Parse(dt.Rows[0]["TotalDocumento"].ToString());
                        DataOc.ElaboradoPor = dt.Rows[0]["Usuario"].ToString();
                        DataOc.Telefono = dt.Rows[0]["Telefono1"].ToString() + " / " + dt.Rows[0]["Telefono2"].ToString();                        

                        DataOc.Detalle = (from row in dt.AsEnumerable()
                                          select new PortalPdfOrdenCompraDetalleEntity()
                                          {
                                              ItemCode = row["Codigo"].ToString(),
                                              Dscription = row["Descripcion"].ToString(),
                                              Umedida = row["UnidadMedida"].ToString(),
                                              Almacen = row["Almacen"].ToString(),
                                              Quantity = double.Parse(row["Cantidad"].ToString()),
                                              Price = double.Parse(row["Precio"].ToString()),
                                              LineTotal = double.Parse(row["TotalConIVA"].ToString())
                                          }).ToList();
                        return DataOc;
                    }

                    return DataOc;
                }
                catch (Exception ex)
                {
                    return DataOc;
                }
            }
        }
        public static List<SocioNegocioEntity> ListadoSocioNegocios(string SocioNegocio)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("SELECT UPPER(CardCode) As CardCode,UPPER(CardName) As CardName FROM OCRD WHERE CardType = 'S' AND CardName LIKE '%" + SocioNegocio + "%' ORDER BY CardName", iConnection);
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
                        var listado = (from row in dt.AsEnumerable()
                                       select new SocioNegocioEntity()
                                       {
                                           CardCode = row["CardCode"].ToString(),
                                           CardName = row["CardName"].ToString()
                                       }).ToList();
                        return listado;
                    }
                    return new List<SocioNegocioEntity>();
                }
                catch (Exception ex)
                {
                    return new List<SocioNegocioEntity>();
                }
            }
        }

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
