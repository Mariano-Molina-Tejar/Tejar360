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
        private CommandType texto = CommandType.Text;
        private CommandType sp = CommandType.StoredProcedure;

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
                }
                ;
            }
            catch
            {
                throw;
            }
        }
        public async Task<IEnumerable<ReclutemientoEntity>> VerSolicitudesDePersonalFinalizadas()
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    var sp = "sp_ver_puestos_solicitados_finalizados";

                    return await conn.QueryAsync<ReclutemientoEntity>(sp, commandType: CommandType.Text, commandTimeout: 60);
                }
                ;
            }
            catch
            {
                throw;
            }
        }
        public async Task<IEnumerable<ReclutemientoEntity>> VerVerificacionDeNuevosPuestos()
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    var sp = "sp_ver_verificarcion_de_puestos_nuevos";

                    return await conn.QueryAsync<ReclutemientoEntity>(sp, commandType: CommandType.Text, commandTimeout: 60);
                }
                ;
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

        public async Task<IEnumerable<DetalleAspirantes>> ObtenerDetalleEnProceso(string userName)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    var sp = "sp_ver_detalle_en_proceso";

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

        public async Task<int> AgregarAspiranteAProceso(int userId)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var query = @"UPDATE [BOLSON_EMPLEOS_TEJAR].DBO.OUSR
                                SET Estado = 1
                                WHERE UserId = @UserId";

                return await conn.ExecuteAsync(query, new { UserId = userId });
            }
        }

        public async Task<int> RechazarAspirante(int userId, int estado = -1)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var query = @"UPDATE [BOLSON_EMPLEOS_TEJAR].DBO.OUSR
                                SET Estado = @Estado
                                WHERE UserId = @UserId";

                return await conn.ExecuteAsync(query, new { UserId = userId, Estado = estado });
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
        public async Task<IEnumerable<int>> ObtenerTranckingAspirante(int aspirante)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string query = $@"SELECT DISTINCT U_EstadoId FROM [ELTEJAR_PRUEBAS_21022026].DBO.[@GESTION_EMP_ER_DETA]
                                    WHERE U_AspiranteId = @Aspirante";

                return await connection.QueryAsync<int>(query, new { Aspirante = aspirante }, commandType: CommandType.Text, commandTimeout: 120);
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
                                FROM [ELTEJAR_PRUEBAS_21022026].[DBO].[@GESTION_EMP_CMNTRS] T0
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

        public async Task<PerfilPuestoModel> ObtenerPerfilDePuestoPorId(string idPuesto)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    var query = @"IF TRY_CAST(@IdPuesto AS INT) IS NOT NULL
                                    SELECT * FROM [ELTEJAR_PRUEBAS_21022026].[DBO].[@GESTION_EMP_PERFIL] T0
                                    INNER JOIN [ELTEJAR_PRUEBAS_21022026].[DBO].OHPS T1 ON T0.U_IdPuesto = t1.posID
                                    WHERE U_IdPuesto = @IdPuesto
                                    ELSE
                                    SELECT TOP 1 * FROM [ELTEJAR_PRUEBAS_21022026].[DBO].[@GESTION_EMP_PERFIL] T0
                                    LEFT JOIN [ELTEJAR_PRUEBAS_21022026].[DBO].OHPS T1 ON T0.U_IdPuesto = t1.posID
                                    WHERE T0.Name = @IdPuesto
                                    ORDER BY T0.Code DESC";

                    return await conn.QueryFirstAsync<PerfilPuestoModel>
                        (
                        query,
                        new { IdPuesto = idPuesto },
                        commandTimeout: 60
                        ) ?? new PerfilPuestoModel();
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> VerificarExistenciaDePerfil(int code)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    var query = @"IF (EXISTS (SELECT * FROM [ELTEJAR_PRUEBAS_21022026].[DBO].[@GESTION_EMP_PERFIL] WHERE code = @Code))
                                    SELECT 1
                                    ELSE SELECT 0";

                    return await conn.QuerySingleAsync<bool>(
                        query,
                        new { Code = code }
                        );
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> actualizarEstadoSolicitudDeAlta(int idSolicitud)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var query = @"UPDATE [ELTEJAR_PRUEBAS_21022026].[DBO].[@GESTION_EMP_A]
                              SET U_Estado = NULL
                              WHERE Code = @IdSolicitud";

                return await conn.ExecuteAsync(query, new { IdSolicitud = idSolicitud });
            }
        }
        public async Task<int> ActualizarCorreo(string usuario, string correo)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    var query = @"UPDATE [BOLSON_EMPLEOS_TEJAR].[DBO].OUSR
                                SET Email = @Correo
                                WHERE UserName = @Usuario";

                    return await conn.ExecuteAsync(
                        query,
                        new { Usuario = usuario, Correo = correo }
                        );
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<EmpleadoEntity>> ObtenerGerentes()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var query = @"SELECT DISTINCT U_IdEmpleado AS CodigoEmpleado, CONCAT(t1.firstName, ' ' , t1.lastName) AS Nombre
                            FROM [ELTEJAR_PRUEBAS_21022026].[DBO].[@GESTION_EMP_GER_DEP] T0
                            LEFT JOIN OHEM T1 ON T1.empID = t0.U_IdEmpleado
                            ORDER BY Nombre";
                return await conn.QueryAsync<EmpleadoEntity>(
                    query
                    );
            }
        }

        public async Task<bool> ExisteUsuario(string userCode)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var query = @"IF(EXISTS(SELECT * FROM [ELTEJAR_PRUEBAS_21022026].[DBO].OUSR WHERE USER_CODE = @User))
                            SELECT 1
                            ELSE SELECT 0";

                return await conn.ExecuteScalarAsync<bool>
                    (
                    query,
                    new { User = userCode }
                    );
            }
        }

        public async Task<IEnumerable<Tiendas>> ObtenerTiendas()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var query = @"SELECT WhsCode, WhsName 
                                FROM OWHS
                                WHERE WhsName LIKE 'Tienda%'";

                return await conn.QueryAsync<Tiendas>(
                    query
                    );
            }
        }

        public async Task<int> AgregarEmpleadoAlAspirante(int empId, string user)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    var query = @"UPDATE [BOLSON_EMPLEOS_TEJAR].[DBO].OUSR
                                SET EmpIdSAP = @EmpId
                                WHERE UserName = @User
                                ";

                    return await conn.ExecuteAsync(
                        query,
                        new { EmpId = empId, User = user }
                        );
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<DatosCreadosEmpleado> ObtenerDatosEmpleadoCreado(int empId)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var sp = "sp_obtener_datos_empleado_creado";

                return await conn.QueryFirstAsync<DatosCreadosEmpleado>(
                    sp,
                    new
                    {
                        EmpId = empId
                    }
                    );
            }
        }

        public async Task<string> ObtenerNombreCompleto(int userId)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var query = @"SELECT	CASE 
		                        WHEN CONCAT(OHEM.firstName, ' ' ,OHEM.LastName) = '' THEN OUSR.U_NAME
		                        ELSE CONCAT(OHEM.firstName, ' ' ,OHEM.LastName) END Nombre
	                        FROM OUSR
	                        LEFT JOIN OHEM ON OUSR.USERID = OHEM.userId
	                        WHERE OHEM.empID = @UserId";

                return await conn.ExecuteScalarAsync<string>
                    (
                    query,
                    new { UserId = userId }
                    );
            }
        }

        public string ObtenerPerfilJson(int idSolicitud, int puestoId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var sp = "sp_ver_perfil_json";
                var parametros = new DynamicParameters();
                parametros.Add("@IdPuesto", puestoId, DbType.Int32);
                parametros.Add("@IdSolicitud", idSolicitud, DbType.Int32);

                string json = connection.QueryFirstOrDefault<string>(
                    sp,
                    parametros,
                    commandType: CommandType.StoredProcedure
                );

                return string.IsNullOrEmpty(json) ? "[]" : json;
            }
        }

        public string ObtenerAspirantesJson(int solicitudId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var sp = "sp_ver_detalle_aspirantes_json";
                var parametros = new DynamicParameters();
                parametros.Add("@SolicitudId", solicitudId, DbType.Int32);

                string json = connection.QueryFirstOrDefault<string>(
                    sp,
                    parametros,
                    commandType: CommandType.StoredProcedure
                );

                return string.IsNullOrEmpty(json) ? "[]" : json;
            }
        }

        public async Task<bool> VerificarAnalisisIA(int idSolicitud)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var query = @"IF (EXISTS (
                                SELECT * FROM [BOLSON_EMPLEOS_TEJAR].DBO.[OUSR] T0
                                LEFT JOIN [ELTEJAR_PRUEBAS_21022026].DBO.[@GESTION_EMP_EV_IA] T1 ON T0.UserId = T1.Code AND T1.U_IdSolicitud = @IdSOlicitud
                                WHERE T0.IdSolicitudAlta = @IdSOlicitud AND T1.Code IS NULL
                                ))
                                SELECT 1
                                ELSE
                                SELECT 0";

                return await conn.ExecuteScalarAsync<bool>(query, new { IdSOlicitud = idSolicitud });
            }
        }

        public async Task<IEnumerable<FortalezasDebilidades>> ObtenerAnalisisAspirantesIA(int userId)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var query = @"SELECT  Observaciones,Tipo, U_Observaciones AS ObservacionesGenerales FROM (
                                SELECT	U_Observaciones AS Observaciones ,
		                                1 AS Tipo
                                FROM [ELTEJAR_PRUEBAS_21022026].DBO.[@GESTION_EMP_FOR_IA] T0
                                WHERE T0.U_UserId = @UserId 
                                UNION ALL 
                                SELECT	U_Observaciones AS Observaciones ,
		                                0 AS Tipo
                                FROM [ELTEJAR_PRUEBAS_21022026].DBO.[@GESTION_EMP_DEB_IA] T0
                                WHERE T0.U_UserId = @UserId 
                                )TB
                                LEFT JOIN [ELTEJAR_PRUEBAS_21022026].DBO.[@GESTION_EMP_EV_IA] T0 ON T0.Code = @UserId";

                return await conn.QueryAsync<FortalezasDebilidades>(query, new { UserId = userId });
            }
        }

        public async Task<string> ObtenerObservacionIA(int idSolicitud)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var query = @"SELECT U_RecomendacionFinalIA FROM [ELTEJAR_PRUEBAS_21022026].DBO.[@GESTION_EMP_A]
                                WHERE U_IdSolicitudBaja = @IdSolicitud";

                return await conn.ExecuteScalarAsync<string>(query, new { IdSolicitud = idSolicitud }) ?? "";
            }
        }
    }
}