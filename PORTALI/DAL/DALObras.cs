using Connection;
using Dapper;
using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DALObras
    {
        public static async Task<List<ListadoVisitasContactosEntity>> ListadoContactos(int NoVisita)
        {
            try
            {
                var pConnection = Conexion.ConexionDB();
                string connectionString = $"Server={pConnection.ServerName};Database={pConnection.DataBase};Uid={pConnection.User};Pwd={pConnection.Password};";

                using (var connection = new SqlConnection(connectionString))
                {
                    string sp = "sp_crm_prospeccion_lista_contactos";
                    return (List<ListadoVisitasContactosEntity>)
                        await connection.QueryAsync<ListadoVisitasContactosEntity>(sp,
                        new { @NoVisita = NoVisita },
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: 120);
                }
            }
            catch (Exception ex)
            {
                return new List<ListadoVisitasContactosEntity>();
            }
        }
        public static async Task<List<ProspListadoVisitasEntity>> ListadoVisitas(int IdUser)
        {
            try
            {
                var pConnection = Conexion.ConexionDB();
                string connectionString = $"Server={pConnection.ServerName};Database={pConnection.DataBase};Uid={pConnection.User};Pwd={pConnection.Password};";

                using (var connection = new SqlConnection(connectionString))
                {
                    string sp = "sp_crm_prospeccion_listado_visitas";
                    return (List<ProspListadoVisitasEntity>)
                        await connection.QueryAsync<ProspListadoVisitasEntity>(sp,
                        new { @IdUser = IdUser },
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: 120);
                }
            }
            catch (Exception ex)
            {
                return new List<ProspListadoVisitasEntity>();
            }
        }
        public static async Task<List<PlanificacionDetalleModel>> DetallePlanificacion(int IdPlan)
        {
            try
            {
                var pConnection = Conexion.ConexionDB();
                string connectionString = $"Server={pConnection.ServerName};Database={pConnection.DataBase};Uid={pConnection.User};Pwd={pConnection.Password};";

                using (var connection = new SqlConnection(connectionString))
                {
                    string sp = "sp_crm_listado_planficacion";
                    return (List<PlanificacionDetalleModel>)
                        await connection.QueryAsync<PlanificacionDetalleModel>(sp,
                        new { @IdPlan = IdPlan },
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: 120);
                }
            }
            catch (Exception ex)
            {
                return new List<PlanificacionDetalleModel>();
            }
        }

        public static List<ListadoAgendaEntity> ListadoAgenda(int? IdRegion = null, string WhsCode = null, int? SlpCode = null, DateTime? FechaI = null, DateTime? FechaF = null, int? UserId = null)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<ListadoAgendaEntity> listado = new List<ListadoAgendaEntity>();

            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                using (OleDbCommand iCommand = new OleDbCommand("sp_crm_listado_agenda", iConnection))
                {
                    iCommand.CommandType = CommandType.StoredProcedure;
                    iCommand.Parameters.AddWithValue("@IdRegion", (object)IdRegion ?? DBNull.Value);
                    iCommand.Parameters.AddWithValue("@WhsCode", (object)WhsCode ?? DBNull.Value);
                    iCommand.Parameters.AddWithValue("@SlpCode", (object)SlpCode ?? DBNull.Value);
                    iCommand.Parameters.AddWithValue("@UserId", UserId);
                    //iCommand.Parameters.AddWithValue("@FechaI", (object)FechaI ?? DBNull.Value);
                    //iCommand.Parameters.AddWithValue("@FechaF", (object)FechaF ?? DBNull.Value);                    

                    try
                    {
                        DataTable dt = new DataTable();
                        using (OleDbDataAdapter iDAResult = new OleDbDataAdapter(iCommand))
                        {
                            iDAResult.Fill(dt);
                        }

                        listado = (from row in dt.AsEnumerable()
                                   select new ListadoAgendaEntity
                                   {
                                       Id = Convert.ToInt32(row["Id"]),
                                       IdRegion = Convert.ToInt32(row["IdRegion"]),
                                       Region = row["Region"].ToString(),
                                       SlpCode = Convert.ToInt32(row["SlpCode"]),
                                       SlpName = row["SlpName"].ToString(),
                                       WhsCode = row["WhsCode"].ToString(),
                                       WhsName = row["WhsName"].ToString(),
                                       IdDepto = Convert.ToInt32(row["IdDepto"]),
                                       IdMunicipio = Convert.ToInt32(row["IdMunicipio"]),
                                       IdColonia = row["IdColonia"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["IdColonia"]),
                                       IdZona = Convert.ToInt32(row["IdZona"]),
                                       AreaAsignada = row["AreaAsignada"].ToString(),
                                       FechaInicio = row["FechaInicio"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["FechaInicio"]),
                                       FechaFinal = row["FechaFinal"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["FechaFinal"]),
                                       Notas = row["Notas"] == DBNull.Value ? null : row["Notas"].ToString(),
                                       Estado = row["Estado"].ToString(),
                                       EstadoPlay = Convert.ToBoolean(row["EstadoPlay"].ToString()),
                                       Enableds = Convert.ToBoolean(row["Enableds"].ToString()),
                                       IdPunto = int.Parse(row["IdPunto"].ToString())
                                   }).ToList();

                        return listado;
                    }
                    catch (Exception ex)
                    {
                        // Puedes loggear ex.Message si deseas
                        return new List<ListadoAgendaEntity>();
                    }
                }
            }
        }
    }
}
