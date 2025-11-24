using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connection;
using Entity;
using Dapper;
using System.Data.SqlClient;
using System.Data;

namespace DAL
{
    public class DALReclutamiento
    {
        private readonly string connectionString;
        CommandType texto = CommandType.Text;
        CommandType sp = CommandType.StoredProcedure;
        public DALReclutamiento()
        {
            ConnectionEntity pConnection = Conexion.ConexionDB();
            connectionString = $"Server={pConnection.ServerName};Database={pConnection.DataBase};Uid={pConnection.User};Pwd={pConnection.Password};";
        }

        public async Task<IEnumerable<ReclutemientoEntity>> VerSolicitudesDePersonal()
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    var sp = "sp_ver_puestos_solicitados";

                    return await conn.QueryAsync<ReclutemientoEntity>(sp, commandType: CommandType.Text, commandTimeout: 60);
                };
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> InsertUsuarioLanding(string usuario, string password, int idSolicitudAlta, string correo, string nombre)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    var query = @"INSERT INTO [BOLSON_EMPLEOS_TEJAR].[DBO].OUSR (UserName, [PassWord] ,IdSolicitudAlta,Email,NombreAspirante)
                                    VALUES (@Usuario, @Password, @IdSolicitudAlta, @Email, @Nombre)";

                    return await conn.ExecuteAsync(
                        sql: query,
                        param: new
                        {
                            Usuario = usuario,
                            Password = password,
                            IdSolicitudAlta = idSolicitudAlta,
                            Email = correo,
                            Nombre = nombre
                        },
                        commandTimeout: 60,
                        commandType: texto
                        )
                        .ConfigureAwait(false);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error en la base de datos", ex);
            }
        }

        public async Task<int> ObtenerCodigoSiguienUsuario(string userName)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    var query = @"SELECT COUNT(*) + 1 FROM [BOLSON_EMPLEOS_TEJAR].[DBO].OUSR
                                WHERE UserName LIKE CONCAT(@UserName,'%')";

                    return await conn.ExecuteScalarAsync<int>(
                        sql: query,
                        param: new { userName = userName },
                        commandTimeout: 60,
                        commandType: texto
                        );
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al obtener el utlimo usuario", ex);
            }
        }

        public async Task<IEnumerable<DetalleAspirantes>> ObtenerDetalleAspirantes(string userName)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    var sp = "sp_ver_detalle_aspirantes";

                    return await conn.QueryAsync<DetalleAspirantes>(
                        sql: sp,
                        param: new { UserName = userName },
                        commandTimeout: 120,
                        commandType: CommandType.StoredProcedure
                        );
                }
            }
            catch
            {
                throw;
            }
        }
        public async Task<DetalleAspirantes> ObtenerDetalleAspirante(string userName)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    var sp = "sp_ver_detalle_aspirante";

                    return await conn.QuerySingleAsync<DetalleAspirantes>(
                        sql: sp,
                        param: new { UserName = userName },
                        commandTimeout: 120,
                        commandType: CommandType.StoredProcedure
                        );
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<string> verClaveUsuario(string userName)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    var query = @"SELECT PassWord FROM [BOLSON_EMPLEOS_TEJAR].[DBO].OUSR
                                    WHERE UserName = @UserName";

                    return await conn.QuerySingleOrDefaultAsync<string>(
                        sql: query,
                        param: new { UserName = userName },
                        commandType: CommandType.Text,
                        commandTimeout: 120
                        ).ConfigureAwait(false) ?? "";
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<DatosAspirantesViewModel> ObtenerDatosPersonalesAspirante(string userName)
        {
            try
            {
                var DatosAspirante = new DatosAspirantesViewModel();

                using (var conn = new SqlConnection(connectionString))
                {
                    var queryDatosPersonales = @"SELECT * FROM [BOLSON_EMPLEOS_TEJAR].[DBO].OPER
                                WHERE Code = (SELECT UserId FROM [BOLSON_EMPLEOS_TEJAR].[DBO].OUSR WHERE UserName = @UserName)";

                    var queryDatosAcademicos = @"SELECT * FROM [BOLSON_EMPLEOS_TEJAR].[DBO].PER1
                                WHERE Code = (SELECT UserId FROM [BOLSON_EMPLEOS_TEJAR].[DBO].OUSR WHERE UserName = @UserName)";

                    var queryDatosLaborales = @"SELECT * FROM [BOLSON_EMPLEOS_TEJAR].[DBO].PER2
                                WHERE Code = (SELECT UserId FROM [BOLSON_EMPLEOS_TEJAR].[DBO].OUSR WHERE UserName = @UserName)";

                    DatosAspirante.DatosPersonalesVM = await conn.QuerySingleOrDefaultAsync<DatosPersonales>(
                        sql: queryDatosPersonales,
                        param: new { UserName = userName },
                        commandType: CommandType.Text,
                        commandTimeout: 120
                        )
                        .ConfigureAwait(false)
                        ?? new DatosPersonales();

                    DatosAspirante.DatosAcademicosVM = (List<DatosAcademicos>)await conn.QueryAsync<DatosAcademicos>(
                        sql: queryDatosAcademicos,
                        param: new { UserName = userName },
                        commandType: CommandType.Text,
                        commandTimeout: 120
                        ).ConfigureAwait(false) ?? new List<DatosAcademicos>();

                    DatosAspirante.DatosLaboralesVM = (List<DatosLaborales>)await conn.QueryAsync<DatosLaborales>(
                       sql: queryDatosLaborales,
                       param: new { UserName = userName },
                       commandType: CommandType.Text,
                       commandTimeout: 120
                       ).ConfigureAwait(false) ?? new List<DatosLaborales>();

                    return DatosAspirante;
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<CVDOCREQ>> VerDocumentosRequeridos()
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string query = $"SELECT * FROM [BOLSON_EMPLEOS_TEJAR].[DBO].CVDOCREQ";

                    return (List<CVDOCREQ>)await connection.QueryAsync<CVDOCREQ>(query, commandType: CommandType.Text, commandTimeout: 120);
                }
            }
            catch (Exception ex)
            {
                return new List<CVDOCREQ>
                {
                    new CVDOCREQ {ErrorMessage = ex.Message }
                };
            }
        }

        public async Task<IEnumerable<Comentarios>> ObtenerComentariosAspitantes(int usuario)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    var query = @"SELECT	U_Comentario AS Comentario,
		                                    CONVERT(DATE,U_Fecha) as Fecha,
		                                    CONCAT(T1.firstName,' ',T1.lastName ) AS ComentarioPor
                                FROM [ELTEJAR_PRUEBAS_R1_5.1].[DBO].[@GESTION_EMP_CMNTRS] T0
                                LEFT JOIN OHEM T1 ON T1.userId =T0.U_UsuarioComentario
                                WHERE T0.U_usuario = @Usuario
                                ORDER BY T0.Code DESC";

                    return await conn.QueryAsync<Comentarios>(
                        sql: query,
                        param: new { Usuario = usuario },
                        commandType: CommandType.Text,
                        commandTimeout: 120
                        ).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<int> ObtenerCodigoUsuario(string userName)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    var query = "SELECT UserId FROM [BOLSON_EMPLEOS_TEJAR].[DBO].OUSR WHERE UserName = @UserName";

                    return await conn.ExecuteScalarAsync<int>(query, new { UserName = userName });
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
